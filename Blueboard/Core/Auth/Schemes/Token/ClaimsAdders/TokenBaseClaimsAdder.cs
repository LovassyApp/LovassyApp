using System.Security.Claims;
using Blueboard.Core.Auth.Interfaces;
using Blueboard.Infrastructure.Persistence.Entities;

namespace Blueboard.Core.Auth.Schemes.Token.ClaimsAdders;

public class TokenBaseClaimsAdder : IClaimsAdder<User>
{
    public async Task AddClaimsAsync(List<Claim> claims, User user)
    {
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, user.Name));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
    }
}