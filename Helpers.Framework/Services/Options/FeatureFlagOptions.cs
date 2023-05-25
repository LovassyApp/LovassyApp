using System.Security.Claims;

namespace Helpers.Framework.Services.Options;

public class FeatureFlagOptions
{
    public string FeatureUserClaim { get; set; } = ClaimTypes.Email;
    public string FeatureGroupClaim { get; set; } = "UserGroup";
}