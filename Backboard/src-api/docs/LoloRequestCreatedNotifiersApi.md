# \LoloRequestCreatedNotifiersApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_lolo_request_created_notifiers_get**](LoloRequestCreatedNotifiersApi.md#api_lolo_request_created_notifiers_get) | **GET** /Api/LoloRequestCreatedNotifiers | Get a list of all emails to notify when a lolo request is created
[**api_lolo_request_created_notifiers_put**](LoloRequestCreatedNotifiersApi.md#api_lolo_request_created_notifiers_put) | **PUT** /Api/LoloRequestCreatedNotifiers | Update the list of emails to notify when a lolo request is created



## api_lolo_request_created_notifiers_get

> Vec<crate::models::ShopIndexLoloRequestCreatedNotifiersResponse> api_lolo_request_created_notifiers_get(filters, sorts, page, page_size)
Get a list of all emails to notify when a lolo request is created

Requires verified email; Requires one of the following permissions: Shop.IndexLoloRequestCreatedNotifiers; Requires the following features to be enabled: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::ShopIndexLoloRequestCreatedNotifiersResponse>**](ShopIndexLoloRequestCreatedNotifiersResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_lolo_request_created_notifiers_put

> api_lolo_request_created_notifiers_put(shop_update_lolo_request_created_notifiers_request_body)
Update the list of emails to notify when a lolo request is created

Requires verified email; Requires one of the following permissions: Shop.UpdateLoloRequestCreatedNotifiers; Requires the following features to be enabled: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**shop_update_lolo_request_created_notifiers_request_body** | Option<[**ShopUpdateLoloRequestCreatedNotifiersRequestBody**](ShopUpdateLoloRequestCreatedNotifiersRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

