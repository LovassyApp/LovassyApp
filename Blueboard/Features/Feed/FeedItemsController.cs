using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.Feed.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;

namespace Blueboard.Features.Feed;

[Authorize]
[EmailVerified]
[FeatureGate("Feed")]
public class FeedItemsController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(FeedPermissions.IndexFeedItems))]
    [EndpointSummary("Get a list of all feed items")]
    public async Task<ActionResult<IEnumerable<IndexFeedItems.Response>>> IndexFeedItems(
        [FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexFeedItems.Query { SieveModel = sieveModel });
        return Ok(response);
    }
}