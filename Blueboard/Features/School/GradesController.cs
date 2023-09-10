using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.School.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;

namespace Blueboard.Features.School;

[Authorize]
[EmailVerified]
[FeatureGate("School")]
public class GradesController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(SchoolPermissions.IndexGrades))]
    [EndpointSummary("Get a list of the current user's grades")]
    public async Task<ActionResult<IEnumerable<IndexGrades.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexGrades.Query
        {
            SieveModel = sieveModel
        });

        return Ok(response);
    }
}