using System.Text.Json;
using Blueboard.Core.Auth.Services;
using Quartz;

namespace Blueboard.Features.Users.Jobs;

public class KickUsersJob(SessionService sessionService, ILogger<KickUsersJob> logger)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var tokens =
            JsonSerializer.Deserialize<IEnumerable<string>>((context.MergedJobDataMap.Get("tokensJson") as string)!)!
                .ToArray();

        foreach (var token in tokens) sessionService.StopSession(token);

        logger.LogInformation("Stopped sessions belonging to {Count} tokens, due to users being kicked",
            tokens.Count());
    }
}