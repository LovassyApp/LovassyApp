using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using WebApi.Common.Models;
using WebApi.Features.Shop.Queries;

namespace WebApi.Features.Shop;

[Authorize]
public class LolosController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IndexLolos.Response>> Index([FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexLolos.Query { SieveModel = sieveModel });
        return Ok(response);
    }
}