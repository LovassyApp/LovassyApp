using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Web;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using WebApi.Core.Auth.Events;
using WebApi.Core.Auth.Exceptions;
using WebApi.Core.Auth.Interfaces;
using WebApi.Core.Auth.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Schemes.Token;

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

        var accessToken = await _context.PersonalAccessTokens.Include(t => t.User).ThenInclude(u => u.UserGroups)
            .Where(t => t.Token == token)
            .AsNoTracking() // This is actually needed here because with tracking the user update endpoint would break (has to do with user groups being tacked)
            .FirstOrDefaultAsync(); //We have to include the user groups here because we need them for the claims

        if (accessToken == null) return AuthenticateResult.Fail("Invalid access token");

        try
        {
            _sessionManager.ResumeSession(token);
        }
        catch (SessionNotFoundException)
        {
            return AuthenticateResult.Fail("Session not found");
        }

        _userAccessor.User = accessToken.User;

        await _publisher.Publish(new AccessTokenUsedEvent
        {
            AccessToken = accessToken
        });

        var claims = new List<Claim>();

        foreach (var claimsAdder in _claimsAdders)
            await claimsAdder.AddClaimsAsync(claims, accessToken.User);

        var identity = new ClaimsIdentity(claims, Scheme.Name);

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }
}