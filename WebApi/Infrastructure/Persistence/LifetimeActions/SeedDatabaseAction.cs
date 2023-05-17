using Helpers.Framework.Interfaces;

namespace WebApi.Infrastructure.Persistence.LifetimeActions;

public class SeedDatabaseAction : ILifetimeAction
{
    public async Task OnStartAsync(CancellationToken cancellationToken)
    {
        //TODO: Seed default warden groups
    }

    public async Task OnStopAsync(CancellationToken cancellationToken)
    {
    }
}