# \FeedItemsApi

All URIs are relative to *https://app.lovassy.hu*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_feed_items_get**](FeedItemsApi.md#api_feed_items_get) | **GET** /Api/FeedItems | Get a list of all feed items



## api_feed_items_get

> Vec<crate::models::FeedIndexFeedItemsResponse> api_feed_items_get(filters, sorts, page, page_size)
Get a list of all feed items

Requires verified email; Requires one of the following permissions: Feed.IndexFeedItems; Requires the following features to be enabled: Feed

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::FeedIndexFeedItemsResponse>**](FeedIndexFeedItemsResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

