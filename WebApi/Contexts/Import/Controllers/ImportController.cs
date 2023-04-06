using Microsoft.AspNetCore.Mvc;
using WebApi.Contexts.Import.Filters.Action;

namespace WebApi.Contexts.Import.Controllers;

[ApiController]
[Route("/Api/[controller]")]
[Produces("application/json")]
[ServiceFilter(typeof(RequireImportKeyFilter))]
public class ImportController : Controller
{
    [HttpGet]
    public async Task<ActionResult> Index()
    {
        return Ok();
    }
}