#![doc = include_str!("../../README.md")]
// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

mod cryptography;
mod grades_processor;

use api::apis::Error;
use api::apis::configuration::{ApiKey, Configuration};
use api::apis::import_api::{
    api_import_grades_user_id_post, api_import_reset_key_password_put, api_import_users_get,
};
use api::apis::status_api::api_status_service_status_get;
use api::models::{
    ImportImportGradesRequestBody, ImportUpdateResetKeyPasswordRequestBody,
    StatusViewServiceStatusResponse,
};
use grades_processor::{GradeCollection, process_grades_csv_file, process_students_csv_file};
use std::collections::HashMap;
use tauri::{Emitter, Window};
use tauri_plugin_autostart::MacosLauncher;

/// extract http error status code from the [api error](api::apis::Error) if available, otherwise convert it into a string
fn handle_api_err<E: std::fmt::Debug>(e: Error<E>) -> String {
    log::error!("error: {e:#?}");
    match e {
        Error::ResponseError(response_error) => response_error.status.as_str().to_string(),
        other_err => other_err.to_string(),
    }
}

/// upload the `reset_key_password` to the server at `blueboard_url`, using the `import_key`
/// # Errors
/// invalid `import_key`, something with the PUT request
#[tauri::command]
async fn upload_reset_key_password(
    blueboard_url: String,
    reset_key_password: String,
    import_key: String,
) -> Result<(), String> {
    let mut config = Configuration::new();
    config.base_path = blueboard_url;
    config.api_key = Some(ApiKey {
        prefix: None,
        key: import_key,
    });

    log::info!("uploading reset key password");
    api_import_reset_key_password_put(
        &config,
        Some(ImportUpdateResetKeyPasswordRequestBody::new(
            reset_key_password,
        )),
    )
    .await
    .map_err(handle_api_err)
}

/// upload all the new [grades][grades_processor::BackboardGrade] to each registered user's account on the server\
/// **NOTE**: imported data won't be visible right away, check out the server code to see what happens ;)\
/// if `students_file_path` is provided: upload|update the information of the students\
/// if `update_rest_key_password`: upload the `reset_key_password`
/// # Errors
/// - coming from [`upload_reset_key_password`]
/// - invalid `import_key`
/// - something with the PUT or GET requests
/// - coming from [`process_students_csv_file`] and/or [`process_grades_csv_file`]
#[tauri::command]
async fn import_grades(
    window: Window,
    grades_file_path: String,
    students_file_path: Option<String>,
    blueboard_url: String,
    reset_key_password: String,
    import_key: String,
    update_reset_key_password: bool,
) -> Result<(), String> {
    log::info!("importing grades");
    if update_reset_key_password {
        upload_reset_key_password(
            blueboard_url.clone(),
            reset_key_password,
            import_key.clone(),
        )
        .await?;
        log::info!("successfully uploaded reset key password");
    }

    let config = Configuration {
        base_path: blueboard_url,
        api_key: Some(ApiKey {
            prefix: None,
            key: import_key,
        }),
        ..Configuration::new()
    };

    // fetches data of users(already registered students) from the server, will add imported data to these later
    let srv_users = api_import_users_get(&config, None, None, None, None)
        .await
        .map_err(handle_api_err)?;
    let num_srv_users = srv_users.len();
    log::info!("users fetched from server already there ({num_srv_users})");
    log::trace!("{srv_users:?}");

    window.emit("import-users", num_srv_users).unwrap(); // GUI report

    let imported_grade_map =
        process_grades_csv_file(grades_file_path).map_err(|err| err.to_string())?;

    let imported_student_info_map = if let Some(path) = students_file_path {
        process_students_csv_file(path).map_err(|err| err.to_string())?
    } else {
        HashMap::new() // leave it empty if file path not provided
    };

    let mut processed_n = 0; // number of users already processed
    for srv_usr in srv_users {
        let hashed_om = &srv_usr.om_code_hashed.clone().unwrap().unwrap(); // used as key to its data
        let Some(user_grades) = imported_grade_map.get(hashed_om) else {
            log::warn!("no imported grades, skipping server user with id {hashed_om:?}");
            continue;
        };
        log::debug!("processing {processed_n}. user: {srv_usr:?}");
        log::trace!("user's freshly imported grades: {user_grades:?}");

        // extract student info from data provided, fall back to grades sometimes containing it
        let (school_class, student_name) =
            if let Some(student_info) = &imported_student_info_map.get(hashed_om) {
                (Some(&student_info.class), &student_info.name)
            } else {
                log::warn!("user not found in students' data, falling back to grades");
                let cls = user_grades.iter().find_map(|g| g.school_class.as_ref());
                (cls, &user_grades[0].student_name)
            };
        log::debug!("user's info: {student_name} ({school_class:?})");

        // pack useful information about user to be sent
        let grade_collection = GradeCollection {
            grades: user_grades.clone(),
            school_class: school_class.cloned(),
            student_name: student_name.clone(),
            user: srv_usr.clone().into(),
        };
        log::trace!("user's grade collection: {grade_collection:?}");

        let pub_key = srv_usr.public_key.clone().unwrap().unwrap(); // public key used for encryption
        log::info!("posting user's data");
        api_import_grades_user_id_post(
            &config,
            &srv_usr.id.unwrap().to_string(),
            Some(ImportImportGradesRequestBody {
                json_encrypted: grade_collection.to_encrypted_json(pub_key)?,
            }),
        )
        .await
        .map_err(handle_api_err)?;
        log::info!("successfully posted user's data");

        processed_n += 1;
        window
            .emit("import-progress", (processed_n / num_srv_users) * 100)
            .unwrap(); // GUI progress report
    }
    let imported_n = imported_grade_map.len();
    if processed_n != imported_n {
        log::warn!(
            "number of processed users ({processed_n} != {imported_n}) number of imported users, there are {} ignored ones",
            imported_n - processed_n
        );
    }

    Ok(())
}

/// GET status of server
/// # Errors
/// request
#[tauri::command]
async fn status(blueboard_url: String) -> Result<StatusViewServiceStatusResponse, String> {
    let mut config = Configuration::new();
    config.base_path = blueboard_url;
    log::info!("requesting service status");

    let res = api_status_service_status_get(&config)
        .await
        .map_err(handle_api_err);
    log::trace!("service status: {res:?}");
    res
}

fn main() {
    let log_p = std::path::Path::new(".lovassyapp-backboard.log");
    ftail::Ftail::new()
        .console_env_level()
        .single_file_env_level(log_p, true)
        .init()
        .unwrap(); // logs to `stderr` and file at runtime dir as well

    tauri::Builder::default()
        .plugin(tauri_plugin_global_shortcut::Builder::new().build())
        .plugin(tauri_plugin_autostart::Builder::new().build())
        .plugin(tauri_plugin_dialog::init())
        .plugin(tauri_plugin_http::init())
        .plugin(tauri_plugin_autostart::init(
            MacosLauncher::LaunchAgent,
            None,
        ))
        .plugin(tauri_plugin_store::Builder::default().build())
        .invoke_handler(tauri::generate_handler![
            status,
            upload_reset_key_password,
            import_grades
        ])
        .run(tauri::generate_context!())
        .expect("encountered an unexpected, fatal error while running Tauri application");
}
