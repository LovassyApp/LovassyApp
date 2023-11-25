# \ImageVotingEntriesApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_image_voting_entries_get**](ImageVotingEntriesApi.md#api_image_voting_entries_get) | **GET** /Api/ImageVotingEntries | Get a list of all image voting entries
[**api_image_voting_entries_id_choice_delete**](ImageVotingEntriesApi.md#api_image_voting_entries_id_choice_delete) | **DELETE** /Api/ImageVotingEntries/{id}/Choice | Unchoose an image voting entry (SingleChoice image votings only)
[**api_image_voting_entries_id_choice_post**](ImageVotingEntriesApi.md#api_image_voting_entries_id_choice_post) | **POST** /Api/ImageVotingEntries/{id}/Choice | Choose an image voting entry (SingleChoice image votings only)
[**api_image_voting_entries_id_delete**](ImageVotingEntriesApi.md#api_image_voting_entries_id_delete) | **DELETE** /Api/ImageVotingEntries/{id} | Delete an image voting entry
[**api_image_voting_entries_id_get**](ImageVotingEntriesApi.md#api_image_voting_entries_id_get) | **GET** /Api/ImageVotingEntries/{id} | Get information about an image voting entry
[**api_image_voting_entries_id_increment_delete**](ImageVotingEntriesApi.md#api_image_voting_entries_id_increment_delete) | **DELETE** /Api/ImageVotingEntries/{id}/Increment | Delete an image voting entry increment (Increment image votings only)
[**api_image_voting_entries_id_increment_post**](ImageVotingEntriesApi.md#api_image_voting_entries_id_increment_post) | **POST** /Api/ImageVotingEntries/{id}/Increment | Increment an image voting entry (Increment image votings only)
[**api_image_voting_entries_id_patch**](ImageVotingEntriesApi.md#api_image_voting_entries_id_patch) | **PATCH** /Api/ImageVotingEntries/{id} | Update an image voting entry
[**api_image_voting_entries_post**](ImageVotingEntriesApi.md#api_image_voting_entries_post) | **POST** /Api/ImageVotingEntries | Create an image voting entry



## api_image_voting_entries_get

> Vec<crate::models::ImageVotingsIndexImageVotingEntriesResponse> api_image_voting_entries_get(filters, sorts, page, page_size)
Get a list of all image voting entries

Requires verified email; Requires one of the following permissions: ImageVotings.IndexImageVotingEntries, ImageVotings.IndexActiveImageVotingEntries; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::ImageVotingsIndexImageVotingEntriesResponse>**](ImageVotingsIndexImageVotingEntriesResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_voting_entries_id_choice_delete

> api_image_voting_entries_id_choice_delete(id, image_votings_unchoose_image_voting_entry_request_body)
Unchoose an image voting entry (SingleChoice image votings only)

Requires verified email; Requires one of the following permissions: ImageVotings.UnchooseActiveImageVotingEntry, ImageVotings.UnchooseImageVotingEntry; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**image_votings_unchoose_image_voting_entry_request_body** | Option<[**ImageVotingsUnchooseImageVotingEntryRequestBody**](ImageVotingsUnchooseImageVotingEntryRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_voting_entries_id_choice_post

> api_image_voting_entries_id_choice_post(id, image_votings_choose_image_voting_entry_request_body)
Choose an image voting entry (SingleChoice image votings only)

Requires verified email; Requires one of the following permissions: ImageVotings.ChooseActiveImageVotingEntry, ImageVotings.ChooseImageVotingEntry; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**image_votings_choose_image_voting_entry_request_body** | Option<[**ImageVotingsChooseImageVotingEntryRequestBody**](ImageVotingsChooseImageVotingEntryRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_voting_entries_id_delete

> api_image_voting_entries_id_delete(id)
Delete an image voting entry

Requires verified email; Requires one of the following permissions: ImageVotings.DeleteImageVotingEntry, ImageVotings.DeleteOwnImageVotingEntry; Requires the following features to be enabled: ImageVotings

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


## api_image_voting_entries_id_get

> crate::models::ImageVotingsViewImageVotingEntryResponse api_image_voting_entries_id_get(id)
Get information about an image voting entry

Requires verified email; Requires one of the following permissions: ImageVotings.ViewImageVotingEntry, ImageVotings.ViewActiveImageVotingEntry; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |

### Return type

[**crate::models::ImageVotingsViewImageVotingEntryResponse**](ImageVotingsViewImageVotingEntryResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_voting_entries_id_increment_delete

> api_image_voting_entries_id_increment_delete(id, image_votings_delete_image_voting_entry_increment_request_body)
Delete an image voting entry increment (Increment image votings only)

Requires verified email; Requires one of the following permissions: ImageVotings.DeleteImageVotingEntryIncrement, ImageVotings.DeleteActiveImageVotingEntryIncrement; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**image_votings_delete_image_voting_entry_increment_request_body** | Option<[**ImageVotingsDeleteImageVotingEntryIncrementRequestBody**](ImageVotingsDeleteImageVotingEntryIncrementRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_voting_entries_id_increment_post

> api_image_voting_entries_id_increment_post(id, image_votings_create_image_voting_entry_increment_request_body)
Increment an image voting entry (Increment image votings only)

Requires verified email; Requires one of the following permissions: ImageVotings.CreateImageVotingEntryIncrement, ImageVotings.CreateActiveImageVotingEntryIncrement; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**image_votings_create_image_voting_entry_increment_request_body** | Option<[**ImageVotingsCreateImageVotingEntryIncrementRequestBody**](ImageVotingsCreateImageVotingEntryIncrementRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_voting_entries_id_patch

> api_image_voting_entries_id_patch(id, image_votings_update_image_voting_entry_request_body)
Update an image voting entry

Requires verified email; Requires one of the following permissions: ImageVotings.UpdateImageVotingEntry, ImageVotings.UpdateOwnImageVotingEntry; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **i32** |  | [required] |
**image_votings_update_image_voting_entry_request_body** | Option<[**ImageVotingsUpdateImageVotingEntryRequestBody**](ImageVotingsUpdateImageVotingEntryRequestBody.md)> |  |  |

### Return type

 (empty response body)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_image_voting_entries_post

> crate::models::ImageVotingsCreateImageVotingEntryResponse api_image_voting_entries_post(image_votings_create_image_voting_entry_request_body)
Create an image voting entry

Requires verified email; Requires one of the following permissions: ImageVotings.ChooseImageVotingEntry, ImageVotings.CreateActiveImageVotingEntry; Requires the following features to be enabled: ImageVotings

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**image_votings_create_image_voting_entry_request_body** | Option<[**ImageVotingsCreateImageVotingEntryRequestBody**](ImageVotingsCreateImageVotingEntryRequestBody.md)> |  |  |

### Return type

[**crate::models::ImageVotingsCreateImageVotingEntryResponse**](ImageVotingsCreateImageVotingEntryResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

