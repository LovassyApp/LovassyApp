using Blueboard.Features.Auth.Models;
using Blueboard.Features.Auth.Services.Options;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.Cryptography.Services;
using Microsoft.Extensions.Options;

namespace Blueboard.Features.Auth.Services;

/// <summary>
///     The singleton service responsible for generating and decrypting refresh tokens.
/// </summary>
public class RefreshService(EncryptionService encryptionService, IOptions<RefreshOptions> refreshOptions)
{
    private readonly RefreshOptions _refreshOptions = refreshOptions.Value;

    /// <summary>
    ///     Generates a refresh token for a <see cref="User" />.
    /// </summary>
    /// <param name="userId">The id of <see cref="User" />.</param>
    /// <param name="password">The password of the given <see cref="User" />.</param>
    /// <returns>The refresh token itself.</returns>
    public string GenerateRefreshToken(Guid userId, string password)
    {
        return encryptionService.SerializeProtect(
            new RefreshTokenContents { Password = password, UserId = userId, Purpose = "Refresh" },
            TimeSpan.FromDays(_refreshOptions.ExpiryDays));
    }

    /// <summary>
    ///     Gets the decrypted contents of a refresh token (or null, if the refresh token is already expired).
    /// </summary>
    /// <param name="refreshToken">The refresh token to decrypt.</param>
    /// <returns>The decrypted contents.</returns>
    public RefreshTokenContents? DecryptRefreshToken(string refreshToken)
    {
        try
        {
            var contents = encryptionService.DeserializeUnprotect<RefreshTokenContents>(refreshToken, out _);
            if (contents == null || contents.Purpose != "Refresh") return null;

            return contents;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    ///     Gets the expiry of a refresh token that is generated now. (This is not ms accurate, as it is calculated at a
    ///     different time than the actual generation of the token)
    /// </summary>
    /// <returns>The time span in which the refresh token would expire.</returns>
    public TimeSpan GetRefreshTokenExpiry()
    {
        return TimeSpan.FromDays(_refreshOptions.ExpiryDays);
    }
}