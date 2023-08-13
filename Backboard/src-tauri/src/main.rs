// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

use std::ops::Deref;

use api::apis::configuration::{ApiKey, Configuration};
use api::apis::import_api::api_import_reset_key_password_put;
use api::apis::status_api::api_status_service_status_get;
use api::apis::Error;
use api::models::{ImportUpdateResetKeyPasswordRequestBody, StatusViewServiceStatusResponse};
use calamine::{
    open_workbook, DataType, DeError, Error as CalamineError, RangeDeserializer,
    RangeDeserializerBuilder, Reader, Xlsx, XlsxError,
};
use serde::{Deserialize, Serialize};
use tauri::http::status::StatusCode;
use tauri_plugin_autostart::MacosLauncher;

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
async fn import_grades(file_path: String) -> Result<(), String> {
    println!("importGrades: {}", file_path);

    process_excel_file(file_path).map_err(|err: CalamineError| err.to_string())?;

    Ok(())
}

fn de_opt_string<'de, D>(deserializer: D) -> Result<Option<String>, D::Error>
where
    D: serde::Deserializer<'de>,
{
    let data_type = calamine::DataType::deserialize(deserializer);
    match data_type {
        Ok(calamine::DataType::Error(e)) => Ok(None),
        Ok(calamine::DataType::Float(f)) => Ok(Some(f.to_string())),
        Ok(calamine::DataType::Int(i)) => Ok(Some(i.to_string())),
        Ok(calamine::DataType::String(s)) => Ok(Some(s)),
        Ok(calamine::DataType::DateTime(d)) => Ok(Some(d.to_string())),
        _ => Ok(None),
    }
}

#[derive(Debug, Clone, Deserialize, Serialize)]
struct ExcelRow {
    #[serde(rename(deserialize = "Tárgy kategória", serialize = "SubjectCategory"))]
    subject_category: String,
    #[serde(rename(deserialize = "Tantárgy", serialize = "Subject"))]
    subject: String,
    #[serde(rename(deserialize = "Osztály/Csoport név", serialize = "Group"))]
    group: String,
    #[serde(
        rename(deserialize = "Pedagógus név", serialize = "Teacher"),
        deserialize_with = "de_opt_string",
        default
    )]
    teacher: Option<String>,
    #[serde(
        rename(deserialize = "Értékelés módja", serialize = "Type"),
        deserialize_with = "de_opt_string",
        default
    )]
    grade_type: Option<String>,
    #[serde(rename(deserialize = "Osztályzat", serialize = "TextGrade"))]
    text_grade: String,
    #[serde(
        rename(deserialize = "Jegy", serialize = "Grade"),
        deserialize_with = "de_opt_string",
        default
    )]
    grade: Option<String>,
    #[serde(rename(deserialize = "Szöveges értékelés", serialize = "ShortTextGrade"))]
    short_text_grade: String,
    #[serde(rename(deserialize = "Magatartás", serialize = "BehaviorGrade"))]
    behavior_grade: String,
    #[serde(rename(deserialize = "Szorgalom", serialize = "DiligenceGrade"))]
    diligence_grade: String,
    #[serde(rename(deserialize = "Bejegyzés dátuma", serialize = "CreateDate"))]
    create_date: String,
    #[serde(rename(deserialize = "Rögzítés dátuma", serialize = "RecordDate"))]
    record_date: String,
    #[serde(rename(deserialize = "Tanuló név", serialize = "Name"))]
    name: String,
    #[serde(rename(deserialize = "Tanuló azonosítója"), skip_serializing)]
    om_code: String,
}

fn process_excel_file(file_path: String) -> Result<(), CalamineError> {
    let mut workbook: Xlsx<_> = open_workbook(file_path)?;
    let range = workbook
        .worksheet_range("Évközi jegyek")
        .ok_or(CalamineError::Msg("Cannot find sheet: 'Évközi jegyek'"))??;

    let mut iter = RangeDeserializerBuilder::new().from_range::<_, ExcelRow>(&range)?;

    if let Some(result) = iter.next() {
        println!("{:?}", serde_json::to_string_pretty(&result.unwrap()));
        Ok(())
    } else {
        Err(From::from("expected at least one record but got none"))
    }
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
