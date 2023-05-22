using Helpers.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using WebApi.Core.Auth.Permissions;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Core.Auth.Policies.Permissions;
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
}