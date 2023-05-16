using System.Security.Claims;
using WebApi.Core.Auth.Models;

namespace WebApi.Core.Auth.Schemes.ImportKey.ClaimsAdders;

public class ImportKeyBaseClaimsAdder : IClaimsAdder<Infrastructure.Persistence.Entities.ImportKey>
{
    public async Task AddClaimsAsync(List<Claim> claims, Infrastructure.Persistence.Entities.ImportKey importKey)
    {
        claims.Add(new Claim(ClaimTypes.NameIdentifier, importKey.Id.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, importKey.Name));
    }
}