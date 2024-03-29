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
pub struct ImportUpdateImportKeyRequestBody {
    #[serde(rename = "name")]
    pub name: String,
    #[serde(rename = "enabled")]
    pub enabled: bool,
}

impl ImportUpdateImportKeyRequestBody {
    pub fn new(name: String, enabled: bool) -> ImportUpdateImportKeyRequestBody {
        ImportUpdateImportKeyRequestBody {
            name,
            enabled,
        }
    }
}


