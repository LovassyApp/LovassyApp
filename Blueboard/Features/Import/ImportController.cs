using Blueboard.Core.Auth;
using Blueboard.Features.Import.Commands;
using Blueboard.Features.Import.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;

namespace Blueboard.Features.Import;

[Authorize(AuthenticationSchemes = AuthConstants.ImportKeyScheme)]
[FeatureGate("Import")]
public class ImportController : ApiControllerBase
{
    [HttpGet("Users")]
    [EndpointSummary("Get a list of all users for grade importing")]
    public async Task<ActionResult<IEnumerable<IndexUsers.Response>>> IndexUsers([FromQuery] SieveModel sieveModel)
    {
        var users = await Mediator.Send(new IndexUsers.Query
        {
            SieveModel = sieveModel
        });

        return Ok(users);
    }

    [EnableRateLimiting("Relaxed")]
    [HttpPost("Grades/{userId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EndpointSummary("Import grades for a user")]
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
    [EndpointSummary("Set the reset key password")]
    public async Task<ActionResult> UpdateResetKeyPassword([FromBody] UpdateResetKeyPassword.RequestBody body)
    {
        await Mediator.Send(new UpdateResetKeyPassword.Command
        {
            Body = body
        });

        return NoContent();
    }
}