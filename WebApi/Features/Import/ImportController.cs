using Helpers.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;
using WebApi.Core.Auth;
using WebApi.Features.Import.Commands;
using WebApi.Features.Import.Queries;

namespace WebApi.Features.Import;

[Authorize(AuthenticationSchemes = AuthConstants.ImportKeyScheme)]
[FeatureGate("Import")]
public class ImportController : ApiControllerBase
{
    [HttpGet("Users")]
    public async Task<ActionResult<IEnumerable<IndexUsers.Response>>> IndexUsers([FromQuery] SieveModel sieveModel)
    {
        var users = await Mediator.Send(new IndexUsers.Query
        {
            SieveModel = sieveModel
        });

        return Ok(users);
    }

    [HttpPost("Grades/{userId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> ImportGrades([FromRoute] Guid userId, [FromBody] ImportGrades.RequestBody body)
    {
        await Mediator.Send(new ImportGrades.Command
        {
            UserId = userId,
            Body = body
        });

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("ResetKeyPassword")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateResetKeyPassword([FromBody] UpdateResetKeyPassword.RequestBody body)
    {
        await Mediator.Send(new UpdateResetKeyPassword.Command
        {
            Body = body
        });

        return NoContent();
    }
}