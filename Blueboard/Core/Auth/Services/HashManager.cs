using Blueboard.Core.Auth.Services.Options;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.Cryptography.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Blueboard.Core.Auth.Services;

/// <summary>
///     The scoped service responsible for hashing data using the user's encrypted salt. (primarily used for hashing ids)
/// </summary>
public class HashManager(EncryptionManager encryptionManager, UserAccessor userAccessor,
    IMemoryCache memoryCache, IOptions<HashManagerOptions> hashManagerOptions)
{
    private string? _userSalt;

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

        var cached = memoryCache.Get<string>($"{hashManagerOptions.Value.HashManagerCachePrefix}:{payload}");

        if (cached != null)
            return cached;

        var hash = HashingUtils.HashWithSalt(payload, _userSalt);
        memoryCache.Set($"{hashManagerOptions.Value.HashManagerCachePrefix}:{payload}", hash);

        return hash;
    }

    private void Init()
    {
        _userSalt = encryptionManager.Decrypt(userAccessor.User.HasherSaltEncrypted);
    }
}