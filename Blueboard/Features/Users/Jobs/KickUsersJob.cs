using System.Text.Json;
using Blueboard.Core.Auth.Services;
using Quartz;

namespace Blueboard.Features.Users.Jobs;

public class KickUsersJob : IJob
{
    private readonly ILogger<KickUsersJob> _logger;
    private readonly SessionService _sessionService;

    public KickUsersJob(SessionService sessionService, ILogger<KickUsersJob> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var tokens =
            JsonSerializer.Deserialize<IEnumerable<string>>((context.MergedJobDataMap.Get("tokensJson") as string)!)!
                .ToArray();

        foreach (var token in tokens) _sessionService.StopSession(token);

        _logger.LogInformation("Stopped sessions belonging to {Count} tokens, due to users being kicked",
            tokens.Count());
    }
}