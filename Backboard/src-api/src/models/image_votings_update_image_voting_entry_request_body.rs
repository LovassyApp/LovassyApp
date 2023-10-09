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
pub struct ImageVotingsUpdateImageVotingEntryRequestBody {
    #[serde(rename = "title")]
    pub title: String,
    #[serde(rename = "imageUrl")]
    pub image_url: String,
}

impl ImageVotingsUpdateImageVotingEntryRequestBody {
    pub fn new(title: String, image_url: String) -> ImageVotingsUpdateImageVotingEntryRequestBody {
        ImageVotingsUpdateImageVotingEntryRequestBody {
            title,
            image_url,
        }
    }
}


