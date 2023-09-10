# \ProductsApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_products_buy_id_post**](ProductsApi.md#api_products_buy_id_post) | **POST** /Api/Products/Buy/{id} | Buy a product
[**api_products_get**](ProductsApi.md#api_products_get) | **GET** /Api/Products | Get a list of all products or only store products depending on permissions
[**api_products_id_delete**](ProductsApi.md#api_products_id_delete) | **DELETE** /Api/Products/{id} | Delete a product
[**api_products_id_get**](ProductsApi.md#api_products_id_get) | **GET** /Api/Products/{id} | Get information about a product
[**api_products_id_patch**](ProductsApi.md#api_products_id_patch) | **PATCH** /Api/Products/{id} | Update a product
[**api_products_post**](ProductsApi.md#api_products_post) | **POST** /Api/Products | Create a new product



## api_products_buy_id_post

> api_products_buy_id_post(id)
Buy a product

Requires verified email; Requires one of the following permissions: Shop.BuyProduct

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


## api_products_get

> Vec<crate::models::ShopIndexProductsResponse> api_products_get(filters, sorts, page, page_size, search)
Get a list of all products or only store products depending on permissions

Requires verified email; Requires one of the following permissions: Shop.IndexProducts, Shop.IndexStoreProducts

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |
**search** | Option<**String**> |  |  |

### Return type

[**Vec<crate::models::ShopIndexProductsResponse>**](ShopIndexProductsResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_products_id_delete

> api_products_id_delete(id)
Delete a product

Requires verified email; Requires one of the following permissions: Shop.DeleteProduct

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


## api_products_id_get

> crate::models::ShopViewProductResponse api_products_id_get(id)
Get information about a product

Requires verified email; Requires one of the following permissions: Shop.ViewProduct, Shop.ViewStoreProduct

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |

### Return type

[**crate::models::ShopViewProductResponse**](ShopViewProductResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_products_id_patch

> api_products_id_patch(id, shop_update_product_request_body)
Update a product

Requires verified email; Requires one of the following permissions: Shop.UpdateProduct

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**shop_update_product_request_body** | Option<[**ShopUpdateProductRequestBody**](ShopUpdateProductRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_products_post

> crate::models::ShopCreateProductResponse api_products_post(shop_create_product_request_body)
Create a new product

Requires verified email; Requires one of the following permissions: Shop.CreateProduct

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**shop_create_product_request_body** | Option<[**ShopCreateProductRequestBody**](ShopCreateProductRequestBody.md)> |  |  |

### Return type

[**crate::models::ShopCreateProductResponse**](ShopCreateProductResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

