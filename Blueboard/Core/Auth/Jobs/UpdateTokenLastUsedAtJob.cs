using Blueboard.Core.Auth.EventHandlers;
using Blueboard.Infrastructure.Persistence;
using Quartz;

namespace Blueboard.Core.Auth.Jobs;

/// <summary>
///     The background job that updates the last used at property of a personal access token. Fired each time when a token
///     is used in <see cref="AccessTokenUsedEventHandler" />.
/// </summary>
public class UpdateTokenLastUsedAtJob(ApplicationDbContext dbContext) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var id = context.JobDetail.JobDataMap.GetInt("id");
        var lastUsedAt = context.JobDetail.JobDataMap.GetDateTime("lastUsedAt");

        var token = await dbContext.PersonalAccessTokens.FindAsync(id);
        if (token == null) return;
        token.LastUsedAt = lastUsedAt.ToUniversalTime();
        await dbContext.SaveChangesAsync();
    }
}