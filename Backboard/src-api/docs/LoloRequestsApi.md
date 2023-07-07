# \LoloRequestsApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_lolo_requests_get**](LoloRequestsApi.md#api_lolo_requests_get) | **GET** /Api/LoloRequests | 
[**api_lolo_requests_id_delete**](LoloRequestsApi.md#api_lolo_requests_id_delete) | **DELETE** /Api/LoloRequests/{id} | 
[**api_lolo_requests_id_get**](LoloRequestsApi.md#api_lolo_requests_id_get) | **GET** /Api/LoloRequests/{id} | 
[**api_lolo_requests_id_patch**](LoloRequestsApi.md#api_lolo_requests_id_patch) | **PATCH** /Api/LoloRequests/{id} | 
[**api_lolo_requests_overrule_id_post**](LoloRequestsApi.md#api_lolo_requests_overrule_id_post) | **POST** /Api/LoloRequests/Overrule/{id} | 
[**api_lolo_requests_own_get**](LoloRequestsApi.md#api_lolo_requests_own_get) | **GET** /Api/LoloRequests/Own | 
[**api_lolo_requests_post**](LoloRequestsApi.md#api_lolo_requests_post) | **POST** /Api/LoloRequests | 



## api_lolo_requests_get

> Vec<crate::models::ShopIndexLoloRequestsResponse> api_lolo_requests_get(filters, sorts, page, page_size)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.IndexLoloRequests<br><b>Requires the following features to be enabled</b>: Shop

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


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.DeleteOwnLoloRequest, Shop.DeleteLoloRequest<br><b>Requires the following features to be enabled</b>: Shop

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


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.ViewLoloRequest<br><b>Requires the following features to be enabled</b>: Shop

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


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.UpdateOwnLoloRequest, Shop.UpdateLoloRequest<br><b>Requires the following features to be enabled</b>: Shop

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


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.OverruleLoloRequest<br><b>Requires the following features to be enabled</b>: Shop

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


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.IndexOwnLoloRequests<br><b>Requires the following features to be enabled</b>: Shop

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

> crate::models::ShopCreateLoloRequestResponse api_lolo_requests_post(shop_create_lolo_request_request_body)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.CreateLoloRequest<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**shop_create_lolo_request_request_body** | Option<[**ShopCreateLoloRequestRequestBody**](ShopCreateLoloRequestRequestBody.md)> |  |  |

### Return type

[**crate::models::ShopCreateLoloRequestResponse**](ShopCreateLoloRequestResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

