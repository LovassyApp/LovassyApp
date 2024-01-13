using Blueboard.Core.Auth.Services;
using Quartz;
using Shimmer.Core;

namespace Blueboard.Features.Users.Jobs;

public class KickUsersJob(SessionService sessionService, ILogger<KickUsersJob> logger)
    : ShimmerJob<KickUsersJob.Data>
{
    protected override async Task Process(Data data, IJobExecutionContext context)
    {
        foreach (var token in data.Tokens) sessionService.StopSession(token);

        logger.LogInformation("Stopped sessions belonging to {Count} tokens, due to users being kicked",
            data.Tokens.Count());
    }

    public class Data
    {
        public IEnumerable<string> Tokens { get; set; } = null!;
    }
}