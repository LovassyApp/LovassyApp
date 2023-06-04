using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Web;
using Blueboard.Core.Auth.Events;
using Blueboard.Core.Auth.Exceptions;
using Blueboard.Core.Auth.Interfaces;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Blueboard.Core.Auth.Schemes.Token;

/// <summary>
///     The handler for the default token authentication scheme for regular users. It logs in a user represented by the
///     <see cref="User" /> entity.
/// </summary>
public class TokenAuthenticationSchemeHandler : AuthenticationHandler<TokenAuthenticationSchemeOptions>
{
    private readonly IEnumerable<IClaimsAdder<User>> _claimsAdders;
    private readonly ApplicationDbContext _context;
    private readonly IPublisher _publisher;
    private readonly SessionManager _sessionManager;
    private readonly UserAccessor _userAccessor;

    public TokenAuthenticationSchemeHandler(IOptionsMonitor<TokenAuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ApplicationDbContext context,
        SessionManager sessionManager, IPublisher publisher, UserAccessor userAccessor,
        IEnumerable<IClaimsAdder<User>> claimsAdders) : base(options, logger, encoder, clock)
    {
        _context = context;
        _sessionManager = sessionManager;
        _publisher = publisher;
        _userAccessor = userAccessor;
        _claimsAdders = claimsAdders;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var isAuthorizationHeaderMissing = !Request.Headers.ContainsKey(HeaderNames.Authorization);
        var isHubPath = Options.HubsBasePath != null && Request.Path.StartsWithSegments(Options.HubsBasePath);

        if ((isAuthorizationHeaderMissing && Options.HubsBasePath == null) ||
            (isAuthorizationHeaderMissing && !isHubPath) ||
            (isHubPath && !Request.Query.ContainsKey("access_token")))
            return AuthenticateResult.Fail("Token Not Found");

        var token = !isHubPath
            ? HttpUtility.UrlDecode(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""))
            : HttpUtility.UrlDecode(Request.Query["access_token"]);

        var accessToken = await _context.PersonalAccessTokens
            .Where(t => t.Token == token)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (accessToken == null) return AuthenticateResult.Fail("Invalid access token");

        // Alright so... A little explanation: We have this as a separate query because after the auth workflow the tokens get updated
        // with a new last used date. This update invalidates the second level cache entry for the token making it necessary to query
        // the database again. This is fine as that is a pretty fast query but if we were to include the user and it's groups there as
        // well it would be a lot slower. So we just query the token and then query the user and it's groups separately (the latter from the cache).
        var user = await _context.Users.Include(u => u.UserGroups).Where(u => u.Id == accessToken.UserId)
            .AsNoTracking() // This is actually needed here because with tracking the user update endpoint would break (has to do with user groups being tacked)
            .FirstAsync(); // We have to include the user groups here because we need them for the claims

        try
        {
            _sessionManager.ResumeSession(token);
        }
        catch (SessionNotFoundException)
        {
            return AuthenticateResult.Fail("Session not found");
        }

        _userAccessor.User = user;

        await _publisher.Publish(new AccessTokenUsedEvent
        {
            AccessToken = accessToken
        });

        var claims = new List<Claim>();

        foreach (var claimsAdder in _claimsAdders)
            await claimsAdder.AddClaimsAsync(claims, user);

        var identity = new ClaimsIdentity(claims, Scheme.Name);

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }
}