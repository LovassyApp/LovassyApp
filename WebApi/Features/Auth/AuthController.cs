using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Models;
using WebApi.Features.Auth.Commands;

namespace WebApi.Features.Auth;

public class AuthController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Login.Response>> Login([FromBody] Login.RequestBody body)
    {
        var response = await Mediator.Send(new Login.Command { Body = body });

        return Ok(response);
    }
}