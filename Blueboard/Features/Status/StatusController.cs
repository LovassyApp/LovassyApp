using Blueboard.Features.Status.Commands;
using Blueboard.Features.Status.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace Blueboard.Features.Status;

public class StatusController : ApiControllerBase
{
    [HttpGet("Version")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status418ImATeapot)]
    public async Task<ActionResult<ViewVersion.Response>> ViewVersion([FromQuery] ViewVersion.RequestBody body)
    {
        var response = await Mediator.Send(new ViewVersion.Query { Body = body });

        if (!body.SendOk)
            HttpContext.Response.StatusCode = StatusCodes.Status418ImATeapot;

        return body.SendOk ? Ok(response) : Json(response);
    }

    [HttpGet("ServiceStatus")]
    public async Task<ActionResult<ViewServiceStatus.Response>> ViewServiceStatus()
    {
        var response = await Mediator.Send(new ViewServiceStatus.Query());

        return Ok(response);
    }

    [HttpPost("NotifyOnResetKeyPasswordSet")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> NotifyOnResetKeyPasswordSet([FromBody] NotifyOnResetKeyPasswordSet.RequestBody body)
    {
        await Mediator.Send(new NotifyOnResetKeyPasswordSet.Command
        {
            Body = body
        });

        return NoContent();
    }
}