using System.Security.Claims;
using Blueboard.Core.Auth.Interfaces;
using Blueboard.Core.Auth.Services.Options;
using Blueboard.Infrastructure.Persistence.Entities;
using Microsoft.Extensions.Options;

namespace Blueboard.Core.Auth.Schemes.Token.ClaimsAdders;

public class TokenPermissionsClaimsAdder : IClaimsAdder<User>
{
    private readonly PermissionsOptions _permissionsOptions;

    public TokenPermissionsClaimsAdder(IOptionsSnapshot<PermissionsOptions> permissionsOptions)
    {
        _permissionsOptions = permissionsOptions.Value;
    }

    public async Task AddClaimsAsync(List<Claim> claims, User user)
    {
        var permissionClaimsToAdd = user.UserGroups
            .SelectMany(userGroup => userGroup.Permissions)
            .Distinct().Select(permission => new Claim(AuthConstants.PermissionClaim, permission))
            .ToList();

        claims.AddRange(permissionClaimsToAdd);
        claims.Add(new Claim(AuthConstants.SuperUserClaim,
            _permissionsOptions.SuperUserEmails?.Contains(user.Email).ToString() ?? bool.FalseString));
    }
}