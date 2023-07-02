# \UsersApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_users_get**](UsersApi.md#api_users_get) | **GET** /Api/Users | 
[**api_users_id_delete**](UsersApi.md#api_users_id_delete) | **DELETE** /Api/Users/{id} | 
[**api_users_id_get**](UsersApi.md#api_users_id_get) | **GET** /Api/Users/{id} | 
[**api_users_id_patch**](UsersApi.md#api_users_id_patch) | **PATCH** /Api/Users/{id} | 
[**api_users_kick_all_post**](UsersApi.md#api_users_kick_all_post) | **POST** /Api/Users/Kick/All | 
[**api_users_kick_id_post**](UsersApi.md#api_users_kick_id_post) | **POST** /Api/Users/Kick/{id} | 
[**api_users_post**](UsersApi.md#api_users_post) | **POST** /Api/Users | 



## api_users_get

> Vec<crate::models::UsersIndexUsersResponse> api_users_get(filters, sorts, page, page_size)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Users.IndexUsers<br><b>Requires the following features to be enabled</b>: Users

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::UsersIndexUsersResponse>**](UsersIndexUsersResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_users_id_delete

> api_users_id_delete(id)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Users.DeleteUser<br><b>Requires the following features to be enabled</b>: Users

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **uuid::Uuid** |  | [required] |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_users_id_get

> crate::models::UsersViewUserResponse api_users_id_get(id)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Users.ViewUser<br><b>Requires the following features to be enabled</b>: Users

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **uuid::Uuid** |  | [required] |

### Return type

[**crate::models::UsersViewUserResponse**](UsersViewUserResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_users_id_patch

> api_users_id_patch(id, users_update_user_request_body)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Users.UpdateUser<br><b>Requires the following features to be enabled</b>: Users

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **uuid::Uuid** |  | [required] |
**users_update_user_request_body** | Option<[**UsersUpdateUserRequestBody**](UsersUpdateUserRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_users_kick_all_post

> api_users_kick_all_post()


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Users.KickAllUsers<br><b>Requires the following features to be enabled</b>: Users

### Parameters

This endpoint does not need any parameter.

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_users_kick_id_post

> api_users_kick_id_post(id)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Users.KickUser<br><b>Requires the following features to be enabled</b>: Users

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **uuid::Uuid** |  | [required] |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_users_post

> api_users_post(verify_url, verify_token_query_key, users_create_user_request_body)


<b>Requires the following features to be enabled</b>: Users

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**verify_url** | Option<**String**> |  |  |
**verify_token_query_key** | Option<**String**> |  |  |
**users_create_user_request_body** | Option<[**UsersCreateUserRequestBody**](UsersCreateUserRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

