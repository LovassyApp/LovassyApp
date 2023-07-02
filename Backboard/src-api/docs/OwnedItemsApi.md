# \OwnedItemsApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_owned_items_get**](OwnedItemsApi.md#api_owned_items_get) | **GET** /Api/OwnedItems | 
[**api_owned_items_id_delete**](OwnedItemsApi.md#api_owned_items_id_delete) | **DELETE** /Api/OwnedItems/{id} | 
[**api_owned_items_id_get**](OwnedItemsApi.md#api_owned_items_id_get) | **GET** /Api/OwnedItems/{id} | 
[**api_owned_items_id_patch**](OwnedItemsApi.md#api_owned_items_id_patch) | **PATCH** /Api/OwnedItems/{id} | 
[**api_owned_items_id_use_post**](OwnedItemsApi.md#api_owned_items_id_use_post) | **POST** /Api/OwnedItems/{id}/Use | 
[**api_owned_items_own_get**](OwnedItemsApi.md#api_owned_items_own_get) | **GET** /Api/OwnedItems/Own | 
[**api_owned_items_post**](OwnedItemsApi.md#api_owned_items_post) | **POST** /Api/OwnedItems | 



## api_owned_items_get

> Vec<crate::models::ShopIndexOwnedItemsResponse> api_owned_items_get(filters, sorts, page, page_size, search)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.IndexOwnedItems<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |
**search** | Option<**String**> |  |  |

### Return type

[**Vec<crate::models::ShopIndexOwnedItemsResponse>**](ShopIndexOwnedItemsResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_owned_items_id_delete

> api_owned_items_id_delete(id)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.DeleteOwnedItem, Shop.DeleteOwnOwnedItem<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_owned_items_id_get

> crate::models::ShopViewOwnedItemResponse api_owned_items_id_get(id)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.ViewOwnedItem, Shop.ViewOwnOwnedItem<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |

### Return type

[**crate::models::ShopViewOwnedItemResponse**](ShopViewOwnedItemResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_owned_items_id_patch

> api_owned_items_id_patch(id, shop_update_owned_item_request_body)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.UpdateOwnedItem<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**shop_update_owned_item_request_body** | Option<[**ShopUpdateOwnedItemRequestBody**](ShopUpdateOwnedItemRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_owned_items_id_use_post

> api_owned_items_id_use_post(id, shop_use_owned_item_request_body)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.UseOwnOwnedItem<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**shop_use_owned_item_request_body** | Option<[**ShopUseOwnedItemRequestBody**](ShopUseOwnedItemRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_owned_items_own_get

> Vec<crate::models::ShopIndexOwnOwnedItemsResponse> api_owned_items_own_get(filters, sorts, page, page_size, search)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.IndexOwnOwnedItems<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |
**search** | Option<**String**> |  |  |

### Return type

[**Vec<crate::models::ShopIndexOwnOwnedItemsResponse>**](ShopIndexOwnOwnedItemsResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_owned_items_post

> crate::models::ShopCreateOwnedItemResponse api_owned_items_post(shop_create_owned_item_request_body)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.CreateOwnedItem<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**shop_create_owned_item_request_body** | Option<[**ShopCreateOwnedItemRequestBody**](ShopCreateOwnedItemRequestBody.md)> |  |  |

### Return type

[**crate::models::ShopCreateOwnedItemResponse**](ShopCreateOwnedItemResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

