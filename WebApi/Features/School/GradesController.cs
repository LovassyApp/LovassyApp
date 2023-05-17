using Helpers.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Features.School.Queries;

namespace WebApi.Features.School;

[Authorize]
[EmailVerified]
public class GradesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndexGrades.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexGrades.Query
        {
            SieveModel = sieveModel
        });

        return Ok(response);
    }
}