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
pub struct ImportUpdateResetKeyPasswordRequestBody {
    #[serde(rename = "resetKeyPassword")]
    pub reset_key_password: String,
}

impl ImportUpdateResetKeyPasswordRequestBody {
    pub fn new(reset_key_password: String) -> ImportUpdateResetKeyPasswordRequestBody {
        ImportUpdateResetKeyPasswordRequestBody {
            reset_key_password,
        }
    }
}


