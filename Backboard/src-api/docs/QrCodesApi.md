# \QrCodesApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_qr_codes_get**](QrCodesApi.md#api_qr_codes_get) | **GET** /Api/QRCodes | 
[**api_qr_codes_id_delete**](QrCodesApi.md#api_qr_codes_id_delete) | **DELETE** /Api/QRCodes/{id} | 
[**api_qr_codes_id_get**](QrCodesApi.md#api_qr_codes_id_get) | **GET** /Api/QRCodes/{id} | 
[**api_qr_codes_id_patch**](QrCodesApi.md#api_qr_codes_id_patch) | **PATCH** /Api/QRCodes/{id} | 
[**api_qr_codes_post**](QrCodesApi.md#api_qr_codes_post) | **POST** /Api/QRCodes | 



## api_qr_codes_get

> Vec<crate::models::ShopIndexQrCodesResponse> api_qr_codes_get(filters, sorts, page, page_size)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.IndexQRCodes<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::ShopIndexQrCodesResponse>**](ShopIndexQRCodesResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_qr_codes_id_delete

> api_qr_codes_id_delete(id)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.DeleteQRCode<br><b>Requires the following features to be enabled</b>: Shop

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


## api_qr_codes_id_get

> crate::models::ShopViewQrCodeResponse api_qr_codes_id_get(id)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.ViewQRCode<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |

### Return type

[**crate::models::ShopViewQrCodeResponse**](ShopViewQRCodeResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_qr_codes_id_patch

> api_qr_codes_id_patch(id, shop_update_qr_code_request_body)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.UpdateQRCode<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**shop_update_qr_code_request_body** | Option<[**ShopUpdateQrCodeRequestBody**](ShopUpdateQrCodeRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_qr_codes_post

> crate::models::ShopCreateQrCodeResponse api_qr_codes_post(shop_create_qr_code_request_body)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: Shop.CreateQRCode<br><b>Requires the following features to be enabled</b>: Shop

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**shop_create_qr_code_request_body** | Option<[**ShopCreateQrCodeRequestBody**](ShopCreateQrCodeRequestBody.md)> |  |  |

### Return type

[**crate::models::ShopCreateQrCodeResponse**](ShopCreateQRCodeResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

