using System.Security.Claims;
using Blueboard.Core.Auth.Interfaces;
using Blueboard.Infrastructure.Persistence.Entities;

namespace Blueboard.Core.Auth.Schemes.Token.ClaimsAdders;

public class TokenFeatureGroupsClaimsAdder : IClaimsAdder<User>
{
    public async Task AddClaimsAsync(List<Claim> claims, User user)
    {
        var userGroupClaimsToAdd = user.UserGroups
            .Select(userGroup => new Claim(AuthConstants.FeatureGroupClaim, userGroup.Name))
            .ToList();

        claims.AddRange(userGroupClaimsToAdd);
    }
}