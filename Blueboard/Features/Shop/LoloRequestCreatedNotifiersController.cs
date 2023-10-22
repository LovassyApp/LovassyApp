using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.Shop.Commands;
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
public class LoloRequestCreatedNotifiersController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(ShopPermissions.IndexLoloRequestCreatedNotifiers))]
    [EndpointSummary("Get a list of all emails to notify when a lolo request is created")]
    public async Task<ActionResult<IEnumerable<IndexLoloRequestCreatedNotifiers.Response>>> Index(
        [FromQuery] SieveModel sieveModel)
    {
        var notifiers = await Mediator.Send(new IndexLoloRequestCreatedNotifiers.Query
        {
            SieveModel = sieveModel
        });

        return Ok(notifiers);
    }

    [HttpPut]
    [Permissions(typeof(ShopPermissions.UpdateLoloRequestCreatedNotifiers))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Update the list of emails to notify when a lolo request is created")]
    public async Task<ActionResult> UpdateCreatedNotifiers(
        [FromBody] UpdateLoloRequestCreatedNotifiers.RequestBody body)
    {
        await Mediator.Send(new UpdateLoloRequestCreatedNotifiers.Command { Body = body });

        return NoContent();
    }
}