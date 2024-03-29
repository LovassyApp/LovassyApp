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
pub struct StatusViewServiceStatusResponse {
    #[serde(rename = "ready", skip_serializing_if = "Option::is_none")]
    pub ready: Option<bool>,
    #[serde(rename = "serviceStatus", skip_serializing_if = "Option::is_none")]
    pub service_status: Option<Box<crate::models::StatusViewServiceStatusResponseServiceStatus>>,
}

impl StatusViewServiceStatusResponse {
    pub fn new() -> StatusViewServiceStatusResponse {
        StatusViewServiceStatusResponse {
            ready: None,
            service_status: None,
        }
    }
}


