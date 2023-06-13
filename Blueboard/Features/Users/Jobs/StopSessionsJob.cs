using System.Text.Json;
using Blueboard.Core.Auth.Services;
using Quartz;

namespace Blueboard.Features.Users.Jobs;

public class StopSessionsJob : IJob
{
    private readonly ILogger<StopSessionsJob> _logger;
    private readonly SessionService _sessionService;

    public StopSessionsJob(SessionService sessionService, ILogger<StopSessionsJob> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var tokens =
            JsonSerializer.Deserialize<IEnumerable<string>>((context.MergedJobDataMap.Get("tokensJson") as string)!);

        foreach (var token in tokens!) _sessionService.StopSession(token);

        //TODO: Send out a notification to the user that they have been kicked

        _logger.LogInformation($"Stopped sessions belonging to {tokens.Count()} tokens, due to users being kicked");
    }
}