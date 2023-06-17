using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.Shop.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;

namespace Blueboard.Features.Shop;

[Authorize]
[EmailVerified]
[FeatureGate("Shop")]
public class OwnedItemsController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(ShopPermissions.IndexOwnedItems))]
    public async Task<ActionResult<IEnumerable<IndexOwnedItems.Response>>> Index([FromQuery] SieveModel sieveModel,
        [FromQuery] string? Search)
    {
        var response = await Mediator.Send(new IndexOwnedItems.Query
        {
            SieveModel = sieveModel,
            Search = Search
        });

        return Ok(response);
    }

    [HttpGet("Own")]
    [Permissions(typeof(ShopPermissions.IndexOwnOwnedItems))]
    public async Task<ActionResult<IEnumerable<IndexOwnOwnedItems.Response>>> IndexOwn(
        [FromQuery] SieveModel sieveModel,
        [FromQuery] string? Search)
    {
        var response = await Mediator.Send(new IndexOwnOwnedItems.Query
        {
            SieveModel = sieveModel,
            Search = Search
        });

        return Ok(response);
    }
}