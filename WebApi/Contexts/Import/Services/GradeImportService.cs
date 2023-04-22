using Mapster;
using WebApi.Contexts.Import.Models;
using WebApi.Persistence;
using WebApi.Persistence.Entities;

namespace WebApi.Contexts.Import.Services;

public class GradeImportService
{
    private readonly ApplicationDbContext _context;

    public GradeImportService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Creates a <c>GradeImport</c> and sets the <c>ImportAvailable</c> flag on the user to true.
    /// </summary>
    /// <param name="data">The contents of the <c>GradeImport</c>.</param>
    /// <param name="user">The user, to which the <c>GradeImport</c> belongs to.</param>
    public async Task ImportGradesAsync(ImportGradesDto data, User user)
    {
        var gradeImport = data.Adapt<GradeImport>();
        gradeImport.UserId = user.Id;
        _context.Add(gradeImport);

        user.ImportAvailable = true;

        await _context.SaveChangesAsync();
    }
}