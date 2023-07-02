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
pub struct UsersIndexUsersResponse {
    #[serde(rename = "id", skip_serializing_if = "Option::is_none")]
    pub id: Option<uuid::Uuid>,
    #[serde(rename = "name", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub name: Option<Option<String>>,
    #[serde(rename = "email", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub email: Option<Option<String>>,
    #[serde(rename = "emailVerifiedAt", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub email_verified_at: Option<Option<String>>,
    #[serde(rename = "realName", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub real_name: Option<Option<String>>,
    #[serde(rename = "class", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub class: Option<Option<String>>,
    #[serde(rename = "userGroups", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub user_groups: Option<Option<Vec<crate::models::UsersIndexUsersResponseUserGroup>>>,
    #[serde(rename = "createdAt", skip_serializing_if = "Option::is_none")]
    pub created_at: Option<String>,
    #[serde(rename = "updatedAt", skip_serializing_if = "Option::is_none")]
    pub updated_at: Option<String>,
}

impl UsersIndexUsersResponse {
    pub fn new() -> UsersIndexUsersResponse {
        UsersIndexUsersResponse {
            id: None,
            name: None,
            email: None,
            email_verified_at: None,
            real_name: None,
            class: None,
            user_groups: None,
            created_at: None,
            updated_at: None,
        }
    }
}

