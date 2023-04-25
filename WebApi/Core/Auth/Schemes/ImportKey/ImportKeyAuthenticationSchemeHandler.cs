using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Core.Cryptography.Services;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Core.Auth.Schemes.ImportKey;

public class ImportKeyAuthenticationSchemeHandler : AuthenticationHandler<ImportKeyAuthenticationSchemeOptions>
{
    private readonly ApplicationDbContext _context;
    private readonly HashService _hashService;

    public ImportKeyAuthenticationSchemeHandler(IOptionsMonitor<ImportKeyAuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ApplicationDbContext context,
        HashService hashService) : base(options,
        logger, encoder, clock)
    {
        _context = context;
        _hashService = hashService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var key = Request.Headers.FirstOrDefault(q => q.Key == "X-Authorization").Value
            .FirstOrDefault();

        if (key == null)
            return AuthenticateResult.Fail("Import key not found");

        var importKey = await _context.ImportKeys.Where(i => i.KeyHashed == _hashService.Hash(key))
            .FirstOrDefaultAsync();

        if (!(importKey is { Enabled: true })) return AuthenticateResult.Fail("Invalid import key");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, importKey.Id.ToString()),
            new(ClaimTypes.Name, importKey.Name)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }
}