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
public class PermissionsController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(AuthPermissions.IndexPermissions))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndexPermissions.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexPermissions.Query { SieveModel = sieveModel });

        return Ok(response);
    }
}