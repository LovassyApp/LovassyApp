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
pub struct UsersCreateUserRequestBody {
    #[serde(rename = "email")]
    pub email: String,
    #[serde(rename = "password")]
    pub password: String,
    #[serde(rename = "name")]
    pub name: String,
    #[serde(rename = "omCode")]
    pub om_code: String,
}

impl UsersCreateUserRequestBody {
    pub fn new(email: String, password: String, name: String, om_code: String) -> UsersCreateUserRequestBody {
        UsersCreateUserRequestBody {
            email,
            password,
            name,
            om_code,
        }
    }
}


