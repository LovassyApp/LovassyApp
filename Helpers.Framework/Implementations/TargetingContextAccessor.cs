using System.Security.Claims;
using Helpers.Framework.Services.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement.FeatureFilters;

namespace Helpers.Framework.Implementations;

public class TargetingContextAccessor : ITargetingContextAccessor
{
    private const string TargetingContextLookup = "TargetingContextAccessor.TargetingContext";
    private readonly FeatureFlagOptions _featureFlagOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TargetingContextAccessor(IHttpContextAccessor httpContextAccessor,
        IOptions<FeatureFlagOptions> featureFlagOptions)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _featureFlagOptions = featureFlagOptions.Value;
    }

    public async ValueTask<TargetingContext> GetContextAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        if (httpContext.Items.TryGetValue(TargetingContextLookup, out var value)) return (TargetingContext)value!;

        var groups = new List<string>();
        if (httpContext.User.Identity!.IsAuthenticated)
            groups.AddRange(httpContext.User.FindAll(_featureFlagOptions.GroupClaim).Select(c => c.Value));

        var targetingContext = new TargetingContext
        {
            UserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
            Groups = groups
        };

        httpContext.Items[TargetingContextLookup] = targetingContext;
        return targetingContext;
    }
}