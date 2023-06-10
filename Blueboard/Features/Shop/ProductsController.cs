using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.Shop.Commands;
using Blueboard.Features.Shop.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace Blueboard.Features.Shop;

[Authorize]
[EmailVerified]
public class ProductsController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(ShopPermissions.IndexProducts), typeof(ShopPermissions.IndexStoreProducts))]
    public async Task<ActionResult<IEnumerable<IndexProducts.Response>>> Index([FromQuery] SieveModel sieveModel,
        [FromQuery] string? Search)
    {
        var response = await Mediator.Send(new IndexProducts.Query
        {
            SieveModel = sieveModel,
            Search = Search
        });

        return Ok(response);
    }

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

    [HttpPatch("{id}")]
    [Permissions(typeof(ShopPermissions.UpdateProduct))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateProduct.RequestBody body)
    {
        await Mediator.Send(new UpdateProduct.Command
        {
            Id = id,
            Body = body
        });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Permissions(typeof(ShopPermissions.DeleteProduct))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteProduct.Command
        {
            Id = id
        });

        return NoContent();
    }
}