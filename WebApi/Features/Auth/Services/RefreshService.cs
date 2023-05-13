using Microsoft.Extensions.Options;
using WebApi.Core.Auth.Models;
using WebApi.Core.Cryptography.Services;
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
            new RefreshTokenContents { Password = password, UserId = userId },
            TimeSpan.FromDays(_refreshOptions.ExpiryDays));
    }

    public RefreshTokenContents? DecryptRefreshToken(string refreshToken)
    {
        return _encryptionService.DeserializeUnprotect<RefreshTokenContents>(refreshToken, out _);
    }

    public TimeSpan GetRefreshTokenExpiry()
    {
        return TimeSpan.FromDays(_refreshOptions.ExpiryDays);
    }
}