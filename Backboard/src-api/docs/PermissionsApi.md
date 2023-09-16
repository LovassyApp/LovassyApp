# \PermissionsApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_permissions_get**](PermissionsApi.md#api_permissions_get) | **GET** /Api/Permissions | Get a list of all permissions



## api_permissions_get

> Vec<crate::models::AuthIndexPermissionsResponse> api_permissions_get(filters, sorts, page, page_size)
Get a list of all permissions

Requires verified email; Requires one of the following permissions: Auth.IndexPermissions

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::AuthIndexPermissionsResponse>**](AuthIndexPermissionsResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

