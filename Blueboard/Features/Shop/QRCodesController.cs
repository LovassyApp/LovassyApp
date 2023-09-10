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
    [EndpointSummary("Get a list of all QR codes")]
    public async Task<ActionResult<IEnumerable<IndexQRCodes.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexQRCodes.Query { SieveModel = sieveModel });

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Permissions(typeof(ShopPermissions.ViewQRCode))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointSummary("Get information about a QR code")]
    public async Task<ActionResult<ViewQRCode.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewQRCode.Query { Id = id });

        return Ok(response);
    }

    [HttpPost]
    [Permissions(typeof(ShopPermissions.CreateQRCode))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EndpointSummary("Create a new QR code")]
    public async Task<ActionResult<CreateQRCode.Response>> Create([FromBody] CreateQRCode.RequestBody body)
    {
        var response = await Mediator.Send(new CreateQRCode.Command
        {
            Body = body
        });

        return CreatedAtAction(nameof(View), new { id = response.Id }, response);
    }

    [HttpPatch("{id}")]
    [Permissions(typeof(ShopPermissions.UpdateQRCode))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Update a QR code")]
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
    [EndpointSummary("Delete a QR code")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteQRCode.Commnad { Id = id });

        return NoContent();
    }
}