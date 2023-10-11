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
pub struct ImageVotingsViewImageVotingEntryResponseUser {
    #[serde(rename = "name", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub name: Option<Option<String>>,
    #[serde(rename = "realName", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub real_name: Option<Option<String>>,
    #[serde(rename = "class", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub class: Option<Option<String>>,
}

impl ImageVotingsViewImageVotingEntryResponseUser {
    pub fn new() -> ImageVotingsViewImageVotingEntryResponseUser {
        ImageVotingsViewImageVotingEntryResponseUser {
            name: None,
            real_name: None,
            class: None,
        }
    }
}

