// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

mod cryptography;
mod grades_processor;

use std::collections::HashMap;

use api::apis::configuration::{ApiKey, Configuration};
use api::apis::import_api::{
    api_import_grades_user_id_post, api_import_reset_key_password_put, api_import_users_get,
};
use api::apis::status_api::api_status_service_status_get;
use api::apis::Error;
use api::models::{
    ImportImportGradesRequestBody, ImportUpdateResetKeyPasswordRequestBody,
    StatusViewServiceStatusResponse,
};
use tauri::http::status::StatusCode;
use tauri::Window;
use tauri_plugin_autostart::MacosLauncher;

use crate::grades_processor::process_excel_file;
use crate::grades_processor::BackboardGrade;
use crate::grades_processor::GradeCollection;

use crate::cryptography::{hash, kyber_encrypt};

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

    api_import_reset_key_password_put(
        &config,
        Some(ImportUpdateResetKeyPasswordRequestBody::new(
            reset_key_password,
        )),
    )
    .await
    .map_err(|e| match e {
        //Match 401 error
        Error::ResponseError(response_error) => {
            if let StatusCode::UNAUTHORIZED = response_error.status {
                "401".to_string()
            } else {
                "error".to_string() // TODO: maybe find a better way to handle this (not important for now)
            }
        }
        _ => "error".to_string(), // TODO: maybe find a better way to handle this (not important for now)
    })
}

#[tauri::command]
async fn import_grades(
    window: Window,
    file_path: String,
    blueboard_url: String,
    reset_key_password: String,
    import_key: String,
    update_reset_key_password: bool,
) -> Result<(), String> {
    if (update_reset_key_password) {
        upload_reset_key_password(
            blueboard_url.clone(),
            reset_key_password,
            import_key.clone(),
        )
        .await?;
    }

    let mut config = Configuration::new();
    config.base_path = blueboard_url;
    config.api_key = Some(ApiKey {
        prefix: None,
        key: import_key,
    });

    let users = api_import_users_get(&config.clone(), None, None, None, None)
        .await
        .map_err(|e| match e {
            Error::ResponseError(response_error) => {
                if let StatusCode::UNAUTHORIZED = response_error.status {
                    "401".to_string()
                } else {
                    "error".to_string() // TODO: maybe find a better way to handle this (not important for now)
                }
            }
            _ => "error".to_string(), // TODO: maybe find a better way to handle this (not important for now)
        })?;

    window.emit("import-users", &users.len()).unwrap();

    let grades = process_excel_file(file_path).map_err(|err: calamine::Error| err.to_string())?;

    let mut grade_map: HashMap<String, Vec<BackboardGrade>> = HashMap::new();

    for grade in grades {
        grade_map
            .entry(hash(grade.clone().om_code))
            .or_insert(Vec::new())
            .push(grade);
    }

    let mut count = 0;
    for user in &users {
        let user_grades = grade_map.get(&user.clone().om_code_hashed.unwrap().unwrap());

        match user_grades {
            None => continue,
            Some(_) => {
                let public_key = user.public_key.clone().unwrap().unwrap();

                let school_class: Option<String> = user_grades
                    .unwrap()
                    .iter()
                    .find(|grade| grade.school_class.is_some())
                    .map(|grade| grade.school_class.clone().unwrap());

                let student_name = user_grades.unwrap()[0].student_name.clone();

                let grade_collection = GradeCollection {
                    grades: user_grades.unwrap().clone(),
                    school_class: school_class,
                    student_name: student_name,
                    user: user.clone().into(),
                };

                let grade_collection_encrypted = kyber_encrypt(
                    serde_json::to_string(&grade_collection).unwrap(),
                    public_key,
                )
                .map_err(|e| e.to_string())?;

                api_import_grades_user_id_post(
                    &config,
                    &user.id.unwrap().to_string(),
                    Some(ImportImportGradesRequestBody {
                        json_encrypted: grade_collection_encrypted,
                    }),
                )
                .await
                .map_err(|e| match e {
                    Error::ResponseError(response_error) => {
                        if let StatusCode::UNAUTHORIZED = response_error.status {
                            "401".to_string()
                        } else {
                            "error".to_string() // TODO: maybe find a better way to handle this (not important for now)
                        }
                    }
                    _ => "error".to_string(), // TODO: maybe find a better way to handle this (not important for now)
                })?;
            }
        }
        count += 1;
        window
            .emit("import-progress", (count / &users.len()) * 100)
            .unwrap();
    }

    Ok(())
}

#[tauri::command]
async fn status(blueboard_url: String) -> Result<StatusViewServiceStatusResponse, String> {
    let mut config = Configuration::new();
    config.base_path = blueboard_url;

    api_status_service_status_get(&config)
        .await
        .map_err(|e| match e {
            _ => "error".to_string(), // TODO: maybe find a better way to handle this (not important for now)
        })
}

fn main() {
    tauri::Builder::default()
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
        .expect("error while running tauri application");
}
