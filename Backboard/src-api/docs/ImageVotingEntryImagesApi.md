# \ImageVotingEntryImagesApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_image_voting_entry_images_get**](ImageVotingEntryImagesApi.md#api_image_voting_entry_images_get) | **GET** /Api/ImageVotingEntryImages | List all images of an image voting
[**api_image_voting_entry_images_id_delete**](ImageVotingEntryImagesApi.md#api_image_voting_entry_images_id_delete) | **DELETE** /Api/ImageVotingEntryImages/{id} | Delete an image meant for an image voting entry
[**api_image_voting_entry_images_post**](ImageVotingEntryImagesApi.md#api_image_voting_entry_images_post) | **POST** /Api/ImageVotingEntryImages | Upload an image to be used in an image voting entry



## api_image_voting_entry_images_get

> Vec<crate::models::ImageVotingsIndexImageVotingEntryImagesResponse> api_image_voting_entry_images_get(filters, sorts, page, page_size, image_votings_index_image_voting_entry_images_request_body)
List all images of an image voting

Requires verified email; Requires one of the following permissions: ImageVotings.IndexOwnImageVotingEntryImages, ImageVotings.IndexImageVotingEntryImages

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |
**image_votings_index_image_voting_entry_images_request_body** | Option<[**ImageVotingsIndexImageVotingEntryImagesRequestBody**](ImageVotingsIndexImageVotingEntryImagesRequestBody.md)> |  |  |

### Return type

[**Vec<crate::models::ImageVotingsIndexImageVotingEntryImagesResponse>**](ImageVotingsIndexImageVotingEntryImagesResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_voting_entry_images_id_delete

> api_image_voting_entry_images_id_delete(id)
Delete an image meant for an image voting entry

Requires verified email; Requires one of the following permissions: ImageVotings.DeleteOwnImageVotingEntryImage, ImageVotings.DeleteImageVotingEntryImage

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


## api_image_voting_entry_images_post

> crate::models::ImageVotingsUploadImageVotingEntryImageResponse api_image_voting_entry_images_post(image_voting_id, file)
Upload an image to be used in an image voting entry

Requires verified email; Requires one of the following permissions: ImageVotings.UploadActiveImageVotingEntryImage, ImageVotings.UploadImageVotingEntryImage

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**image_voting_id** | Option<**i32**> |  |  |
**file** | Option<**std::path::PathBuf**> |  |  |

### Return type

[**crate::models::ImageVotingsUploadImageVotingEntryImageResponse**](ImageVotingsUploadImageVotingEntryImageResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: multipart/form-data
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

