using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebApi.Helpers.Auth.Exceptions;
using WebApi.Helpers.Auth.Services;
using WebApi.Helpers.Cryptography.Exceptions;
using WebApi.Helpers.Cryptography.Services.Options;
using WebApi.Helpers.Cryptography.Traits;
using WebApi.Persistence.Entities;

namespace WebApi.Helpers.Cryptography.Services;

public class HashManager : IUsesHashing
{
    private readonly string _cachePrefix;
    private readonly EncryptionManager _encryptionManager;
    private readonly IMemoryCache _memoryCache;
    private readonly SessionManager _sessionManager;

    private string? _userSalt;

    public HashManager(SessionManager sessionManager, EncryptionManager encryptionManager, IMemoryCache memoryCache,
        IOptions<HashOptions> options)
    {
        _sessionManager = sessionManager;
        _encryptionManager = encryptionManager;
        _memoryCache = memoryCache;

        _cachePrefix = options.Value.HashManagerCachePrefix;
    }

    public string HashWithHasherSalt(string payload)
    {
        if (_userSalt == null)
            throw new HasherSaltNotFoundException();

        var cached = _memoryCache.Get<string>($"{_cachePrefix}:{payload}");

        if (cached != null)
            return cached;

        var hash = ((IUsesHashing)this)._HashWithSalt(payload, _userSalt);
        _memoryCache.Set($"{_cachePrefix}:{payload}", hash);

        return hash;
    }

    public void Init(User user)
    {
        if (_sessionManager.Session == null)
            throw new SessionNotFoundException();

        if (_encryptionManager.MasterKey == null)
            throw new MasterKeyNotFoundException();

        _userSalt = _encryptionManager.Decrypt(user.HasherSaltEncrypted);
    }
}