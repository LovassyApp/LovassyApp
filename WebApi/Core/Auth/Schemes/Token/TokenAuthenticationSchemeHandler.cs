using System.Globalization;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Quartz;
using WebApi.Core.Auth.Exceptions;
using WebApi.Core.Auth.Jobs;
using WebApi.Core.Auth.Services;
using WebApi.Core.Cryptography.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Schemes.Token;

public class TokenAuthenticationSchemeHandler : AuthenticationHandler<TokenAuthenticationSchemeOptions>
{
    private readonly ApplicationDbContext _context;
    private readonly EncryptionManager _encryptionManager;
    private readonly HashManager _hashManager;
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly SessionManager _sessionManager;

    public TokenAuthenticationSchemeHandler(IOptionsMonitor<TokenAuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ApplicationDbContext context,
        SessionManager sessionManager, EncryptionManager encryptionManager, HashManager hashManager,
        ISchedulerFactory schedulerFactory) : base(options,
        logger, encoder, clock)
    {
        _context = context;
        _sessionManager = sessionManager;
        _encryptionManager = encryptionManager;
        _hashManager = hashManager;
        _schedulerFactory = schedulerFactory;
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
            InitializeManagers(token, accessToken.User);
        }
        catch (SessionNotFoundException)
        {
            return AuthenticateResult.Fail("Session not found");
        }

        //TODO: Add permission claims with Warden

        await ScheduleOnTokenUsedJobsAsync(accessToken.Id, DateTime.Now);

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

    private async Task ScheduleOnTokenUsedJobsAsync(int id, DateTime lastUsedAt)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var updateTokenLastUsedAtJob = JobBuilder.Create<UpdateTokenLastUsedAtJob>()
            .WithIdentity("updateTokenLastUsedAtJob", "onTokenUsedJobs")
            .UsingJobData("id", id)
            .UsingJobData("lastUsedAt", lastUsedAt.ToString(CultureInfo.InvariantCulture))
            .Build();

        var updateTokenLastUsedAtTrigger = TriggerBuilder.Create()
            .WithIdentity("updateTokenLastUsedAtTrigger", "onTokenUsedJobs")
            .StartNow()
            .Build();

        await scheduler.ScheduleJob(updateTokenLastUsedAtJob, updateTokenLastUsedAtTrigger);
    }
}