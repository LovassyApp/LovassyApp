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
    public async Task<ActionResult<ViewUser.Response>> View([FromRoute] Guid id)
    {
        var user = await Mediator.Send(new ViewUser.Query { Id = id });

        return Ok(user);
    }

    [HttpPost]
    [EnableRateLimiting("Strict")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateUser([FromBody] CreateUser.RequestBody body, [FromQuery] string verifyUrl,
        [FromQuery] string verifyTokenQueryKey)
    {
        var response = await Mediator.Send(new CreateUser.Command
            { Body = body, VerifyUrl = verifyUrl, VerifyTokenQueryKey = verifyTokenQueryKey });

        return Created(nameof(View), response);
    }

    [HttpPatch("{id}")]
    [Permissions(typeof(UsersPermissions.UpdateUser))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUser.RequestBody body)
    {
        await Mediator.Send(new UpdateUser.Command { Id = id, Body = body });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Permissions(typeof(UsersPermissions.DeleteUser))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteUser.Command { Id = id });

        return NoContent();
    }
}