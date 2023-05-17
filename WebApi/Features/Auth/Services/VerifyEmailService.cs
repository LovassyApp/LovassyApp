using Helpers.Cryptography.Services;
using Microsoft.Extensions.Options;
using WebApi.Features.Auth.Models;
using WebApi.Features.Auth.Services.Options;

namespace WebApi.Features.Auth.Services;

public class VerifyEmailService
{
    private readonly EncryptionService _encryptionService;
    private readonly VerifyEmailOptions _verifyEmailOptions;

    public VerifyEmailService(EncryptionService encryptionService, IOptions<VerifyEmailOptions> verifyEmailOptions)
    {
        _encryptionService = encryptionService;
        _verifyEmailOptions = verifyEmailOptions.Value;
    }

    public string GenerateVerifyToken(Guid userId)
    {
        return _encryptionService.SerializeProtect(
            new VerifyEmailTokenContents { UserId = userId, Purpose = "VerifyEmail" },
            TimeSpan.FromMinutes(_verifyEmailOptions.ExpiryMinutes));
    }

    public VerifyEmailTokenContents? DecryptVerifyToken(string verifyToken)
    {
        try
        {
            var contents = _encryptionService.DeserializeUnprotect<VerifyEmailTokenContents>(verifyToken, out _);
            if (contents is null || contents.Purpose != "VerifyEmail") return null;

            return contents;
        }
        catch
        {
            return null;
        }
    }
}