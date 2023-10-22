# \ImageVotingChoicesApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_image_voting_choices_get**](ImageVotingChoicesApi.md#api_image_voting_choices_get) | **GET** /Api/ImageVotingChoices | Get a list of all image voting choices
[**api_image_voting_choices_id_get**](ImageVotingChoicesApi.md#api_image_voting_choices_id_get) | **GET** /Api/ImageVotingChoices/{id} | Get information about an image voting choice



## api_image_voting_choices_get

> Vec<crate::models::ImageVotingsIndexImageVotingChoicesResponse> api_image_voting_choices_get(filters, sorts, page, page_size)
Get a list of all image voting choices

Requires verified email; Requires one of the following permissions: ImageVotings.IndexImageVotingChoices, ImageVotings.IndexActiveImageVotingChoices; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::ImageVotingsIndexImageVotingChoicesResponse>**](ImageVotingsIndexImageVotingChoicesResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_voting_choices_id_get

> crate::models::ImageVotingsViewImageVotingChoiceResponse api_image_voting_choices_id_get(id)
Get information about an image voting choice

Requires verified email; Requires one of the following permissions: ImageVotings.ViewImageVotingChoice, ImageVotings.ViewActiveImageVotingChoice; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |

### Return type

[**crate::models::ImageVotingsViewImageVotingChoiceResponse**](ImageVotingsViewImageVotingChoiceResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

