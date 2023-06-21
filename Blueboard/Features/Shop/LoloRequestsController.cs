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
public class LoloRequestsController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(ShopPermissions.IndexLoloRequests))]
    public async Task<ActionResult<IEnumerable<IndexLoloRequests.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var loloRequests = await Mediator.Send(new IndexLoloRequests.Query
        {
            SieveModel = sieveModel
        });

        return Ok(loloRequests);
    }

    [HttpGet("{id}")]
    [Permissions(typeof(ShopPermissions.ViewLoloRequest))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ViewLoloRequest.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewLoloRequest.Query { Id = id });

        return Ok(response);
    }

    [HttpPost]
    [Permissions(typeof(ShopPermissions.CreateLoloRequest))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateLoloRequest.Response>> Create([FromBody] CreateLoloRequest.RequestBody body)
    {
        var response = await Mediator.Send(new CreateLoloRequest.Command
        {
            Body = body
        });

        return CreatedAtAction(nameof(View), new { id = response.Id }, response);
    }

    [HttpPost("Overrule/{id}")]
    [Permissions(typeof(ShopPermissions.OverruleLoloRequest))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Overrule([FromRoute] int id, [FromBody] OverruleLoloRequest.RequestBody body)
    {
        await Mediator.Send(new OverruleLoloRequest.Command { Id = id, Body = body });

        return NoContent();
    }

    [HttpPatch("{id}")]
    [Permissions(typeof(ShopPermissions.UpdateOwnLoloRequest), typeof(ShopPermissions.UpdateLoloRequest))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateLoloRequest.RequestBody body)
    {
        await Mediator.Send(new UpdateLoloRequest.Command { Id = id, Body = body });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Permissions(typeof(ShopPermissions.DeleteOwnLoloRequest), typeof(ShopPermissions.DeleteLoloRequest))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteLoloRequest.Command { Id = id });

        return NoContent();
    }
}