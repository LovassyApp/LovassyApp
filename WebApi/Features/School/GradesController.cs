using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Models;
using WebApi.Features.School.Queries;

namespace WebApi.Features.School;

[Authorize]
public class GradesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndexGrades.Response>>> Index()
    {
        var response = await Mediator.Send(new IndexGrades.Query());

        return Ok(response);
    }
}