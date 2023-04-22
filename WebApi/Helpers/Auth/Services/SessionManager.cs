using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebApi.Helpers.Auth.Exceptions;
using WebApi.Helpers.Auth.Models;
using WebApi.Helpers.Cryptography.Utils;
using WebApi.Persistence;
using WebApi.Persistence.Entities;
using SessionOptions = WebApi.Helpers.Auth.Services.Options.SessionOptions;

namespace WebApi.Helpers.Auth.Services;

public class SessionManager
{
    private readonly ApplicationDbContext _context;
    private readonly int _expiryMinute;
    private readonly IMemoryCache _memoryCache;

    private string? _encryptionKey;

    private string? _token;

    public SessionManager(IMemoryCache memoryCache, ApplicationDbContext context, IOptions<SessionOptions> options)
    {
        _memoryCache = memoryCache;
        _context = context;
        _expiryMinute = options.Value.ExpiryMinutes;
    }

    public Session? Session { get; set; }

    public void Init(string token)
    {
        _token = token;

        var tokenHash = HashingUtils.Hash(token);
        var session = _memoryCache.Get<Session>(tokenHash);

        Session = session ?? throw new SessionNotFoundException();
    }

    public async Task<string> StartSessionAsync(User user)
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        _token = Convert.ToBase64String(tokenBytes);

        var expiry = DateTime.Now.AddMinutes(_expiryMinute);

        var hash = HashingUtils.Hash(_token);

        await _context.PersonalAccessTokens.AddAsync(new PersonalAccessToken
        {
            UserId = user.Id,
            Token = _token
        });
        await _context.SaveChangesAsync();

        Session = new Session
        {
            Hash = hash,
            Salt = HashingUtils.GenerateSalt(),
            Expiry = expiry
        };
        UpdateCache();

        _encryptionKey = HashingUtils.GenerateBasicKey(_token, Session.Salt);

        return _token;
    }

    public void Set(string key, string value)
    {
        if (Session == null || _encryptionKey == null)
            throw new SessionNotFoundException();

        Session.Data[key] = EncryptionUtils.Encrypt(value, _encryptionKey);
        UpdateCache();
    }

    public string? Get(string key)
    {
        if (Session == null || _encryptionKey == null)
            throw new SessionNotFoundException();

        var encryptedValue = Session.Data.GetValueOrDefault(key);

        if (encryptedValue == null)
            return null;

        return EncryptionUtils.Decrypt(encryptedValue, _encryptionKey);
    }

    private void UpdateCache()
    {
        _memoryCache.Set(Session!.Hash, Session, Session.Expiry);
    }
}