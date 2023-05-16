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
using WebApi.Core.Auth.Services;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Core.Auth.Schemes.Token;

public class TokenAuthenticationSchemeHandler : AuthenticationHandler<TokenAuthenticationSchemeOptions>
{
    private readonly ApplicationDbContext _context;
    private readonly IPublisher _publisher;
    private readonly SessionManager _sessionManager;
    private readonly UserAccessor _userAccessor;

    public TokenAuthenticationSchemeHandler(IOptionsMonitor<TokenAuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ApplicationDbContext context,
        SessionManager sessionManager, IPublisher publisher, UserAccessor userAccessor) : base(options,
        logger, encoder, clock)
    {
        _context = context;
        _sessionManager = sessionManager;
        _publisher = publisher;
        _userAccessor = userAccessor;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(HeaderNames.Authorization) || (Options.HubsBasePath != null &&
                                                                        Request.Path.StartsWithSegments(
                                                                            Options.HubsBasePath) &&
                                                                        !Request.Query.ContainsKey("access_token")))
            return AuthenticateResult.Fail("Token Not Found");

        var token = HttpUtility.UrlDecode(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
        if (Options.HubsBasePath != null && Request.Path.StartsWithSegments(Options.HubsBasePath))
            token = HttpUtility.UrlDecode(Request.Query["access_token"]);

        var accessToken = await _context.PersonalAccessTokens.Include(t => t.User).Where(t => t.Token == token)
            .FirstOrDefaultAsync();

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

        //TODO: Add permission claims with Warden

        await _publisher.Publish(new AccessTokenUsedEvent
        {
            AccessToken = accessToken
        });

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, accessToken.User.Id.ToString()),
            new(ClaimTypes.Name, accessToken.User.Name),
            new(ClaimTypes.Email, accessToken.User.Email),
            new(AuthConstants.EmailVerifiedClaim, (accessToken.User.EmailVerifiedAt != null).ToString())
        }; //TODO: Clean this up a bit, it's gonna get messy with warden

        var identity = new ClaimsIdentity(claims, Scheme.Name);

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }
}