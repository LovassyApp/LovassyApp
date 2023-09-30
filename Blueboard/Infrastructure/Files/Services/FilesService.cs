using Blueboard.Infrastructure.Files.Models;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;

namespace Blueboard.Infrastructure.Files.Services;

/// <summary>
///     The scoped service responsible for handling file <see cref="FileUpload">FileUploads</see>.
/// </summary>
public class FilesService
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    public FilesService(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    ///     Uploads the file to the server and creates a new <see cref="FileUpload" /> entity. It does not save the changes to
    ///     the database.
    /// </summary>
    /// <param name="file">The <see cref="IFormFile" /> to upload.</param>
    /// <param name="userId">The id of the <see cref="User" /> that uploaded the file.</param>
    /// <param name="purpose">The purpose of the file, used to limit the amount of files uploadable by a <see cref="User" />.</param>
    /// <returns>
    ///     The <see cref="FileUploadResult" /> containing the <see cref="FileUpload" /> (tracked) entity. If the
    ///     <see cref="HttpContext" /> is available it also contains an URL to the file.
    /// </returns>
    public async Task<FileUploadResult> UploadFileAsync(IFormFile file, Guid userId, string purpose)
    {
        var uniqueFileName = GetUniqueFileName(file.FileName);
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FileUploads");
        var filePath = Path.Combine(uploadsPath, uniqueFileName);

        await file.CopyToAsync(new FileStream(filePath, FileMode.Create));

        var fileUpload = new FileUpload
        {
            Filename = uniqueFileName,
            OriginalFilename = file.FileName,
            Path = filePath,
            MimeType = file.ContentType,
            Purpose = purpose,
            UserId = userId
        };

        _context.FileUploads.Add(fileUpload);

        var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();

        if (httpContextAccessor == null || httpContextAccessor.HttpContext == null)
            return new FileUploadResult
            {
                FileUpload = fileUpload
            };

        var request = httpContextAccessor.HttpContext.Request;

        return new FileUploadResult
        {
            FileUpload = fileUpload,
            Url = $"{request.Scheme}://{request.Host}{request.PathBase}/Files/{fileUpload.Filename}"
        };
    }

    /// <summary>
    ///     Deletes the file from the server and removes the <see cref="FileUpload" /> entity from the database. It does not
    ///     save the changes to the database.
    /// </summary>
    /// <param name="fileUpload">The <see cref="FileUpload" /> entity to delete.</param>
    public void DeleteFile(FileUpload fileUpload)
    {
        File.Delete(fileUpload.Path);

        _context.FileUploads.Remove(fileUpload);
    }

    private string GetUniqueFileName(string fileName)
    {
        fileName = Path.GetFileName(fileName);
        return string.Concat(Path.GetFileNameWithoutExtension(fileName)
            , "_"
            , Guid.NewGuid().ToString().AsSpan(0, 8)
            , Path.GetExtension(fileName));
    }
}