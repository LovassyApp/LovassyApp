using System.Security.Claims;
using Helpers.Framework.Services.Options;
using Microsoft.Extensions.Options;
using WebApi.Core.Auth.Interfaces;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Schemes.Token.ClaimsAdders;

public class TokenFeatureGroupsClaimsAdder : IClaimsAdder<User>
{
    private readonly FeatureFlagOptions _featureFlagOptions;

    public TokenFeatureGroupsClaimsAdder(IOptions<FeatureFlagOptions> featureFlagOptions)
    {
        _featureFlagOptions = featureFlagOptions.Value;
    }

    public async Task AddClaimsAsync(List<Claim> claims, User user)
    {
        var userGroupClaimsToAdd = user.UserGroups
            .Select(userGroup => new Claim(_featureFlagOptions.FeatureGroupClaim, userGroup.Name))
            .ToList();

        claims.AddRange(userGroupClaimsToAdd);
    }
}