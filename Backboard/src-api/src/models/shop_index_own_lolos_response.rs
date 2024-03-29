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
pub struct ShopIndexOwnLolosResponse {
    #[serde(rename = "balance", skip_serializing_if = "Option::is_none")]
    pub balance: Option<i32>,
    #[serde(rename = "coins", default, with = "::serde_with::rust::double_option", skip_serializing_if = "Option::is_none")]
    pub coins: Option<Option<Vec<crate::models::ShopIndexOwnLolosResponseCoin>>>,
}

impl ShopIndexOwnLolosResponse {
    pub fn new() -> ShopIndexOwnLolosResponse {
        ShopIndexOwnLolosResponse {
            balance: None,
            coins: None,
        }
    }
}


