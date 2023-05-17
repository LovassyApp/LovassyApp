using Helpers.Cryptography.Services;
using Microsoft.Extensions.Options;
using WebApi.Features.Auth.Models;
using WebApi.Features.Auth.Services.Options;

namespace WebApi.Features.Auth.Services;

public class RefreshService
{
    private readonly EncryptionService _encryptionService;
    private readonly RefreshOptions _refreshOptions;

    public RefreshService(EncryptionService encryptionService, IOptions<RefreshOptions> refreshOptions)
    {
        _encryptionService = encryptionService;
        _refreshOptions = refreshOptions.Value;
    }

    public string GenerateRefreshToken(Guid userId, string password)
    {
        return _encryptionService.SerializeProtect(
            new RefreshTokenContents { Password = password, UserId = userId, Purpose = "Refresh" },
            TimeSpan.FromDays(_refreshOptions.ExpiryDays));
    }

    public RefreshTokenContents? DecryptRefreshToken(string refreshToken)
    {
        try
        {
            var contents = _encryptionService.DeserializeUnprotect<RefreshTokenContents>(refreshToken, out _);
            if (contents is null || contents.Purpose != "Refresh") return null;

            return contents;
        }
        catch
        {
            return null;
        }
    }

    public TimeSpan GetRefreshTokenExpiry()
    {
        return TimeSpan.FromDays(_refreshOptions.ExpiryDays);
    }
}