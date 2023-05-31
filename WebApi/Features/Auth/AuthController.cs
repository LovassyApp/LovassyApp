using Helpers.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Auth;
using WebApi.Core.Auth.Permissions;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Core.Auth.Policies.Permissions;
using WebApi.Features.Auth.Commands;
using WebApi.Features.Auth.Queries;

namespace WebApi.Features.Auth;

public class AuthController : ApiControllerBase
{
    [HttpPost("Login")]
    public async Task<ActionResult<Login.Response>> Login([FromBody] Login.RequestBody body)
    {
        var response = await Mediator.Send(new Login.Command { Body = body });

        if (response.RefreshToken != null)
            Response.Cookies.Append(AuthConstants.RefreshCookieKey, response.RefreshToken, new CookieOptions
            {
                Expires = response.RefreshTokenExpiration
            });

        return Ok(response);
    }

    [HttpPost("Refresh")]
    public async Task<ActionResult<Refresh.Response>> Refresh([FromQuery] string? token)
    {
        var response = await Mediator.Send(new Refresh.Command
            { RefreshToken = token ?? Request.Cookies[AuthConstants.RefreshCookieKey] });

        Response.Cookies.Append(AuthConstants.RefreshCookieKey, response.RefreshToken, new CookieOptions
        {
            Expires = response.RefreshTokenExpiration
        });

        return Ok(response);
    }

    [Authorize]
    [Permissions(typeof(AuthPermissions.ViewControl))]
    [HttpGet("Control")]
    public async Task<ActionResult<ViewControl.Response>> ViewControl()
    {
        var response = await Mediator.Send(new ViewControl.Query());

        return Ok(response);
    }

    [Authorize]
    [HttpDelete("Logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Logout()
    {
        await Mediator.Send(new Logout.Command());

        Response.Cookies.Delete(AuthConstants.RefreshCookieKey);

        return NoContent();
    }

    [HttpPost("VerifyEmail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> VerifyEmail([FromQuery] string verifyToken)
    {
        await Mediator.Send(new VerifyEmail.Command { VerifyToken = verifyToken });

        return NoContent();
    }

    [Authorize]
    [EmailVerified(false)]
    [HttpPost("ResendVerifyEmail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ResendVerifyEmail([FromQuery] string verifyUrl,
        [FromQuery] string verifyTokenQueryKey)

    {
        await Mediator.Send(new ResendVerifyEmail.Command
        {
            VerifyUrl = verifyUrl,
            VerifyTokenQueryKey = verifyTokenQueryKey
        });

        return NoContent();
    }

    [HttpPost("SendPasswordReset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SendPasswordReset([FromQuery] string passwordResetUrl,
        [FromQuery] string passwordResetTokenQueryKey, [FromBody] SendPasswordReset.RequestBody body)
    {
        await Mediator.Send(new SendPasswordReset.Command
        {
            PasswordResetUrl = passwordResetUrl,
            PasswordResetTokenQueryKey = passwordResetTokenQueryKey,
            Body = body
        });

        return NoContent();
    }
}