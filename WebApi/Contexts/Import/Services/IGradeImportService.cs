using WebApi.Contexts.Import.Models;
using WebApi.Persistence.Entities;

namespace WebApi.Contexts.Import.Services;

public interface IGradeImportService
{
    public Task ImportGrades(ImportGradesDto data, User user);
}