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
pub struct ImportImportGradesRequestBody {
    #[serde(rename = "jsonEncrypted")]
    pub json_encrypted: String,
}

impl ImportImportGradesRequestBody {
    pub fn new(json_encrypted: String) -> ImportImportGradesRequestBody {
        ImportImportGradesRequestBody {
            json_encrypted,
        }
    }
}

