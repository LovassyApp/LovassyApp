using WebApi.Core.Backboard.Services;
using WebApi.Core.Cryptography.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Jobs;

public class UpdateGradesJob
{
    private readonly IServiceProvider _serviceProvider;

    public UpdateGradesJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Run(User user, string masterKey)
    {
        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var encryptionManager = scope.ServiceProvider.GetRequiredService<EncryptionManager>();
        var hashManager = scope.ServiceProvider.GetRequiredService<HashManager>();
        var backboardAdapter = scope.ServiceProvider.GetRequiredService<BackboardAdapter>();

        context.Attach(user); // We have to attach the user to the context, because it's not tracked yet in this scope

        encryptionManager.Init(masterKey);
        hashManager.Init(user);
        backboardAdapter.Init(user);

        backboardAdapter.TryUpdatingAsync().Wait();
    }
}