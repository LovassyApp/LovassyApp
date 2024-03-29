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
pub struct StatusViewServiceStatusResponseServiceStatus {
    #[serde(rename = "database", skip_serializing_if = "Option::is_none")]
    pub database: Option<bool>,
    #[serde(rename = "realtime", skip_serializing_if = "Option::is_none")]
    pub realtime: Option<bool>,
    #[serde(rename = "resetKeyPassword", skip_serializing_if = "Option::is_none")]
    pub reset_key_password: Option<bool>,
}

impl StatusViewServiceStatusResponseServiceStatus {
    pub fn new() -> StatusViewServiceStatusResponseServiceStatus {
        StatusViewServiceStatusResponseServiceStatus {
            database: None,
            realtime: None,
            reset_key_password: None,
        }
    }
}


