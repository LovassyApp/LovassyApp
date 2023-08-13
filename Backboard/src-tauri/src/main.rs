// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

mod grades_processor;

use api::apis::configuration::{ApiKey, Configuration};
use api::apis::import_api::api_import_reset_key_password_put;
use api::apis::status_api::api_status_service_status_get;
use api::apis::Error;
use api::models::{ImportUpdateResetKeyPasswordRequestBody, StatusViewServiceStatusResponse};
use tauri::http::status::StatusCode;
use tauri_plugin_autostart::MacosLauncher;

use crate::grades_processor::process_excel_file;

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
    file_path: String,
    reset_key_password: String,
    import_key: String,
    update_reset_key_password: bool,
) -> Result<(), String> {
    println!("importGrades: {}", file_path);

    let grades = process_excel_file(file_path).map_err(|err: calamine::Error| err.to_string())?;

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
