using Blueboard.Features.Auth.Models;
using Blueboard.Features.Auth.Services.Options;
using Helpers.Cryptography.Services;
using Microsoft.Extensions.Options;

namespace Blueboard.Features.Auth.Services;

/// <summary>
///     The singleton service responsible for generating and decrypting password reset tokens.
/// </summary>
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