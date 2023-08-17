/*
 * Blueboard
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v4.0.0
 * 
 * Generated by: https://openapi-generator.tech
 */




#[derive(Clone, Debug, PartialEq, Default, Serialize, Deserialize)]
pub struct SchoolIndexGradesResponseGrade {
    #[serde(rename = "id", skip_serializing_if = "Option::is_none")]
    pub id: Option<i32>,
    #[serde(rename = "uid", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub uid: Option<Option<String>>,
    #[serde(rename = "subject", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub subject: Option<Option<String>>,
    #[serde(rename = "subjectCategory", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub subject_category: Option<Option<String>>,
    #[serde(rename = "theme", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub theme: Option<Option<String>>,
    #[serde(rename = "teacher", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub teacher: Option<Option<String>>,
    #[serde(rename = "group", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub group: Option<Option<String>>,
    #[serde(rename = "gradeValue", skip_serializing_if = "Option::is_none")]
    pub grade_value: Option<i32>,
    #[serde(rename = "textGrade", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub text_grade: Option<Option<String>>,
    #[serde(rename = "shortTextGrade", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub short_text_grade: Option<Option<String>>,
    #[serde(rename = "weight", skip_serializing_if = "Option::is_none")]
    pub weight: Option<i32>,
    #[serde(rename = "evaluationDate", skip_serializing_if = "Option::is_none")]
    pub evaluation_date: Option<String>,
    #[serde(rename = "createDate", skip_serializing_if = "Option::is_none")]
    pub create_date: Option<String>,
    #[serde(rename = "type", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub r#type: Option<Option<String>>,
    #[serde(rename = "gradeType", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub grade_type: Option<Option<String>>,
    #[serde(rename = "createdAt", skip_serializing_if = "Option::is_none")]
    pub created_at: Option<String>,
    #[serde(rename = "updatedAt", skip_serializing_if = "Option::is_none")]
    pub updated_at: Option<String>,
}

impl SchoolIndexGradesResponseGrade {
    pub fn new() -> SchoolIndexGradesResponseGrade {
        SchoolIndexGradesResponseGrade {
            id: None,
            uid: None,
            subject: None,
            subject_category: None,
            theme: None,
            teacher: None,
            group: None,
            grade_value: None,
            text_grade: None,
            short_text_grade: None,
            weight: None,
            evaluation_date: None,
            create_date: None,
            r#type: None,
            grade_type: None,
            created_at: None,
            updated_at: None,
        }
    }
}


