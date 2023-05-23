using Helpers.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using WebApi.Core.Auth.Permissions;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Core.Auth.Policies.Permissions;
using WebApi.Features.Auth.Commands;
using WebApi.Features.Auth.Queries;

namespace WebApi.Features.Auth;

[Authorize]
[EmailVerified]
public class UserGroupsController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(AuthPermissions.IndexUserGroups))]
    public async Task<ActionResult<IEnumerable<IndexUserGroups.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var userGroups = await Mediator.Send(new IndexUserGroups.Query
        {
            SieveModel = sieveModel
        });

        return Ok(userGroups);
    }

    [HttpGet("{id}")]
    [Permissions(typeof(AuthPermissions.ViewUserGroup))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ViewUserGroup.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewUserGroup.Query { Id = id });

        return Ok(response);
    }

    [HttpPost]
    [Permissions(typeof(AuthPermissions.CreateUserGroup))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateUserGroup.Response>> Create([FromBody] CreateUserGroup.RequestBody body)
    {
        var response = await Mediator.Send(new CreateUserGroup.Command
        {
            Body = body
        });

        return Created(nameof(View), response);
    }

    [HttpPatch("{id}")]
    [Permissions(typeof(AuthPermissions.UpdateUserGroup))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateUserGroup.RequestBody body)
    {
        await Mediator.Send(new UpdateUserGroup.Command { Id = id, Body = body });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Permissions(typeof(AuthPermissions.DeleteUserGroup))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteUserGroup.Command { Id = id });

        return NoContent();
    }
}