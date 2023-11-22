using System.Security.Claims;
using System.Text.Encodings.Web;
using Blueboard.Core.Auth.Interfaces;
using Blueboard.Infrastructure.Persistence;
using Helpers.Cryptography.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Blueboard.Core.Auth.Schemes.ImportKey;

/// <summary>
///     The handler for the import key authentication scheme. It logs in a user represented by the
///     <see cref="Infrastructure.Persistence.Entities.ImportKey" /> entity. It is only meant to be used by the school
///     administration to import data.
/// </summary>
public class ImportKeyAuthenticationSchemeHandler(IOptionsMonitor<ImportKeyAuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ApplicationDbContext context,
        HashService hashService,
        IEnumerable<IClaimsAdder<Infrastructure.Persistence.Entities.ImportKey>> claimsAdders)
    : AuthenticationHandler<ImportKeyAuthenticationSchemeOptions>(options, logger,
        encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var key = Request.Headers
            .FirstOrDefault(q => string.Equals(q.Key, "X-Authorization", StringComparison.OrdinalIgnoreCase)).Value
            .FirstOrDefault(); //Amazingly, the generated rust code uses X-Authorization in lowercase... For some reason...

        if (key.IsNullOrEmpty())
            return AuthenticateResult.Fail("Import key not found");

        var importKey = await context.ImportKeys.Where(i => i.KeyHashed == hashService.Hash(key!))
            .AsNoTracking().FirstOrDefaultAsync();

        if (!(importKey is { Enabled: true })) return AuthenticateResult.Fail("Invalid import key");

        var claims = new List<Claim>();

        foreach (var claimsAdder in claimsAdders)
            await claimsAdder.AddClaimsAsync(claims, importKey);

        var identity = new ClaimsIdentity(claims, Scheme.Name);

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }
}