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
public class QRCodesController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(ShopPermissions.IndexQRCodes))]
    public async Task<ActionResult<IndexQRCodes.Response>> Index([FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexQRCodes.Query { SieveModel = sieveModel });

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Permissions(typeof(ShopPermissions.ViewQRCode))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewQRCode.Query { Id = id });

        return Ok(response);
    }

    [HttpPost]
    [Permissions(typeof(ShopPermissions.CreateQRCode))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateQRCode.Response>> Create([FromBody] CreateQRCode.RequestBody body)
    {
        var response = await Mediator.Send(new CreateQRCode.Command
        {
            Body = body
        });

        return Created(nameof(View), response);
    }

    [HttpPatch("{id}")]
    [Permissions(typeof(ShopPermissions.UpdateQRCode))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateQRCode.RequestBody body)
    {
        await Mediator.Send(new UpdateQRCode.Command
        {
            Id = id,
            Body = body
        });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Permissions(typeof(ShopPermissions.DeleteQRCode))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteQRCode.Commnad { Id = id });

        return NoContent();
    }
}