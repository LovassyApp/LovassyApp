# \OwnedItemsApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_owned_items_get**](OwnedItemsApi.md#api_owned_items_get) | **GET** /Api/OwnedItems | Get a list of all owned items
[**api_owned_items_id_delete**](OwnedItemsApi.md#api_owned_items_id_delete) | **DELETE** /Api/OwnedItems/{id} | Delete an owned item
[**api_owned_items_id_get**](OwnedItemsApi.md#api_owned_items_id_get) | **GET** /Api/OwnedItems/{id} | Get information about an owned item
[**api_owned_items_id_patch**](OwnedItemsApi.md#api_owned_items_id_patch) | **PATCH** /Api/OwnedItems/{id} | Update an owned item
[**api_owned_items_id_use_post**](OwnedItemsApi.md#api_owned_items_id_use_post) | **POST** /Api/OwnedItems/{id}/Use | Use an owned item
[**api_owned_items_own_get**](OwnedItemsApi.md#api_owned_items_own_get) | **GET** /Api/OwnedItems/Own | Get a list of the current user's owned items
[**api_owned_items_post**](OwnedItemsApi.md#api_owned_items_post) | **POST** /Api/OwnedItems | Create a new owned item



## api_owned_items_get

> Vec<crate::models::ShopIndexOwnedItemsResponse> api_owned_items_get(filters, sorts, page, page_size, search)
Get a list of all owned items

Requires verified email; Requires one of the following permissions: Shop.IndexOwnedItems; Requires the following features to be enabled: Shop

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
Delete an owned item

Requires verified email; Requires one of the following permissions: Shop.DeleteOwnedItem, Shop.DeleteOwnOwnedItem; Requires the following features to be enabled: Shop

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
Get information about an owned item

Requires verified email; Requires one of the following permissions: Shop.ViewOwnedItem, Shop.ViewOwnOwnedItem; Requires the following features to be enabled: Shop

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
Update an owned item

Requires verified email; Requires one of the following permissions: Shop.UpdateOwnedItem; Requires the following features to be enabled: Shop

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
Use an owned item

Requires verified email; Requires one of the following permissions: Shop.UseOwnOwnedItem; Requires the following features to be enabled: Shop

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
Get a list of the current user's owned items

Requires verified email; Requires one of the following permissions: Shop.IndexOwnOwnedItems; Requires the following features to be enabled: Shop

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
Create a new owned item

Requires verified email; Requires one of the following permissions: Shop.CreateOwnedItem; Requires the following features to be enabled: Shop

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

