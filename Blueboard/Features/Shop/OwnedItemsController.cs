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
public class OwnedItemsController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(ShopPermissions.IndexOwnedItems))]
    [EndpointSummary("Get a list of all owned items")]
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
    [EndpointSummary("Get a list of the current user's owned items")]
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

    [HttpGet("{id}")]
    [Permissions(typeof(ShopPermissions.ViewOwnedItem), typeof(ShopPermissions.ViewOwnOwnedItem))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointSummary("Get information about an owned item")]
    public async Task<ActionResult<ViewOwnedItem.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewOwnedItem.Query { Id = id });

        return Ok(response);
    }

    [HttpPost]
    [Permissions(typeof(ShopPermissions.CreateOwnedItem))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EndpointSummary("Create a new owned item")]
    public async Task<ActionResult<CreateOwnedItem.Response>> Create([FromBody] CreateOwnedItem.RequestBody body)
    {
        var response = await Mediator.Send(new CreateOwnedItem.Command { Body = body });

        return CreatedAtAction(nameof(View), new { id = response.Id }, response);
    }

    [HttpPatch("{id}")]
    [Permissions(typeof(ShopPermissions.UpdateOwnedItem))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Update an owned item")]
    public async Task<ActionResult> Update([FromRoute] int id,
        [FromBody] UpdateOwnedItem.RequestBody body)
    {
        await Mediator.Send(new UpdateOwnedItem.Command { Id = id, Body = body });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Permissions(typeof(ShopPermissions.DeleteOwnedItem), typeof(ShopPermissions.DeleteOwnOwnedItem))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Delete an owned item")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteOwnedItem.Command { Id = id });

        return NoContent();
    }

    [HttpPost("{id}/Use")]
    [Permissions(typeof(ShopPermissions.UseOwnOwnedItem))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Use an owned item")]
    public async Task<ActionResult> Use([FromRoute] int id,
        [FromBody] UseOwnedItem.RequestBody body)
    {
        await Mediator.Send(new UseOwnedItem.Command { Id = id, Body = body });

        return NoContent();
    }
}