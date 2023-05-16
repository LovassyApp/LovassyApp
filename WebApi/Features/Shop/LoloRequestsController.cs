using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using WebApi.Common.Models;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Features.Shop.Commands;
using WebApi.Features.Shop.Queries;

namespace WebApi.Features.Shop;

[Authorize]
[EmailVerified]
public class LoloRequestsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndexLoloRequests.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var loloRequests = await Mediator.Send(new IndexLoloRequests.Query
        {
            SieveModel = sieveModel
        });

        return Ok(loloRequests);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ViewLoloRequest.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewLoloRequest.Query { Id = id });

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateLoloRequest.Response>> Create([FromBody] CreateLoloRequest.RequestBody body)
    {
        var response = await Mediator.Send(new CreateLoloRequest.Command
        {
            Body = body
        });

        return Created(nameof(View), response);
    }

    [HttpPatch("Overrule/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Overrule([FromRoute] int id, [FromBody] OverruleLoloRequest.RequestBody body)
    {
        await Mediator.Send(new OverruleLoloRequest.Command { Id = id, Body = body });

        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateLoloRequest.RequestBody body)
    {
        await Mediator.Send(new UpdateLoloRequest.Command { Id = id, Body = body });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteLoloRequest.Command { Id = id });

        return NoContent();
    }
}