using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.Shop.Commands;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blueboard.Features.Shop;

[Authorize]
[EmailVerified]
public class ProductsController : ApiControllerBase
{
    [HttpPost]
    [Permissions(typeof(ShopPermissions.CreateProduct))]
    public async Task<ActionResult<CreateProduct.Response>> Create([FromBody] CreateProduct.RequestBody body)
    {
        var response = await Mediator.Send(new CreateProduct.Command
        {
            Body = body
        });

        return Ok(response);
    }
}