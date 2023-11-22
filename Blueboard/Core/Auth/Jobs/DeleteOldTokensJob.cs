using Blueboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Blueboard.Core.Auth.Jobs;

/// <summary>
///     The scheduled job that deletes old personal access tokens.
/// </summary>
public class DeleteOldTokensJob(ApplicationDbContext dbContext, ILogger<DeleteOldTokensJob> logger)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var tokens =
            dbContext.PersonalAccessTokens.Where(x =>
                x.LastUsedAt != null && x.LastUsedAt < DateTime.Now.AddDays(-7).ToUniversalTime());
        var count = await tokens.CountAsync();
        dbContext.PersonalAccessTokens.RemoveRange(tokens);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Deleted {Count} old tokens", count);
    }
}