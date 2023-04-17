using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts.Import.Filters.Action;
using WebApi.Contexts.Import.Models;
using WebApi.Contexts.Import.Services;
using WebApi.Helpers.Cryptography.Services;
using WebApi.Persistence;

namespace WebApi.Contexts.Import.Controllers;

[ApiController]
[Route("/Api/[controller]")]
[Produces("application/json")]
[ServiceFilter(typeof(RequireImportKeyFilter))]
public class ImportController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IGradeImportService _gradeImportService;
    private readonly IResetService _resetService;

    public ImportController(IGradeImportService gradeImportService, ApplicationDbContext context,
        IResetService resetService)
    {
        _gradeImportService = gradeImportService;
        _context = context;
        _resetService = resetService;
    }

    [HttpGet("Users")]
    public async Task<ActionResult<IEnumerable<IndexUsersResponse>>> IndexUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users.Adapt<IEnumerable<IndexUsersResponse>>());
    }

    [HttpPost("Grades/{userId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> ImportGrades([FromRoute] Guid userId, [FromBody] ImportGradesRequest request)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            return NotFound();

        await _gradeImportService.ImportGrades(request.Adapt<ImportGradesDto>(), user);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPatch("ResetKeyPassword")]
    public async Task<ActionResult> ImportResetKeyPassword([FromBody] ImportResetKeyPasswordRequest request)
    {
        _resetService.SetResetKeyPassword(request.ResetKeyPassword);

        return Ok();
    }
}