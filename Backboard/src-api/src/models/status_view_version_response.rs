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
pub struct StatusViewVersionResponse {
    #[serde(rename = "whoAmI", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub who_am_i: Option<Option<String>>,
    #[serde(rename = "version", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub version: Option<Option<String>>,
    #[serde(rename = "dotNetVersion", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub dot_net_version: Option<Option<String>>,
    #[serde(rename = "contributors", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub contributors: Option<Option<Vec<String>>>,
    #[serde(rename = "repository", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub repository: Option<Option<String>>,
    #[serde(rename = "motd", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub motd: Option<Option<String>>,
}

impl StatusViewVersionResponse {
    pub fn new() -> StatusViewVersionResponse {
        StatusViewVersionResponse {
            who_am_i: None,
            version: None,
            dot_net_version: None,
            contributors: None,
            repository: None,
            motd: None,
        }
    }
}


