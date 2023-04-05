using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contexts.Import.Models;
using WebApi.Contexts.Import.Services;

namespace WebApi.Contexts.Import.Controllers;

//TODO: AUTHORIZATION!!!

[ApiController]
[Route("/Api/[controller]")]
[Produces("application/json")]
public class ImportKeyController : Controller
{
    private readonly IImportKeyService _importKeyService;

    public ImportKeyController(IImportKeyService importKeyService)
    {
        _importKeyService = importKeyService;
    }

    [HttpGet("/{id}")]
    public async Task<ActionResult<ViewImportKeyResponse>> View([FromRoute] int id)
    {
        var (importKey, key) = await _importKeyService.GetImportKeyAsync(id);

        if (importKey == null)
            return NotFound();

        var response = importKey.Adapt<ViewImportKeyResponse>();
        response.Key = key!;

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateImportKeyResponse>> Create([FromBody] CreateImportKeyRequest request)
    {
        var key = await _importKeyService.CreateImportKeyAsync(request.Adapt<CreateImportKeyDto>());

        var response = request.Adapt<CreateImportKeyResponse>();
        response.Key = key;

        return Created(nameof(View), response);
    }
}