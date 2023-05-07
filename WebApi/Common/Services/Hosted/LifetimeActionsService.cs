using WebApi.Common.Models;

namespace WebApi.Common.Services.Hosted;

/// <summary>
///     The hosted service that executes all lifetime actions on application start and stop.
/// </summary>
public class LifetimeActionsService : IHostedService
{
    private readonly IEnumerable<ILifetimeAction> _lifetimeActions;

    public LifetimeActionsService(IEnumerable<ILifetimeAction> lifetimeActions)
    {
        _lifetimeActions = lifetimeActions;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var lifetimeAction in _lifetimeActions)
            lifetimeAction.OnStartAsync(cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var lifetimeAction in _lifetimeActions)
            lifetimeAction.OnStopAsync(cancellationToken);

        return Task.CompletedTask;
    }
}