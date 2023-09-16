# \StatusApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_status_notify_on_reset_key_password_set_post**](StatusApi.md#api_status_notify_on_reset_key_password_set_post) | **POST** /Api/Status/NotifyOnResetKeyPasswordSet | Subscribe an email to when a password reset key has been set
[**api_status_service_status_get**](StatusApi.md#api_status_service_status_get) | **GET** /Api/Status/ServiceStatus | Get information about the status of the application
[**api_status_version_get**](StatusApi.md#api_status_version_get) | **GET** /Api/Status/Version | Get information about the application version



## api_status_notify_on_reset_key_password_set_post

> api_status_notify_on_reset_key_password_set_post(status_notify_on_reset_key_password_set_request_body)
Subscribe an email to when a password reset key has been set

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
Get information about the status of the application

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
Get information about the application version

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

