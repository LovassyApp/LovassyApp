using System.Security.Claims;
using Helpers.Framework.Interfaces;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Schemes.Token.ClaimsAdders;

public class TokenBaseClaimsAdder : IClaimsAdder<User>
{
    public async Task AddClaimsAsync(List<Claim> claims, User user)
    {
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, user.Name));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        claims.Add(new Claim(AuthConstants.EmailVerifiedClaim, (user.EmailVerifiedAt != null).ToString()));
    }
}