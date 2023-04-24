using Hangfire;
using WebApi.Core.Auth.Jobs;

namespace WebApi.Core.Auth.Services.Hosted;

public class ScheduledAuthJobsService : IHostedService
{
    private readonly ILogger<ScheduledAuthJobsService> _logger;
    private readonly IRecurringJobManager _recurringJobManager;

    public ScheduledAuthJobsService(IRecurringJobManager recurringJobManager, ILogger<ScheduledAuthJobsService> logger)
    {
        _recurringJobManager = recurringJobManager;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _recurringJobManager.AddOrUpdate<DeleteOldTokensJob>("DeleteOldTokens", j => j.Run(), Cron.Daily);
        _logger.LogInformation($"Added {nameof(DeleteOldTokensJob)} to recurring jobs");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}