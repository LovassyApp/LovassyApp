using Hangfire;
using WebApi.Common.Models;
using WebApi.Core.Auth.Jobs;

namespace WebApi.Core.Auth.LifetimeActions;

public class AddScheduledAuthJobsAction : ILifetimeAction
{
    private readonly ILogger<AddScheduledAuthJobsAction> _logger;
    private readonly IRecurringJobManager _recurringJobManager;

    public AddScheduledAuthJobsAction(IRecurringJobManager recurringJobManager,
        ILogger<AddScheduledAuthJobsAction> logger)
    {
        _recurringJobManager = recurringJobManager;
        _logger = logger;
    }

    public async Task OnStartAsync(CancellationToken cancellationToken)
    {
        _recurringJobManager.AddOrUpdate<DeleteOldTokensJob>("DeleteOldTokens", j => j.Run(), Cron.Daily);
        _logger.LogInformation($"Added {nameof(DeleteOldTokensJob)} to recurring jobs");
    }

    public async Task OnStopAsync(CancellationToken cancellationToken)
    {
    }
}