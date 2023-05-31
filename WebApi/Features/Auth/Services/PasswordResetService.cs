using Helpers.Cryptography.Services;
using Microsoft.Extensions.Options;
using WebApi.Features.Auth.Models;
using WebApi.Features.Auth.Services.Options;

namespace WebApi.Features.Auth.Services;

public class PasswordResetService
{
    private readonly EncryptionService _encryptionService;
    private readonly PasswordResetOptions _options;

    public PasswordResetService(EncryptionService encryptionService, IOptions<PasswordResetOptions> options)
    {
        _encryptionService = encryptionService;
        _options = options.Value;
    }

    public string GeneratePasswordResetToken(Guid userId)
    {
        return _encryptionService.SerializeProtect(
            new PasswordResetTokenContents { UserId = userId, Purpose = "PasswordReset" },
            TimeSpan.FromMinutes(_options.ExpiryMinutes));
    }

    public PasswordResetTokenContents? DecryptPasswordResetToken(string passwordResetToken)
    {
        try
        {
            var contents =
                _encryptionService.DeserializeUnprotect<PasswordResetTokenContents>(passwordResetToken, out _);
            if (contents == null || contents.Purpose != "PasswordReset") return null;

            return contents;
        }
        catch
        {
            return null;
        }
    }
}