# \LoloRequestsApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_lolo_requests_get**](LoloRequestsApi.md#api_lolo_requests_get) | **GET** /Api/LoloRequests | Get a list of all lolo requests
[**api_lolo_requests_id_delete**](LoloRequestsApi.md#api_lolo_requests_id_delete) | **DELETE** /Api/LoloRequests/{id} | Delete a lolo request
[**api_lolo_requests_id_get**](LoloRequestsApi.md#api_lolo_requests_id_get) | **GET** /Api/LoloRequests/{id} | Get information about a lolo request
[**api_lolo_requests_id_patch**](LoloRequestsApi.md#api_lolo_requests_id_patch) | **PATCH** /Api/LoloRequests/{id} | Update a lolo request
[**api_lolo_requests_overrule_id_post**](LoloRequestsApi.md#api_lolo_requests_overrule_id_post) | **POST** /Api/LoloRequests/Overrule/{id} | Overrule a lolo request
[**api_lolo_requests_own_get**](LoloRequestsApi.md#api_lolo_requests_own_get) | **GET** /Api/LoloRequests/Own | Get a list of the current user's lolo requests
[**api_lolo_requests_post**](LoloRequestsApi.md#api_lolo_requests_post) | **POST** /Api/LoloRequests | Create a new lolo request



## api_lolo_requests_get

> Vec<crate::models::ShopIndexLoloRequestsResponse> api_lolo_requests_get(filters, sorts, page, page_size)
Get a list of all lolo requests

Requires verified email; Requires one of the following permissions: Shop.IndexLoloRequests; Requires the following features to be enabled: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::ShopIndexLoloRequestsResponse>**](ShopIndexLoloRequestsResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_lolo_requests_id_delete

> api_lolo_requests_id_delete(id)
Delete a lolo request

Requires verified email; Requires one of the following permissions: Shop.DeleteOwnLoloRequest, Shop.DeleteLoloRequest; Requires the following features to be enabled: Shop

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


## api_lolo_requests_id_get

> crate::models::ShopViewLoloRequestResponse api_lolo_requests_id_get(id)
Get information about a lolo request

Requires verified email; Requires one of the following permissions: Shop.ViewLoloRequest; Requires the following features to be enabled: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |

### Return type

[**crate::models::ShopViewLoloRequestResponse**](ShopViewLoloRequestResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_lolo_requests_id_patch

> api_lolo_requests_id_patch(id, shop_update_lolo_request_request_body)
Update a lolo request

Requires verified email; Requires one of the following permissions: Shop.UpdateOwnLoloRequest, Shop.UpdateLoloRequest; Requires the following features to be enabled: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**shop_update_lolo_request_request_body** | Option<[**ShopUpdateLoloRequestRequestBody**](ShopUpdateLoloRequestRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_lolo_requests_overrule_id_post

> api_lolo_requests_overrule_id_post(id, shop_overrule_lolo_request_request_body)
Overrule a lolo request

Requires verified email; Requires one of the following permissions: Shop.OverruleLoloRequest; Requires the following features to be enabled: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**shop_overrule_lolo_request_request_body** | Option<[**ShopOverruleLoloRequestRequestBody**](ShopOverruleLoloRequestRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_lolo_requests_own_get

> Vec<crate::models::ShopIndexOwnLoloRequestsResponse> api_lolo_requests_own_get(filters, sorts, page, page_size)
Get a list of the current user's lolo requests

Requires verified email; Requires one of the following permissions: Shop.IndexOwnLoloRequests; Requires the following features to be enabled: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::ShopIndexOwnLoloRequestsResponse>**](ShopIndexOwnLoloRequestsResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_lolo_requests_post

> crate::models::ShopCreateLoloRequestResponse api_lolo_requests_post(lolo_requests_url, shop_create_lolo_request_request_body)
Create a new lolo request

Requires verified email; Requires one of the following permissions: Shop.CreateLoloRequest; Requires the following features to be enabled: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**lolo_requests_url** | Option<**String**> |  |  |
**shop_create_lolo_request_request_body** | Option<[**ShopCreateLoloRequestRequestBody**](ShopCreateLoloRequestRequestBody.md)> |  |  |

### Return type

[**crate::models::ShopCreateLoloRequestResponse**](ShopCreateLoloRequestResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

