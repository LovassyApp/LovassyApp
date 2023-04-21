using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using WebApi.Helpers.Auth.Exceptions;
using WebApi.Helpers.Auth.Models;
using WebApi.Helpers.Cryptography.Traits.Extensions;
using WebApi.Helpers.Cryptography.Traits.Interfaces;
using WebApi.Persistence;
using WebApi.Persistence.Entities;

namespace WebApi.Helpers.Auth.Services;

public class SessionManager : IUsesHashing, IUsesEncryption
{
    private static readonly int ExpiryMinutes = 30;

    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;

    private bool _active;
    private string? _encryptionKey;
    private Session? _session;

    private string? _token;

    public SessionManager(IMemoryCache memoryCache, ApplicationDbContext context)
    {
        _memoryCache = memoryCache;
        _context = context;

        _active = false;
    }

    public void Init(string token)
    {
        _token = token;

        var tokenHash = this.Hash(token);
        var session = _memoryCache.Get<Session>(tokenHash);

        _session = session ?? throw new SessionNotFoundException();
    }

    public async Task<string> StartSessionAsync(User user)
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        _token = Convert.ToBase64String(tokenBytes);

        var expiry = DateTime.Now.AddMinutes(ExpiryMinutes);

        var hash = this.Hash(_token);

        await _context.PersonalAccessTokens.AddAsync(new PersonalAccessToken
        {
            UserId = user.Id,
            Token = _token
        });
        await _context.SaveChangesAsync();

        _session = new Session
        {
            Hash = hash,
            Salt = this.GenerateSalt(),
            UserSalt = user.HasherSalt,
            Expiry = expiry
        };
        _memoryCache.Set(hash, _session, expiry);

        _encryptionKey = this.GenerateBasicKey(_token, _session.Salt);
        _active = true;

        return _token;
    }

    public void Store(string key, string value)
    {
        if (_session == null || _encryptionKey == null)
            throw new SessionNotFoundException();

        _session.Data[key] = this.Encrypt(value, _encryptionKey);
        UpdateCache();
    }

    public string? Retrieve(string key)
    {
        if (_session == null || _encryptionKey == null)
            throw new SessionNotFoundException();

        var encryptedValue = _session.Data.GetValueOrDefault(key);

        if (encryptedValue == null)
            return null;

        return this.Decrypt(encryptedValue, _encryptionKey);
    }

    private void UpdateCache()
    {
        _memoryCache.Set(_session.Hash, _session, _session.Expiry);
    }
}