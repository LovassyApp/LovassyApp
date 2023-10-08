using Helpers.WebApi.Interfaces;

namespace Blueboard.Infrastructure.Files.StartupActions;

public class CreateFileUploadsFolderAction : IStartupAction
{
    public async Task Execute()
    {
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FileUploads");
        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);
    }
}