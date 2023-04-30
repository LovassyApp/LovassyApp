using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Models;
using WebApi.Features.Auth.Commands;

namespace WebApi.Features.Auth;

public class AuthController : ApiControllerBase
{
    [HttpPost("Login")]
    public async Task<ActionResult<Login.Response>> Login([FromBody] Login.RequestBody body)
    {
        var response = await Mediator.Send(new Login.Command { Body = body });

        if (response.RefreshToken != null)
            Response.Cookies.Append("BlueboardRefresh", response.RefreshToken, new CookieOptions
            {
                Expires = response.RefreshTokenExpiration
            });

        return Ok(response);
    }

    [HttpPost("Refresh")]
    public async Task<ActionResult<Refresh.Response>> Refresh([FromQuery] string? token)
    {
        var response = await Mediator.Send(new Refresh.Command
            { RefreshToken = token ?? Request.Cookies["BlueboardRefresh"] });

        Response.Cookies.Append("BlueboardRefresh", response.RefreshToken, new CookieOptions
        {
            Expires = response.RefreshTokenExpiration
        });

        return Ok(response);
    }
}