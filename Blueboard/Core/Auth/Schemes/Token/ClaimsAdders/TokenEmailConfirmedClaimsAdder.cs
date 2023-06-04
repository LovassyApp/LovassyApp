using System.Security.Claims;
using Blueboard.Core.Auth.Interfaces;
using Blueboard.Infrastructure.Persistence.Entities;

namespace Blueboard.Core.Auth.Schemes.Token.ClaimsAdders;

public class TokenEmailConfirmedClaimsAdder : IClaimsAdder<User>
{
    public async Task AddClaimsAsync(List<Claim> claims, User user)
    {
        claims.Add(new Claim(AuthConstants.EmailVerifiedClaim, (user.EmailVerifiedAt != null).ToString()));
    }
}