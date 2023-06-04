using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.Import.Commands;
using Blueboard.Features.Import.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;

namespace Blueboard.Features.Import;

[Authorize]
[EmailVerified]
[FeatureGate("Import")]
public class ImportKeysController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(ImportPermissions.IndexImportKeys))]
    public async Task<ActionResult<IEnumerable<IndexImportKeys.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var importKeys = await Mediator.Send(new IndexImportKeys.Query
        {
            SieveModel = sieveModel
        });

        return Ok(importKeys);
    }

    [HttpGet("{id}")]
    [Permissions(typeof(ImportPermissions.ViewImportKey))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ViewImportKey.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewImportKey.Query { Id = id });

        return Ok(response);
    }

    [HttpPost]
    [Permissions(typeof(ImportPermissions.CreateImportKey))]
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
    [Permissions(typeof(ImportPermissions.UpdateImportKey))]
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
    [Permissions(typeof(ImportPermissions.DeleteImportKey))]
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