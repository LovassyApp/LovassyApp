using System.Security.Claims;
using WebApi.Core.Auth.Interfaces;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Schemes.Token.ClaimsAdders;

public class TokenEmailConfirmedClaimsAdder : IClaimsAdder<User>
{
    public async Task AddClaimsAsync(List<Claim> claims, User user)
    {
        claims.Add(new Claim(AuthConstants.EmailVerifiedClaim, (user.EmailVerifiedAt != null).ToString()));
    }
}