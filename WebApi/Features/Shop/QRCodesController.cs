using Helpers.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using WebApi.Core.Auth.Permissions;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Core.Auth.Policies.Permissions;
using WebApi.Features.Shop.Commands;
using WebApi.Features.Shop.Queries;

namespace WebApi.Features.Shop;

[Authorize]
[EmailVerified]
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
    public async Task<ActionResult> View([FromRoute] int id)
    {
        throw new NotImplementedException();
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
}