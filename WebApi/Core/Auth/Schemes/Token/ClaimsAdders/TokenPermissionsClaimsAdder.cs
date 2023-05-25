using System.Security.Claims;
using Microsoft.Extensions.Options;
using WebApi.Core.Auth.Interfaces;
using WebApi.Core.Auth.Services.Options;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Schemes.Token.ClaimsAdders;

public class TokenPermissionsClaimsAdder : IClaimsAdder<User>
{
    private readonly PermissionsOptions _permissionsOptions;

    public TokenPermissionsClaimsAdder(IOptions<PermissionsOptions> permissionsOptions)
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