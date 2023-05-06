using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using WebApi.Common.Models;
using WebApi.Core.Auth;
using WebApi.Features.Import.Commands;
using WebApi.Features.Import.Queries;

namespace WebApi.Features.Import;

[Authorize(AuthenticationSchemes = AuthConstants.ImportKeyScheme)]
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

    [HttpPatch("ResetKeyPassword")]
    public async Task<ActionResult> ImportResetKeyPassword([FromBody] UpdateResetKeyPassword.RequestBody body)
    {
        await Mediator.Send(new UpdateResetKeyPassword.Command
        {
            Body = body
        });

        return Ok();
    }
}