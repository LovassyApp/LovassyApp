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
pub struct ShopUpdateOwnedItemRequestBody {
    #[serde(rename = "usedAt", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub used_at: Option<Option<String>>,
}

impl ShopUpdateOwnedItemRequestBody {
    pub fn new() -> ShopUpdateOwnedItemRequestBody {
        ShopUpdateOwnedItemRequestBody {
            used_at: None,
        }
    }
}


