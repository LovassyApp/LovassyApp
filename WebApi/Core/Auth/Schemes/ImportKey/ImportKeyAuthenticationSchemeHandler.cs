using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Core.Auth.Models;
using WebApi.Core.Cryptography.Services;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Core.Auth.Schemes.ImportKey;

public class ImportKeyAuthenticationSchemeHandler : AuthenticationHandler<ImportKeyAuthenticationSchemeOptions>
{
    private readonly IEnumerable<IClaimsAdder<Infrastructure.Persistence.Entities.ImportKey>> _claimsAdders;
    private readonly ApplicationDbContext _context;
    private readonly HashService _hashService;

    public ImportKeyAuthenticationSchemeHandler(IOptionsMonitor<ImportKeyAuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ApplicationDbContext context,
        HashService hashService,
        IEnumerable<IClaimsAdder<Infrastructure.Persistence.Entities.ImportKey>> claimsAdders) : base(options, logger,
        encoder, clock)
    {
        _context = context;
        _hashService = hashService;
        _claimsAdders = claimsAdders;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var key = Request.Headers.FirstOrDefault(q => q.Key == "X-Authorization").Value
            .FirstOrDefault();

        if (key.IsNullOrEmpty())
            return AuthenticateResult.Fail("Import key not found");

        var importKey = await _context.ImportKeys.Where(i => i.KeyHashed == _hashService.Hash(key!))
            .FirstOrDefaultAsync();

        if (!(importKey is { Enabled: true })) return AuthenticateResult.Fail("Invalid import key");

        var claims = new List<Claim>();

        foreach (var claimsAdder in _claimsAdders)
            await claimsAdder.AddClaimsAsync(claims, importKey);

        var identity = new ClaimsIdentity(claims, Scheme.Name);

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }
}