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

    public async Task ImportGradesAsync(ImportGradesDto data, User user)
    {
        var gradeImport = data.Adapt<GradeImport>();
        gradeImport.UserId = user.Id;
        _context.Add(gradeImport);

        user.ImportAvailable = true;

        await _context.SaveChangesAsync();
    }
}