using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.Shop.Commands;
using Blueboard.Features.Shop.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blueboard.Features.Shop;

[Authorize]
[EmailVerified]
public class ProductsController : ApiControllerBase
{
    [HttpGet("{id}")]
    [Permissions(typeof(ShopPermissions.ViewProduct), typeof(ShopPermissions.ViewStoreProduct))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ViewProduct.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewProduct.Query
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpPost]
    [Permissions(typeof(ShopPermissions.CreateProduct))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateProduct.Response>> Create([FromBody] CreateProduct.RequestBody body)
    {
        var response = await Mediator.Send(new CreateProduct.Command
        {
            Body = body
        });

        return Created(nameof(View), response);
    }
}