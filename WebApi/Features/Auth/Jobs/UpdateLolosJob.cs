using System.Text.Json;
using Quartz;
using WebApi.Core.Cryptography.Services;
using WebApi.Core.Lolo.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Jobs;

public class UpdateLolosJob : IJob
{
    private readonly ApplicationDbContext _context;
    private readonly EncryptionManager _encryptionManager;
    private readonly HashManager _hashManager;
    private readonly LoloManager _loloManager;

    public UpdateLolosJob(ApplicationDbContext context, EncryptionManager encryptionManager, HashManager hashManager,
        LoloManager loloManager)
    {
        _context = context;
        _encryptionManager = encryptionManager;
        _hashManager = hashManager;
        _loloManager = loloManager;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var masterKey = context.MergedJobDataMap.Get("masterKey") as string;

        _encryptionManager.Init(masterKey);
        _hashManager.Init(user!.HasherSaltEncrypted);
        _loloManager.Init(user.Id);

        await _loloManager.GenerateAsync();

        //TODO: Send out a notification through websockets informing the user that their lolos have finished updating (we should refetch them afterwards)
    }
}