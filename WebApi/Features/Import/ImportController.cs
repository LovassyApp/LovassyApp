using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Models;
using WebApi.Core.Auth;
using WebApi.Features.Import.Commands;
using WebApi.Features.Import.Queries;

namespace WebApi.Features.Import;

[Authorize(AuthenticationSchemes = AuthConstants.ImportKeyScheme)]
public class ImportController : ApiControllerBase
{
    [HttpGet("Users")]
    public async Task<ActionResult<IEnumerable<IndexUsersResponse>>> IndexUsers()
    {
        var users = await Mediator.Send(new IndexUsersQuery());

        return Ok(users);
    }

    [HttpPost("Grades/{userId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> ImportGrades([FromRoute] Guid userId, [FromBody] ImportGradesBody body)
    {
        await Mediator.Send(new ImportGradesCommand
        {
            UserId = userId,
            Body = body
        });

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPatch("ResetKeyPassword")]
    public async Task<ActionResult> ImportResetKeyPassword([FromBody] ImportResetKeyPasswordBody body)
    {
        await Mediator.Send(new ImportResetKeyPasswordCommand
        {
            Body = body
        });

        return Ok();
    }
}