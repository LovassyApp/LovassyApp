using Helpers.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Auth.Permissions;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Core.Auth.Policies.Permissions;
using WebApi.Features.Shop.Commands;

namespace WebApi.Features.Shop;

[Authorize]
[EmailVerified]
public class QRCodesController : ApiControllerBase
{
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