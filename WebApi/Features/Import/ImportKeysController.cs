using Helpers.Framework;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using WebApi.Features.Import.Commands;
using WebApi.Features.Import.Queries;

namespace WebApi.Features.Import;

//TODO: AUTHORIZATION!!!

public class ImportKeysController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndexImportKeys.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var importKeys = await Mediator.Send(new IndexImportKeys.Query
        {
            SieveModel = sieveModel
        });

        return Ok(importKeys);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ViewImportKey.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewImportKey.Query { Id = id });

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateImportKey.Response>> Create([FromBody] CreateImportKey.RequestBody body)
    {
        var response = await Mediator.Send(new CreateImportKey.Command
        {
            Body = body
        });

        return Created(nameof(View), response);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateImportKey.RequestBody request)
    {
        await Mediator.Send(new UpdateImportKey.Command
        {
            Id = id,
            Body = request
        });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteImportKey.Command
        {
            Id = id
        });

        return NoContent();
    }
}