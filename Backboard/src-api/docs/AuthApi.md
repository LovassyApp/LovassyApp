# \AuthApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_auth_control_get**](AuthApi.md#api_auth_control_get) | **GET** /Api/Auth/Control | 
[**api_auth_login_post**](AuthApi.md#api_auth_login_post) | **POST** /Api/Auth/Login | 
[**api_auth_logout_delete**](AuthApi.md#api_auth_logout_delete) | **DELETE** /Api/Auth/Logout | 
[**api_auth_refresh_post**](AuthApi.md#api_auth_refresh_post) | **POST** /Api/Auth/Refresh | 
[**api_auth_resend_verify_email_post**](AuthApi.md#api_auth_resend_verify_email_post) | **POST** /Api/Auth/ResendVerifyEmail | 
[**api_auth_reset_password_post**](AuthApi.md#api_auth_reset_password_post) | **POST** /Api/Auth/ResetPassword | 
[**api_auth_send_password_reset_post**](AuthApi.md#api_auth_send_password_reset_post) | **POST** /Api/Auth/SendPasswordReset | 
[**api_auth_verify_email_post**](AuthApi.md#api_auth_verify_email_post) | **POST** /Api/Auth/VerifyEmail | 



## api_auth_control_get

> crate::models::AuthViewControlResponse api_auth_control_get()


<b>Requires one of the following permissions</b>: Auth.Control

### Parameters

This endpoint does not need any parameter.

### Return type

[**crate::models::AuthViewControlResponse**](AuthViewControlResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_auth_login_post

> crate::models::AuthLoginResponse api_auth_login_post(auth_login_request_body)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**auth_login_request_body** | Option<[**AuthLoginRequestBody**](AuthLoginRequestBody.md)> |  |  |

### Return type

[**crate::models::AuthLoginResponse**](AuthLoginResponse.md)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_auth_logout_delete

> api_auth_logout_delete()


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


## api_auth_refresh_post

> crate::models::AuthRefreshResponse api_auth_refresh_post(token)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**token** | Option<**String**> |  |  |

### Return type

[**crate::models::AuthRefreshResponse**](AuthRefreshResponse.md)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_auth_resend_verify_email_post

> api_auth_resend_verify_email_post(verify_url, verify_token_query_key)


<b>Requires unverified email</b>

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**verify_url** | Option<**String**> |  |  |
**verify_token_query_key** | Option<**String**> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_auth_reset_password_post

> api_auth_reset_password_post(password_reset_token, auth_reset_password_request_body)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**password_reset_token** | Option<**String**> |  |  |
**auth_reset_password_request_body** | Option<[**AuthResetPasswordRequestBody**](AuthResetPasswordRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_auth_send_password_reset_post

> api_auth_send_password_reset_post(password_reset_url, password_reset_token_query_key, auth_send_password_reset_request_body)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**password_reset_url** | Option<**String**> |  |  |
**password_reset_token_query_key** | Option<**String**> |  |  |
**auth_send_password_reset_request_body** | Option<[**AuthSendPasswordResetRequestBody**](AuthSendPasswordResetRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_auth_verify_email_post

> api_auth_verify_email_post(verify_token)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**verify_token** | Option<**String**> |  |  |

### Return type

 (empty response body)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

