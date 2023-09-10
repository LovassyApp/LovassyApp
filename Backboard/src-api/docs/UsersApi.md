# \UsersApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_users_get**](UsersApi.md#api_users_get) | **GET** /Api/Users | Get a list of all users
[**api_users_id_delete**](UsersApi.md#api_users_id_delete) | **DELETE** /Api/Users/{id} | Delete a user
[**api_users_id_get**](UsersApi.md#api_users_id_get) | **GET** /Api/Users/{id} | Get information about a user
[**api_users_id_patch**](UsersApi.md#api_users_id_patch) | **PATCH** /Api/Users/{id} | Update a user
[**api_users_kick_all_post**](UsersApi.md#api_users_kick_all_post) | **POST** /Api/Users/Kick/All | Delete all active sessions of all users
[**api_users_kick_id_post**](UsersApi.md#api_users_kick_id_post) | **POST** /Api/Users/Kick/{id} | Delete all active sessions of a user
[**api_users_post**](UsersApi.md#api_users_post) | **POST** /Api/Users | Create a new user



## api_users_get

> Vec<crate::models::UsersIndexUsersResponse> api_users_get(filters, sorts, page, page_size)
Get a list of all users

Requires verified email; Requires one of the following permissions: Users.IndexUsers; Requires the following features to be enabled: Users

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
Delete a user

Requires verified email; Requires one of the following permissions: Users.DeleteUser; Requires the following features to be enabled: Users

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

> api_users_id_get(id)
Get information about a user

Requires verified email; Requires one of the following permissions: Users.ViewUser; Requires the following features to be enabled: Users

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


## api_users_id_patch

> api_users_id_patch(id, users_update_user_request_body)
Update a user

Requires verified email; Requires one of the following permissions: Users.UpdateUser; Requires the following features to be enabled: Users

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
Delete all active sessions of all users

Requires verified email; Requires one of the following permissions: Users.KickAllUsers; Requires the following features to be enabled: Users

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
Delete all active sessions of a user

Requires verified email; Requires one of the following permissions: Users.KickUser; Requires the following features to be enabled: Users

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
Create a new user

Requires the following features to be enabled: Users

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

