using System.Security.Claims;
using WebApi.Core.Auth.Interfaces;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Schemes.Token.ClaimsAdders;

public class TokenPermissionsClaimsAdder : IClaimsAdder<User>
{
    public async Task AddClaimsAsync(List<Claim> claims, User user)
    {
        var claimsToAdd = user.UserGroups
            .SelectMany(userGroup => userGroup.Permissions)
            .Select(permission => new Claim(AuthConstants.PermissionClaim, permission))
            .ToList();

        claims.AddRange(claimsToAdd);
    }
}