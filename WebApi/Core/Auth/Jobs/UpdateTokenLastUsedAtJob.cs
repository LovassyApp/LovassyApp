using WebApi.Persistence;
using WebApi.Persistence.Entities;

namespace WebApi.Core.Auth.Jobs;

public class UpdateTokenLastUsedAtJob
{
    private readonly ApplicationDbContext _context;

    public UpdateTokenLastUsedAtJob(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Run(PersonalAccessToken token)
    {
        token.LastUsedAt = DateTime.Now;
        _context.SaveChanges(); // Can't use async here, because Hangfire doesn't support it
    }
}