# \UserGroupsApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_user_groups_get**](UserGroupsApi.md#api_user_groups_get) | **GET** /Api/UserGroups | Get a list of all user groups
[**api_user_groups_id_delete**](UserGroupsApi.md#api_user_groups_id_delete) | **DELETE** /Api/UserGroups/{id} | Delete a user group
[**api_user_groups_id_get**](UserGroupsApi.md#api_user_groups_id_get) | **GET** /Api/UserGroups/{id} | Get information about a user group
[**api_user_groups_id_patch**](UserGroupsApi.md#api_user_groups_id_patch) | **PATCH** /Api/UserGroups/{id} | Update a user group
[**api_user_groups_post**](UserGroupsApi.md#api_user_groups_post) | **POST** /Api/UserGroups | Create a new user group



## api_user_groups_get

> Vec<crate::models::AuthIndexUserGroupsResponse> api_user_groups_get(filters, sorts, page, page_size)
Get a list of all user groups

Requires verified email; Requires one of the following permissions: Auth.IndexUserGroups

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::AuthIndexUserGroupsResponse>**](AuthIndexUserGroupsResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_user_groups_id_delete

> api_user_groups_id_delete(id)
Delete a user group

Requires verified email; Requires one of the following permissions: Auth.DeleteUserGroup

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


## api_user_groups_id_get

> crate::models::AuthViewUserGroupResponse api_user_groups_id_get(id)
Get information about a user group

Requires verified email; Requires one of the following permissions: Auth.ViewUserGroup

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |

### Return type

[**crate::models::AuthViewUserGroupResponse**](AuthViewUserGroupResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_user_groups_id_patch

> api_user_groups_id_patch(id, auth_update_user_group_request_body)
Update a user group

Requires verified email; Requires one of the following permissions: Auth.UpdateUserGroup

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**auth_update_user_group_request_body** | Option<[**AuthUpdateUserGroupRequestBody**](AuthUpdateUserGroupRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_user_groups_post

> crate::models::AuthCreateUserGroupResponse api_user_groups_post(auth_create_user_group_request_body)
Create a new user group

Requires verified email; Requires one of the following permissions: Auth.CreateUserGroup

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**auth_create_user_group_request_body** | Option<[**AuthCreateUserGroupRequestBody**](AuthCreateUserGroupRequestBody.md)> |  |  |

### Return type

[**crate::models::AuthCreateUserGroupResponse**](AuthCreateUserGroupResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

