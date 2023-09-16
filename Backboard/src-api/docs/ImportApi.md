# \ImportApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_import_grades_user_id_post**](ImportApi.md#api_import_grades_user_id_post) | **POST** /Api/Import/Grades/{userId} | Import grades for a user
[**api_import_reset_key_password_put**](ImportApi.md#api_import_reset_key_password_put) | **PUT** /Api/Import/ResetKeyPassword | Set the reset key password
[**api_import_users_get**](ImportApi.md#api_import_users_get) | **GET** /Api/Import/Users | Get a list of all users for grade importing



## api_import_grades_user_id_post

> api_import_grades_user_id_post(user_id, import_import_grades_request_body)
Import grades for a user

Requires the following features to be enabled: Import

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**user_id** | **uuid::Uuid** |  | [required] |
**import_import_grades_request_body** | Option<[**ImportImportGradesRequestBody**](ImportImportGradesRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[ImportKey](../README.md#ImportKey)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_import_reset_key_password_put

> api_import_reset_key_password_put(import_update_reset_key_password_request_body)
Set the reset key password

Requires the following features to be enabled: Import

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**import_update_reset_key_password_request_body** | Option<[**ImportUpdateResetKeyPasswordRequestBody**](ImportUpdateResetKeyPasswordRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[ImportKey](../README.md#ImportKey)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_import_users_get

> Vec<crate::models::ImportIndexUsersResponse> api_import_users_get(filters, sorts, page, page_size)
Get a list of all users for grade importing

Requires the following features to be enabled: Import

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::ImportIndexUsersResponse>**](ImportIndexUsersResponse.md)

### Authorization

[ImportKey](../README.md#ImportKey)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

