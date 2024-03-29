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
pub struct AuthViewControlResponse {
    #[serde(rename = "user", skip_serializing_if = "Option::is_none")]
    pub user: Option<Box<crate::models::AuthViewControlResponseUser>>,
    #[serde(rename = "session", skip_serializing_if = "Option::is_none")]
    pub session: Option<Box<crate::models::AuthViewControlResponseSession>>,
    #[serde(rename = "isSuperUser", skip_serializing_if = "Option::is_none")]
    pub is_super_user: Option<bool>,
    #[serde(rename = "userGroups", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub user_groups: Option<Option<Vec<String>>>,
    #[serde(rename = "permissions", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub permissions: Option<Option<Vec<String>>>,
    #[serde(rename = "features", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub features: Option<Option<Vec<String>>>,
}

impl AuthViewControlResponse {
    pub fn new() -> AuthViewControlResponse {
        AuthViewControlResponse {
            user: None,
            session: None,
            is_super_user: None,
            user_groups: None,
            permissions: None,
            features: None,
        }
    }
}


