using Helpers.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;
using WebApi.Core.Auth.Permissions;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Core.Auth.Policies.Permissions;
using WebApi.Features.Shop.Queries;

namespace WebApi.Features.Shop;

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