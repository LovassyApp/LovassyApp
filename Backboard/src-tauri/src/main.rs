// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

use api::apis::configuration::{ApiKey, Configuration};
use api::apis::import_api::{api_import_reset_key_password_put, ApiImportResetKeyPasswordPutError};
use api::apis::status_api::api_status_service_status_get;
use api::apis::Error;
use api::models::{ImportUpdateResetKeyPasswordRequestBody, StatusViewServiceStatusResponse};
use tauri::http::status::StatusCode;

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
        .plugin(tauri_plugin_store::Builder::default().build())
        .invoke_handler(tauri::generate_handler![status, upload_reset_key_password])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
