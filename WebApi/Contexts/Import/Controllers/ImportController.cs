using Microsoft.AspNetCore.Mvc;
using WebApi.Contexts.Import.Filters.Action;

namespace WebApi.Contexts.Import.Controllers;

[ApiController]
[Route("/Api/[controller]")]
[Produces("application/json")]
public class ImportController : Controller
{
    [ServiceFilter(typeof(RequireImportKeyFilter))]
    [HttpGet]
    public async Task<ActionResult> Index()
    {
        return Ok();
    }
}