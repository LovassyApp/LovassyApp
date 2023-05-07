using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebApi.Core.Cryptography.Exceptions;
using WebApi.Core.Cryptography.Services.Options;
using WebApi.Core.Cryptography.Utils;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Cryptography.Services;

/// <summary>
///     The scoped service responsible for hashing data using the user's encrypted salt. (primarily used for hashing ids)
/// </summary>
public class HashManager
{
    private readonly string _cachePrefix;
    private readonly EncryptionManager _encryptionManager;
    private readonly IMemoryCache _memoryCache;

    private string? _userSalt;

    public HashManager(EncryptionManager encryptionManager, IMemoryCache memoryCache,
        IOptions<HashOptions> options)
    {
        _encryptionManager = encryptionManager;
        _memoryCache = memoryCache;

        _cachePrefix = options.Value.HashManagerCachePrefix;
    }

    /// <summary>
    ///     Hashes a payload with the <see cref="User" />'s own encrypted salt. Primarily meant for hashing the user id and
    ///     lolo ids.
    /// </summary>
    /// <param name="payload">The payload to hash.</param>
    /// <returns>The hashed payload as a string.</returns>
    /// <exception cref="HasherSaltNotFoundException">
    ///     The exception thrown when the <see cref="HashManager" /> has not yet been
    ///     initialized.
    /// </exception>
    public string HashWithHasherSalt(string payload)
    {
        if (_userSalt == null)
            throw new HasherSaltNotFoundException();

        var cached = _memoryCache.Get<string>($"{_cachePrefix}:{payload}");

        if (cached != null)
            return cached;

        var hash = HashingUtils.HashWithSalt(payload, _userSalt);
        _memoryCache.Set($"{_cachePrefix}:{payload}", hash);

        return hash;
    }

    /// <summary>
    ///     Initializes the <see cref="HashManager" /> with the user's encrypted salt.
    /// </summary>
    /// <param name="user">The <see cref="User" />, who's salt is going to be used.</param>
    /// <exception cref="MasterKeyNotFoundException">
    ///     The exception thrown when the <see cref="EncryptionManager" /> has not yet been
    ///     initialized.
    /// </exception>
    public void Init(User user)
    {
        if (_encryptionManager.MasterKey == null)
            throw new MasterKeyNotFoundException();

        _userSalt = _encryptionManager.Decrypt(user.HasherSaltEncrypted);
    }
}