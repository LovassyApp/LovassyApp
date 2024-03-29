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
pub struct ImageVotingsUnchooseImageVotingEntryRequestBody {
    #[serde(rename = "aspectKey", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub aspect_key: Option<Option<String>>,
}

impl ImageVotingsUnchooseImageVotingEntryRequestBody {
    pub fn new() -> ImageVotingsUnchooseImageVotingEntryRequestBody {
        ImageVotingsUnchooseImageVotingEntryRequestBody {
            aspect_key: None,
        }
    }
}


