using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApi.Helpers.Auth.Exceptions;
using WebApi.Helpers.Auth.Models;
using WebApi.Helpers.Cryptography.Traits.Extensions;
using WebApi.Helpers.Cryptography.Traits.Interfaces;
using WebApi.Persistence;

namespace WebApi.Helpers.Auth.Services;

public class SessionManager : IUsesHashing
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private Session? _session;

    private string? _token;

    public SessionManager(IMemoryCache memoryCache, ApplicationDbContext context)
    {
        _memoryCache = memoryCache;
        _context = context;
    }

    public async Task InitAsync(string token)
    {
        _token = token;
        var tokenHash = MakeTokenHash(token);

        if (!await _context.PersonalAccessTokens.Where(t => t.Token == tokenHash).AnyAsync())
            throw new TokenMissingException();

        var session = _memoryCache.Get<Session>(tokenHash);

        _session = session ?? throw new SessionNotFoundException();
    }

    private string MakeTokenHash(string token)
    {
        var tokenString = token.Split("|")[1];
        return this.Hash(tokenString);
    }
}