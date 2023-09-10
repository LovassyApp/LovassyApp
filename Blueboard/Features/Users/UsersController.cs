using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.Users.Commands;
using Blueboard.Features.Users.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;

namespace Blueboard.Features.Users;

[Authorize]
[EmailVerified]
[FeatureGate("Users")]
public class UsersController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(UsersPermissions.IndexUsers))]
    [EndpointSummary("Get a list of all users")]
    public async Task<ActionResult<IEnumerable<IndexUsers.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var users = await Mediator.Send(new IndexUsers.Query
        {
            SieveModel = sieveModel
        });

        return Ok(users);
    }

    [HttpGet("{id}")]
    [Permissions(typeof(UsersPermissions.ViewUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Get information about a user")]
    public async Task<ActionResult<ViewUser.Response>> View([FromRoute] Guid id)
    {
        var user = await Mediator.Send(new ViewUser.Query { Id = id });

        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost]
    [EnableRateLimiting("Strict")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EndpointSummary("Create a new user")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUser.RequestBody body, [FromQuery] string verifyUrl,
        [FromQuery] string verifyTokenQueryKey)
    {
        var response = await Mediator.Send(new CreateUser.Command
            { Body = body, VerifyUrl = verifyUrl, VerifyTokenQueryKey = verifyTokenQueryKey });

        return CreatedAtAction(nameof(View), new { id = response.Id }, response);
    }

    [HttpPatch("{id}")]
    [Permissions(typeof(UsersPermissions.UpdateUser))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Update a user")]
    public async Task<ActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUser.RequestBody body)
    {
        await Mediator.Send(new UpdateUser.Command { Id = id, Body = body });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Permissions(typeof(UsersPermissions.DeleteUser))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Delete a user")]
    public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteUser.Command { Id = id });

        return NoContent();
    }

    [HttpPost("Kick/{id}")]
    [Permissions(typeof(UsersPermissions.KickUser))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Delete all active sessions of a user")]
    public async Task<ActionResult> KickUser([FromRoute] Guid id)
    {
        await Mediator.Send(new KickUser.Command { Id = id });

        return NoContent();
    }

    [HttpPost("Kick/All")]
    [Permissions(typeof(UsersPermissions.KickAllUsers))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Delete all active sessions of all users")]
    public async Task<ActionResult> KickAllUsers()
    {
        await Mediator.Send(new KickAllUsers.Command());

        return NoContent();
    }
}