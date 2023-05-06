using System.Security.Claims;
using System.Text.Encodings.Web;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using WebApi.Core.Auth.Exceptions;
using WebApi.Core.Auth.Jobs;
using WebApi.Core.Auth.Services;
using WebApi.Core.Cryptography.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Schemes.Token;

public class TokenAuthenticationSchemeHandler : AuthenticationHandler<TokenAuthenticationSchemeOptions>
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ApplicationDbContext _context;
    private readonly EncryptionManager _encryptionManager;
    private readonly HashManager _hashManager;
    private readonly SessionManager _sessionManager;

    public TokenAuthenticationSchemeHandler(IOptionsMonitor<TokenAuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ApplicationDbContext context,
        SessionManager sessionManager, EncryptionManager encryptionManager, HashManager hashManager,
        IBackgroundJobClient backgroundJobClient) : base(options,
        logger, encoder, clock)
    {
        _context = context;
        _sessionManager = sessionManager;
        _encryptionManager = encryptionManager;
        _hashManager = hashManager;
        _backgroundJobClient = backgroundJobClient;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(HeaderNames.Authorization) || (Options.HubsBasePath != null &&
                                                                        Request.Path.StartsWithSegments(
                                                                            Options.HubsBasePath) &&
                                                                        !Request.Query.ContainsKey("access_token")))
            return AuthenticateResult.Fail("Token Not Found");

        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        if (Options.HubsBasePath != null && Request.Path.StartsWithSegments(Options.HubsBasePath))
            token = Request.Query["access_token"];

        var accessToken = await _context.PersonalAccessTokens.Include(t => t.User).Where(t => t.Token == token)
            .FirstOrDefaultAsync();

        if (accessToken == null) return AuthenticateResult.Fail("Invalid access token");

        try
        {
            InitializeManagers(token!, accessToken.User);
        }
        catch (SessionNotFoundException)
        {
            return AuthenticateResult.Fail("Session not found");
        }

        //TODO: Add permission claims with Warden

        _backgroundJobClient.Enqueue<UpdateTokenLastUsedAtJob>(j =>
            j.Run(accessToken.Id, DateTime.Now.ToUniversalTime()));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, accessToken.User.Id.ToString()),
            new(ClaimTypes.Name, accessToken.User.Name),
            new(ClaimTypes.Email, accessToken.User.Email)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }

    private void InitializeManagers(string token, User user)
    {
        _sessionManager.Init(token);
        _encryptionManager.Init();
        _hashManager.Init(user);
    }
}