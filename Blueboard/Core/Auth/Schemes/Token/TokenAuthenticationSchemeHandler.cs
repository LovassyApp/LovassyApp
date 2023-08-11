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
            (isHubPath && isAuthorizationHeaderMissing && !Request.Query.ContainsKey("access_token")))
            return AuthenticateResult.Fail("Token Not Found");

        var token = isAuthorizationHeaderMissing
            ? HttpUtility.UrlDecode(Request.Query["access_token"])
            : HttpUtility.UrlDecode(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));

        try
        {
            _sessionManager.ResumeSession(token);
        }
        catch (SessionNotFoundException)
        {
            return AuthenticateResult.Fail("Session not found");
        }

        // Alright so... A little explanation: We have this as a query here to have the users permissions update real time,
        // because we validate the access token through the SessionManager. This should be pretty fast because the user
        // is cached after the first query and is only queried again if from the database if it's changed.
        var user = await _context.Users.Include(u => u.UserGroups)
            .Where(u => u.Id == _sessionManager.Session!.AccessToken.UserId)
            .AsNoTracking() // This is actually needed here because with tracking the user update endpoint would break (has to do with user groups being tacked)
            .FirstAsync(); // We have to include the user groups here because we need them for the claims

        _userAccessor.User = user;

        await _publisher.Publish(new AccessTokenUsedEvent
        {
            AccessToken = _sessionManager.Session!.AccessToken
        });

        var claims = new List<Claim>();

        foreach (var claimsAdder in _claimsAdders)
            await claimsAdder.AddClaimsAsync(claims, user);

        var identity = new ClaimsIdentity(claims, Scheme.Name);

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }
}