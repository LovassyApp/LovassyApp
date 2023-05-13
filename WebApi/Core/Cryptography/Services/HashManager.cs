using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebApi.Core.Auth.Exceptions;
using WebApi.Core.Auth.Services;
using WebApi.Core.Cryptography.Services.Options;
using WebApi.Core.Cryptography.Utils;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Cryptography.Services;

/// <summary>
///     The scoped service responsible for hashing data using the user's encrypted salt. (primarily used for hashing ids)
/// </summary>
public class HashManager
{
    private readonly EncryptionManager _encryptionManager;
    private readonly HashOptions _hashOptions;
    private readonly IMemoryCache _memoryCache;
    private readonly UserAccessor _userAccessor;

    private string? _userSalt;

    public HashManager(EncryptionManager encryptionManager, UserAccessor userAccessor, IMemoryCache memoryCache,
        IOptions<HashOptions> options)
    {
        _encryptionManager = encryptionManager;
        _memoryCache = memoryCache;
        _userAccessor = userAccessor;
        _hashOptions = options.Value;
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

        var cached = _memoryCache.Get<string>($"{_hashOptions.HashManagerCachePrefix}:{payload}");

        if (cached != null)
            return cached;

        var hash = HashingUtils.HashWithSalt(payload, _userSalt);
        _memoryCache.Set($"{_hashOptions.HashManagerCachePrefix}:{payload}", hash);

        return hash;
    }

    private void Init()
    {
        var user = _userAccessor.User;

        if (user == null)
            throw new UserNotFoundException();

        _userSalt = _encryptionManager.Decrypt(user.HasherSaltEncrypted);
    }
}