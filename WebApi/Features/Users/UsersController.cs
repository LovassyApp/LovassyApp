using Helpers.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Sieve.Models;
using WebApi.Core.Auth.Permissions;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Core.Auth.Policies.Permissions;
using WebApi.Features.Users.Commands;
using WebApi.Features.Users.Queries;

namespace WebApi.Features.Users;

[Authorize]
[EmailVerified]
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