/*
 * Blueboard
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 4.1.0
 * 
 * Generated by: https://openapi-generator.tech
 */




#[derive(Clone, Debug, PartialEq, Default, Serialize, Deserialize)]
pub struct ImageVotingsViewImageVotingChoiceResponseImageVoting {
    #[serde(rename = "id", skip_serializing_if = "Option::is_none")]
    pub id: Option<i32>,
    #[serde(rename = "name", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub name: Option<Option<String>>,
    #[serde(rename = "description", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub description: Option<Option<String>>,
    #[serde(rename = "type", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub r#type: Option<Option<String>>,
    #[serde(rename = "aspects", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub aspects: Option<Option<Vec<crate::models::ImageVotingsViewImageVotingChoiceResponseImageVotingAspect>>>,
    #[serde(rename = "active", skip_serializing_if = "Option::is_none")]
    pub active: Option<bool>,
    #[serde(rename = "showUploaderInfo", skip_serializing_if = "Option::is_none")]
    pub show_uploader_info: Option<bool>,
    #[serde(rename = "superIncrementAllowed", skip_serializing_if = "Option::is_none")]
    pub super_increment_allowed: Option<bool>,
    #[serde(rename = "superIncrementValue", skip_serializing_if = "Option::is_none")]
    pub super_increment_value: Option<i32>,
    #[serde(rename = "createdAt", skip_serializing_if = "Option::is_none")]
    pub created_at: Option<String>,
    #[serde(rename = "updatedAt", skip_serializing_if = "Option::is_none")]
    pub updated_at: Option<String>,
}

impl ImageVotingsViewImageVotingChoiceResponseImageVoting {
    pub fn new() -> ImageVotingsViewImageVotingChoiceResponseImageVoting {
        ImageVotingsViewImageVotingChoiceResponseImageVoting {
            id: None,
            name: None,
            description: None,
            r#type: None,
            aspects: None,
            active: None,
            show_uploader_info: None,
            super_increment_allowed: None,
            super_increment_value: None,
            created_at: None,
            updated_at: None,
        }
    }
}


