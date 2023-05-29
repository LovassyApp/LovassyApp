using Helpers.Cryptography.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebApi.Core.Auth.Services.Options;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Services;

/// <summary>
///     The scoped service responsible for hashing data using the user's encrypted salt. (primarily used for hashing ids)
/// </summary>
public class HashManager
{
    private readonly EncryptionManager _encryptionManager;
    private readonly HashManagerOptions _hashManagerOptions;
    private readonly IMemoryCache _memoryCache;
    private readonly UserAccessor _userAccessor;

    private string? _userSalt;

    public HashManager(EncryptionManager encryptionManager, UserAccessor userAccessor,
        IMemoryCache memoryCache,
        IOptions<HashManagerOptions> hashManagerOptions)
    {
        _encryptionManager = encryptionManager;
        _memoryCache = memoryCache;
        _userAccessor = userAccessor;
        _hashManagerOptions = hashManagerOptions.Value;
    }

    /// <summary>
    ///     Hashes a payload with the <see cref="User" />'s own encrypted salt. Primarily meant for hashing the user id and
    ///     lolo ids.
    /// </summary>
    /// <param name="payload">The payload to hash.</param>
    /// <returns>The hashed payload as a string.</returns>
    public string HashWithHasherSalt(string payload)
    {
        if (_userSalt == null)
            Init();

        var cached = _memoryCache.Get<string>($"{_hashManagerOptions.HashManagerCachePrefix}:{payload}");

        if (cached != null)
            return cached;

        var hash = HashingUtils.HashWithSalt(payload, _userSalt);
        _memoryCache.Set($"{_hashManagerOptions.HashManagerCachePrefix}:{payload}", hash);

        return hash;
    }

    private void Init()
    {
        _userSalt = _encryptionManager.Decrypt(_userAccessor.User.HasherSaltEncrypted);
    }
}