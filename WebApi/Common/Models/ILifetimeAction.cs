namespace WebApi.Common.Models;

/// <summary>
///     The base interface for all actions that should be executed on application start or stop.
/// </summary>
public interface ILifetimeAction
{
    public Task OnStartAsync(CancellationToken cancellationToken);
    public Task OnStopAsync(CancellationToken cancellationToken);
}