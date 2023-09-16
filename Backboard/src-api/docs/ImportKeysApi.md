# \ImportKeysApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_import_keys_get**](ImportKeysApi.md#api_import_keys_get) | **GET** /Api/ImportKeys | Get a list of all import keys
[**api_import_keys_id_delete**](ImportKeysApi.md#api_import_keys_id_delete) | **DELETE** /Api/ImportKeys/{id} | Delete an import key
[**api_import_keys_id_get**](ImportKeysApi.md#api_import_keys_id_get) | **GET** /Api/ImportKeys/{id} | Get information about an import key
[**api_import_keys_id_patch**](ImportKeysApi.md#api_import_keys_id_patch) | **PATCH** /Api/ImportKeys/{id} | Update an import key
[**api_import_keys_post**](ImportKeysApi.md#api_import_keys_post) | **POST** /Api/ImportKeys | Create a new import key



## api_import_keys_get

> Vec<crate::models::ImportIndexImportKeysResponse> api_import_keys_get(filters, sorts, page, page_size)
Get a list of all import keys

Requires verified email; Requires one of the following permissions: Import.IndexImportKeys; Requires the following features to be enabled: Import

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::ImportIndexImportKeysResponse>**](ImportIndexImportKeysResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_import_keys_id_delete

> api_import_keys_id_delete(id)
Delete an import key

Requires verified email; Requires one of the following permissions: Import.DeleteImportKey; Requires the following features to be enabled: Import

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


## api_import_keys_id_get

> crate::models::ImportViewImportKeyResponse api_import_keys_id_get(id)
Get information about an import key

Requires verified email; Requires one of the following permissions: Import.ViewImportKey; Requires the following features to be enabled: Import

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |

### Return type

[**crate::models::ImportViewImportKeyResponse**](ImportViewImportKeyResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_import_keys_id_patch

> api_import_keys_id_patch(id, import_update_import_key_request_body)
Update an import key

Requires verified email; Requires one of the following permissions: Import.UpdateImportKey; Requires the following features to be enabled: Import

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**import_update_import_key_request_body** | Option<[**ImportUpdateImportKeyRequestBody**](ImportUpdateImportKeyRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_import_keys_post

> crate::models::ImportCreateImportKeyResponse api_import_keys_post(import_create_import_key_request_body)
Create a new import key

Requires verified email; Requires one of the following permissions: Import.CreateImportKey; Requires the following features to be enabled: Import

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**import_create_import_key_request_body** | Option<[**ImportCreateImportKeyRequestBody**](ImportCreateImportKeyRequestBody.md)> |  |  |

### Return type

[**crate::models::ImportCreateImportKeyResponse**](ImportCreateImportKeyResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

