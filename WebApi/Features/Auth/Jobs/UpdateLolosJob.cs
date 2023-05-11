using WebApi.Core.Cryptography.Services;
using WebApi.Core.Lolo.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Jobs;

public class UpdateLolosJob
{
    private readonly IServiceProvider _serviceProvider;

    public UpdateLolosJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Run(User user, string masterKey)
    {
        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var encryptionManager = scope.ServiceProvider.GetRequiredService<EncryptionManager>();
        var hashManager = scope.ServiceProvider.GetRequiredService<HashManager>();
        var loloManager = scope.ServiceProvider.GetRequiredService<LoloManager>();

        context.Attach(user); // We have to attach the user to the context, because it's not tracked yet in this scope

        encryptionManager.Init(masterKey);
        hashManager.Init(user);
        loloManager.Init(user);

        loloManager.GenerateAsync().Wait();

        //TODO: Send out a notification through websockets informing the user that their lolos have finished updating (we should refetch them afterwards)
    }
}