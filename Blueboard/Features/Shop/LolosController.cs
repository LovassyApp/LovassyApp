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
public class LolosController : ApiControllerBase
{
    [HttpGet("Own")]
    [Permissions(typeof(ShopPermissions.IndexOwnLolos))]
    public async Task<ActionResult<IndexOwnLolos.Response>> IndexOwn([FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexOwnLolos.Query { SieveModel = sieveModel });
        return Ok(response);
    }

    [HttpGet]
    [Permissions(typeof(ShopPermissions.IndexLolos))]
    public async Task<ActionResult<IndexLolos.Response>> Index([FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexLolos.Query { SieveModel = sieveModel });
        return Ok(response);
    }
}