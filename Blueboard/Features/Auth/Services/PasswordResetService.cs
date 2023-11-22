using Blueboard.Features.Auth.Models;
using Blueboard.Features.Auth.Services.Options;
using Helpers.Cryptography.Services;
using Microsoft.Extensions.Options;

namespace Blueboard.Features.Auth.Services;

/// <summary>
///     The singleton service responsible for generating and decrypting password reset tokens.
/// </summary>
public class PasswordResetService(EncryptionService encryptionService,
    IOptions<PasswordResetOptions> passwordResetOptions)
{
    public string GeneratePasswordResetToken(Guid userId)
    {
        return encryptionService.SerializeProtect(
            new PasswordResetTokenContents { UserId = userId, Purpose = "PasswordReset" },
            TimeSpan.FromMinutes(passwordResetOptions.Value.ExpiryMinutes));
    }

    public PasswordResetTokenContents? DecryptPasswordResetToken(string passwordResetToken)
    {
        try
        {
            var contents =
                encryptionService.DeserializeUnprotect<PasswordResetTokenContents>(passwordResetToken, out _);
            if (contents == null || contents.Purpose != "PasswordReset") return null;

            return contents;
        }
        catch
        {
            return null;
        }
    }
}