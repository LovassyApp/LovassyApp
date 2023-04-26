using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Models;
using WebApi.Features.Status.Queries;

namespace WebApi.Features.Status;

public class StatusController : ApiControllerBase
{
    [HttpGet("Version")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status418ImATeapot)]
    public async Task<ActionResult<ViewVersionResponse>> ViewVersion([FromQuery] ViewVersionBody body)
    {
        var response = await Mediator.Send(new ViewVersionQuery { Body = body });

        if (!body.SendOk)
            HttpContext.Response.StatusCode = StatusCodes.Status418ImATeapot;

        return body.SendOk ? Ok(response) : Json(response);
    }

    [HttpGet("ServiceStatus")]
    public async Task<ActionResult<ViewServiceStatusResponse>> ViewServiceStatus()
    {
        var response = await Mediator.Send(new ViewServiceStatusQuery());

        return Ok(response);
    }
}