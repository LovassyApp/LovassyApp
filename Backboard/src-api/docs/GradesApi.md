# \GradesApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_grades_get**](GradesApi.md#api_grades_get) | **GET** /Api/Grades | 



## api_grades_get

> Vec<crate::models::SchoolIndexGradesResponse> api_grades_get(filters, sorts, page, page_size)


<b>Requires verified email</b><br><b>Requires one of the following permissions</b>: School.IndexGrades<br><b>Requires the following features to be enabled</b>: School

### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**filters** | Option<**String**> |  |  |
**sorts** | Option<**String**> |  |  |
**page** | Option<**i32**> |  |  |
**page_size** | Option<**i32**> |  |  |

### Return type

[**Vec<crate::models::SchoolIndexGradesResponse>**](SchoolIndexGradesResponse.md)

### Authorization

[Token](../README.md#Token)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

