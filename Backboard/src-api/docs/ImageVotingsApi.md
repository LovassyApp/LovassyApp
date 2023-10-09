# \ImageVotingsApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_image_votings_get**](ImageVotingsApi.md#api_image_votings_get) | **GET** /Api/ImageVotings | Get a list of image votings
[**api_image_votings_id_delete**](ImageVotingsApi.md#api_image_votings_id_delete) | **DELETE** /Api/ImageVotings/{id} | Delete an image voting
[**api_image_votings_id_get**](ImageVotingsApi.md#api_image_votings_id_get) | **GET** /Api/ImageVotings/{id} | Get information about an image voting
[**api_image_votings_id_patch**](ImageVotingsApi.md#api_image_votings_id_patch) | **PATCH** /Api/ImageVotings/{id} | Update an image voting
[**api_image_votings_id_results_get**](ImageVotingsApi.md#api_image_votings_id_results_get) | **GET** /Api/ImageVotings/{id}/Results | Get results of an image voting
[**api_image_votings_post**](ImageVotingsApi.md#api_image_votings_post) | **POST** /Api/ImageVotings | Create a new image voting



## api_image_votings_get

> Vec<crate::models::ImageVotingsIndexImageVotingsResponse> api_image_votings_get(filters, sorts, page, page_size)
Get a list of image votings

Requires verified email; Requires one of the following permissions: ImageVotings.IndexImageVotings, ImageVotings.IndexActiveImageVotings; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::ImageVotingsIndexImageVotingsResponse>**](ImageVotingsIndexImageVotingsResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_votings_id_delete

> api_image_votings_id_delete(id)
Delete an image voting

Requires verified email; Requires one of the following permissions: ImageVotings.DeleteImageVoting; Requires the following features to be enabled: ImageVotings

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


## api_image_votings_id_get

> crate::models::ImageVotingsViewImageVotingResponse api_image_votings_id_get(id)
Get information about an image voting

Requires verified email; Requires one of the following permissions: ImageVotings.ViewImageVoting, ImageVotings.ViewActiveImageVoting; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |

### Return type

[**crate::models::ImageVotingsViewImageVotingResponse**](ImageVotingsViewImageVotingResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_votings_id_patch

> api_image_votings_id_patch(id, image_votings_update_image_voting_request_body)
Update an image voting

Requires verified email; Requires one of the following permissions: ImageVotings.UpdateImageVoting; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**image_votings_update_image_voting_request_body** | Option<[**ImageVotingsUpdateImageVotingRequestBody**](ImageVotingsUpdateImageVotingRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_votings_id_results_get

> crate::models::ImageVotingsViewImageVotingResultsResponse api_image_votings_id_results_get(id)
Get results of an image voting

Requires verified email; Requires one of the following permissions: ImageVotings.ViewImageVotingResults, ImageVotings.ViewActiveImageVotingResults; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |

### Return type

[**crate::models::ImageVotingsViewImageVotingResultsResponse**](ImageVotingsViewImageVotingResultsResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_votings_post

> crate::models::ImageVotingsCreateImageVotingResponse api_image_votings_post(image_votings_create_image_voting_request_body)
Create a new image voting

Requires verified email; Requires one of the following permissions: ImageVotings.CreateImageVoting; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**image_votings_create_image_voting_request_body** | Option<[**ImageVotingsCreateImageVotingRequestBody**](ImageVotingsCreateImageVotingRequestBody.md)> |  |  |

### Return type

[**crate::models::ImageVotingsCreateImageVotingResponse**](ImageVotingsCreateImageVotingResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

