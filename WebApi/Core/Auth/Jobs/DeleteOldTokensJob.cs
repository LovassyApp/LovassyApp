using Microsoft.EntityFrameworkCore;
using Quartz;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Core.Auth.Jobs;

/// <summary>
///     The scheduled job that deletes old personal access tokens.
/// </summary>
public class DeleteOldTokensJob : IJob
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DeleteOldTokensJob> _logger;

    public DeleteOldTokensJob(ApplicationDbContext context, ILogger<DeleteOldTokensJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var tokens =
            _context.PersonalAccessTokens.Where(x =>
                x.LastUsedAt != null && x.LastUsedAt < DateTime.Now.AddDays(-7).ToUniversalTime());
        var count = await tokens.CountAsync();
        _context.PersonalAccessTokens.RemoveRange(tokens);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted {Count} old tokens", count);
    }
}