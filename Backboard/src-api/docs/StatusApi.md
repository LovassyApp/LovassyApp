# \StatusApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_status_notify_on_reset_key_password_set_post**](StatusApi.md#api_status_notify_on_reset_key_password_set_post) | **POST** /Api/Status/NotifyOnResetKeyPasswordSet | 
[**api_status_service_status_get**](StatusApi.md#api_status_service_status_get) | **GET** /Api/Status/ServiceStatus | 
[**api_status_version_get**](StatusApi.md#api_status_version_get) | **GET** /Api/Status/Version | 



## api_status_notify_on_reset_key_password_set_post

> api_status_notify_on_reset_key_password_set_post(status_notify_on_reset_key_password_set_request_body)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**status_notify_on_reset_key_password_set_request_body** | Option<[**StatusNotifyOnResetKeyPasswordSetRequestBody**](StatusNotifyOnResetKeyPasswordSetRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_status_service_status_get

> crate::models::StatusViewServiceStatusResponse api_status_service_status_get()


### Parameters

This endpoint does not need any parameter.

### Return type

[**crate::models::StatusViewServiceStatusResponse**](StatusViewServiceStatusResponse.md)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_status_version_get

> crate::models::StatusViewVersionResponse api_status_version_get(send_ok, send_motd)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**send_ok** | **bool** |  | [required] |
**send_motd** | **bool** |  | [required] |

### Return type

[**crate::models::StatusViewVersionResponse**](StatusViewVersionResponse.md)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

