using Helpers.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using WebApi.Core.Auth.Permissions;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Core.Auth.Policies.Permissions;
using WebApi.Features.School.Queries;

namespace WebApi.Features.School;

[Authorize]
[EmailVerified]
public class GradesController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(SchoolPermissions.ViewGrades))]
    public async Task<ActionResult<IEnumerable<IndexGrades.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexGrades.Query
        {
            SieveModel = sieveModel
        });

        return Ok(response);
    }
}