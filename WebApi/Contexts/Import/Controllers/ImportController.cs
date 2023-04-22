using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts.Import.Models;
using WebApi.Contexts.Import.Services;
using WebApi.Helpers.Auth;
using WebApi.Helpers.Cryptography.Services;
using WebApi.Persistence;

namespace WebApi.Contexts.Import.Controllers;

[ApiController]
[Route("/Api/[controller]")]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = AuthConstants.ImportKeyScheme)]
public class ImportController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly GradeImportService _gradeImportService;
    private readonly ResetService _resetService;

    public ImportController(GradeImportService gradeImportService, ApplicationDbContext context,
        ResetService resetService)
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

        await _gradeImportService.ImportGradesAsync(request.Adapt<ImportGradesDto>(), user);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPatch("ResetKeyPassword")]
    public async Task<ActionResult> ImportResetKeyPassword([FromBody] ImportResetKeyPasswordRequest request)
    {
        _resetService.SetResetKeyPassword(request.ResetKeyPassword);

        return Ok();
    }
}