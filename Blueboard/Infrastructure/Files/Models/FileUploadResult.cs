using Blueboard.Infrastructure.Files.Services;
using Blueboard.Infrastructure.Persistence.Entities;

namespace Blueboard.Infrastructure.Files.Models;

/// <summary>
///     The model representing the result of a file upload done using <see cref="FilesService.UploadFileAsync" />.
/// </summary>
public class FileUploadResult
{
    public FileUpload FileUpload { get; set; }
    public string? Url { get; set; }
}