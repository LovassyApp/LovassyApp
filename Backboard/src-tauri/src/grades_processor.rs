//! # Grades Processor
//! provides bindings and functions necessary to import grades and user(student) data\
//! also processes imported data to match the format of the server, where it will be sent to

use api::models::ImportIndexUsersResponse;
use serde::{Deserialize, Serialize};
use std::collections::HashMap;

/// # Backboard Grade
/// bindings to parse a grade, that comes from an [E-Kreta](https://e-kreta.hu) export created by a school admin\
/// **NOTE**: skipped fields on deserialization are: "Születési idő", "Utolsó mentés dátuma", "Százalékos értékelés"
#[derive(Debug, Clone, Deserialize, Serialize)]
#[serde(rename_all = "PascalCase")]
pub struct BackboardGrade {
    #[serde(rename(deserialize = "Tanuló név"))]
    pub student_name: String,
    #[serde(rename(deserialize = "Tanuló osztálya"), skip_serializing)]
    pub school_class: Option<String>,
    #[serde(rename(deserialize = "Tanuló azonosítója"), skip_serializing)]
    om_code: String,
    #[serde(rename(deserialize = "Tárgy kategória"))]
    subject_category: String,
    #[serde(rename(deserialize = "Tantárgy"))]
    subject: String,
    #[serde(rename(deserialize = "Osztály/Csoport név"))]
    group: String,
    #[serde(rename(deserialize = "Pedagógus név"), default)]
    teacher: Option<String>,
    #[serde(rename(deserialize = "Téma"))]
    theme: String,
    #[serde(rename(deserialize = "Értékelés módja"), default)]
    r#type: Option<String>,
    #[serde(rename(deserialize = "Osztályzat"))]
    text_grade: String,
    #[serde(rename(deserialize = "Jegy"), default)]
    grade: Option<String>,
    #[serde(rename(deserialize = "Szöveges értékelés"))]
    short_text_grade: String,
    #[serde(rename(deserialize = "Magatartás"))]
    behavior_grade: String,
    #[serde(rename(deserialize = "Szorgalom"))]
    diligence_grade: String,
    #[serde(rename(deserialize = "Bejegyzés dátuma"))]
    create_date: String,
    #[serde(rename(deserialize = "Rögzítés dátuma"))]
    record_date: String,
}
impl BackboardGrade {
    /// returns the hashed `om_code`, then replaces it with an empty string
    pub fn hashed_om_code(&mut self) -> String {
        crate::cryptography::hash(&std::mem::take(&mut self.om_code))
    }
}

#[derive(Debug, Clone, Deserialize)]
pub struct BackboardStudent {
    #[serde(rename(deserialize = "Név"))]
    pub name: String,
    #[serde(rename(deserialize = "Oktatási azonosítója"))]
    om_code: String,
    #[serde(rename(deserialize = "Osztály"))]
    pub class: String,
}
impl BackboardStudent {
    /// returns the hashed `om_code`, then replaces it with an empty string
    pub fn hashed_om_code(&mut self) -> String {
        crate::cryptography::hash(&std::mem::take(&mut self.om_code))
    }
}

/// reads, parses and processes a csv grades export from the `path`\
/// a valid example can be found [here](../test_grades.csv)\
/// **NOTE**: the csv shall use the ';' character as delimiter
/// # Errors
/// invalid csv
pub fn process_grades_csv_file(
    path: String,
) -> Result<HashMap<String, Vec<BackboardGrade>>, csv::Error> {
    log::info!("processing grades from {path:?}");
    let mut csv_raw = csv::ReaderBuilder::new().delimiter(b';').from_path(path)?;
    let mut grades: HashMap<String, Vec<BackboardGrade>> = HashMap::new();
    for grade in csv_raw.deserialize() {
        let mut grade: BackboardGrade = grade?;
        grades
            .entry(grade.hashed_om_code())
            .or_default()
            .push(grade);
    }
    log::info!("successfully processed grades");
    log::trace!("hashed-om-id mapped grades: {grades:?}");

    Ok(grades)
}

/// reads, parses and processes a csv student-info export from the `path`\
/// a valid example can be found [here](../test_students.csv)\
/// **NOTE**: the csv shall use the ';' character as delimiter
/// # Errors
/// invalid csv
pub fn process_students_csv_file(
    path: String,
) -> Result<HashMap<String, BackboardStudent>, csv::Error> {
    log::info!("processing students from {path:?}");
    let mut csv_raw = csv::ReaderBuilder::new().delimiter(b';').from_path(path)?;
    let mut students = HashMap::new();
    for student in csv_raw.deserialize() {
        let mut student: BackboardStudent = student?;
        students.insert(student.hashed_om_code(), student);
    }
    log::info!("successfully processed students");
    log::trace!("hashed-om-id mapped students: {students:?}");

    Ok(students)
}

/// data of a user(student) that comes from the server\
/// will be used to add further grades and/or update the information of the account
#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "PascalCase")]
pub struct BackboardUser {
    id: String,
    public_key: String,
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

/// a nice pack of data containing all the relevant information about a student\
/// it will be sent to the server once further encrypted
#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "PascalCase")]
pub struct GradeCollection {
    pub grades: Vec<BackboardGrade>,
    pub school_class: Option<String>,
    pub student_name: String,
    pub user: BackboardUser,
}
impl GradeCollection {
    /// convert the [`GradeCollection`] to json and encrypt it to be safely transferred over the wire to the server
    /// # Errors
    /// coming from [`serde_json::to_string`] and [`crate::cryptography::kyber_encrypt`]
    pub fn to_encrypted_json(&self, pub_key: String) -> Result<String, String> {
        log::info!("encrypting user's grade collection");
        let as_json = serde_json::to_string(&self).map_err(|e| e.to_string())?;
        let ret =
            crate::cryptography::kyber_encrypt(&as_json, pub_key).map_err(|e| e.to_string())?;
        log::info!("successfully encrypted user's grade collection");
        Ok(ret)
    }
}

#[test]
fn parse_grades() {
    let path = String::from("test_grades.csv");
    assert!(std::fs::exists(&path).unwrap());
    let grades = process_grades_csv_file(path).inspect_err(|err| eprintln!("{err}"));
    assert!(grades.is_ok());
    eprintln!("imported {:#?}", grades.unwrap());
}

#[test]
fn parse_students() {
    let path = String::from("test_students.csv");
    assert!(std::fs::exists(&path).unwrap());
    let students = process_students_csv_file(path).inspect_err(|err| eprintln!("{err}"));
    assert!(students.is_ok());
    eprintln!("imported {:#?}", students.unwrap());
}
