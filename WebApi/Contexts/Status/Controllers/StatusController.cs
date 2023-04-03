using Microsoft.AspNetCore.Mvc;
using WebApi.Contexts.Status.Models;
using WebApi.Contexts.Status.Services;

namespace WebApi.Contexts.Status.Controllers;

[ApiController]
[Route("/Api/[controller]")]
[Produces("application/json")]
public class StatusController : Controller
{
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpGet("Version")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status418ImATeapot)]
    public ActionResult<VersionResponse> GetVersion([FromQuery] VersionRequest request)
    {
        var response = new VersionResponse
        {
            WhoAmI = _statusService.WhoAmI,
            Version = _statusService.Version,
            DotNetVersion = _statusService.DotNetVersion,
            Contributors = _statusService.Contributors,
            Repository = _statusService.Repository,
            MOTD = request.SendMOTD ? _statusService.MOTDs[new Random().Next(0, _statusService.MOTDs.Count)] : null
        };

        if (!request.SendOk)
            HttpContext.Response.StatusCode = StatusCodes.Status418ImATeapot;

        return request.SendOk ? Ok(response) : Json(response);
    }

    [HttpGet("ServiceStatus")]
    public ActionResult<ServiceStatusResponse> GetServiceStatus()
    {
        var response = new ServiceStatusResponse
        {
            Ready = _statusService.IsReady(),
            ServiceStatus = _statusService.GetServiceStatus()
        };

        return Ok(response);
    }
}