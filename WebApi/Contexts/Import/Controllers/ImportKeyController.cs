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
    private readonly ImportKeyService _importKeyService;

    public ImportKeyController(ImportKeyService importKeyService)
    {
        _importKeyService = importKeyService;
    }

    [HttpGet]
    public async Task<IEnumerable<IndexImportKeyResponse>> Index()
    {
        var importKeys = await _importKeyService.IndexImportKeysAsync();
        return importKeys.Adapt<IEnumerable<IndexImportKeyResponse>>();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    public async Task<ActionResult<ViewImportKeyResponse>> Create([FromBody] CreateImportKeyRequest request)
    {
        var key = await _importKeyService.CreateImportKeyAsync(request.Adapt<CreateImportKeyDto>());

        var response = request.Adapt<ViewImportKeyResponse>();
        response.Key = key;

        return Created(nameof(View), response);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateImportKeyRequest request)
    {
        var (importKey, _) = await _importKeyService.GetImportKeyAsync(id);

        if (importKey == null)
            return NotFound();

        await _importKeyService.UpdateImportKeyAsync(importKey, request.Adapt<UpdateImportKeyDto>());

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var (importKey, _) = await _importKeyService.GetImportKeyAsync(id);

        if (importKey == null)
            return NotFound();

        await _importKeyService.DeleteImportKeyAsync(importKey);

        return NoContent();
    }
}