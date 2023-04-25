using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Models;
using WebApi.Features.Import.Commands;
using WebApi.Features.Import.Queries;

namespace WebApi.Features.Import;

//TODO: AUTHORIZATION!!!

public class ImportKeyController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndexImportKeyResponse>>> Index()
    {
        var importKeys = await Mediator.Send(new IndexImportKeysQuery());

        return Ok(importKeys);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ViewImportKeyResponse>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewImportKeyQuery { Id = id });

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ViewImportKeyResponse>> Create([FromBody] CreateImportKeyBody body)
    {
        var response = await Mediator.Send(new CreateImportKeyCommand
        {
            Body = body
        });

        return Created(nameof(View), response);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateImportKeyBody request)
    {
        await Mediator.Send(new UpdateImportKeyCommand
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
        await Mediator.Send(new DeleteImportKeyCommand
        {
            Id = id
        });

        return NoContent();
    }
}