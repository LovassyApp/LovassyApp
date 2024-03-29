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
pub struct AuthLoginRequestBody {
    #[serde(rename = "email")]
    pub email: String,
    #[serde(rename = "password")]
    pub password: String,
    #[serde(rename = "remember")]
    pub remember: bool,
}

impl AuthLoginRequestBody {
    pub fn new(email: String, password: String, remember: bool) -> AuthLoginRequestBody {
        AuthLoginRequestBody {
            email,
            password,
            remember,
        }
    }
}


