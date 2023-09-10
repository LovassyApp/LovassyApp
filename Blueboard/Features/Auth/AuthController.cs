using Blueboard.Core.Auth;
using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.Auth.Commands;
using Blueboard.Features.Auth.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Blueboard.Features.Auth;

public class AuthController : ApiControllerBase
{
    [HttpPost("Login")]
    [EnableRateLimiting("Strict")]
    [EndpointSummary("Create a new session for a user")]
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
    [EndpointSummary("Refresh a user's session")]
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
    [HttpGet("Control")]
    [Permissions(typeof(AuthPermissions.ViewControl))]
    [EndpointSummary("Get information about the current user")]
    public async Task<ActionResult<ViewControl.Response>> ViewControl()
    {
        var response = await Mediator.Send(new ViewControl.Query());

        return Ok(response);
    }

    [Authorize]
    [HttpDelete("Logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Delete the current user's session and refresh cookie")]
    public async Task<ActionResult> Logout()
    {
        await Mediator.Send(new Logout.Command());

        Response.Cookies.Delete(AuthConstants.RefreshCookieKey);

        return NoContent();
    }

    [HttpPost("VerifyEmail")]
    [EnableRateLimiting("Strict")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointSummary("Verify a user's email address")]
    public async Task<ActionResult> VerifyEmail([FromQuery] string verifyToken)
    {
        await Mediator.Send(new VerifyEmail.Command { VerifyToken = verifyToken });

        return NoContent();
    }

    [HttpPost("ResendVerifyEmail")]
    [EnableRateLimiting("Strict")]
    [Authorize]
    [EmailVerified(false)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Resend a user's email verification email")]
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
    [EnableRateLimiting("Strict")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Send a password reset email to a user")]
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

    [HttpPost("ResetPassword")]
    [EnableRateLimiting("Strict")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [EndpointSummary("Reset a user's password")]
    public async Task<ActionResult> ResetPassword([FromQuery] string passwordResetToken,
        [FromBody] ResetPassword.RequestBody body)
    {
        await Mediator.Send(new ResetPassword.Command
        {
            PasswordResetToken = passwordResetToken,
            Body = body
        });

        return NoContent();
    }
}