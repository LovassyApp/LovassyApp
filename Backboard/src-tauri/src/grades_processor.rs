use api::models::ImportIndexUsersResponse;
use calamine::{
    open_workbook, DataType, DeError, Error as CalamineError, RangeDeserializer,
    RangeDeserializerBuilder, Reader, Xlsx, XlsxError,
};
use serde::{Deserialize, Serialize};
use tauri::http::status::StatusCode;
use tauri_plugin_autostart::MacosLauncher;

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
pub struct BackboardGrade {
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
    pub name: String,
    #[serde(rename(deserialize = "Tanuló azonosítója"), skip_serializing)]
    pub om_code: String,
    #[serde(rename(deserialize = "Tanuló osztálya"), skip_serializing)]
    pub school_class: Option<String>,
}

pub fn process_excel_file(file_path: String) -> Result<Vec<BackboardGrade>, CalamineError> {
    let mut workbook: Xlsx<_> = open_workbook(file_path)?;
    let range = workbook
        .worksheet_range("Évközi jegyek")
        .ok_or(CalamineError::Msg("Cannot find sheet: 'Évközi jegyek'"))??;

    let mut iter = RangeDeserializerBuilder::new()
        .from_range::<_, BackboardGrade>(&range)?
        .enumerate();

    let mut grades = Vec::new();
    while let Some((_, Ok(grade))) = iter.next() {
        grades.push(grade);
    }

    Ok(grades)
}

#[derive(Debug, Clone, Serialize)]
pub struct BackboardUser {
    #[serde(rename(serialize = "Id"))]
    id: String,
    #[serde(rename(serialize = "PublicKey"))]
    public_key: String,
    #[serde(rename(serialize = "OmCodeHashed"))]
    om_code_hashed: String,
}

impl From<ImportIndexUsersResponse> for BackboardUser {
    fn from(user: ImportIndexUsersResponse) -> Self {
        BackboardUser {
            id: user.id.unwrap().to_string(),
            public_key: user.public_key.unwrap().unwrap(),
            om_code_hashed: user.om_code_hashed.unwrap().unwrap(),
        }
    }
}

pub struct GradeCollection {
    grades: Vec<BackboardGrade>,
    school_class: String,
    student_name: String,
    user: BackboardUser,
}
