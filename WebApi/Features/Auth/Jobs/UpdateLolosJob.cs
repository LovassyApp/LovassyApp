using System.Text.Json;
using Quartz;
using WebApi.Core.Auth.Services;
using WebApi.Core.Cryptography.Services;
using WebApi.Core.Lolo.Services;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Jobs;

public class UpdateLolosJob : IJob
{
    private readonly EncryptionManager _encryptionManager;
    private readonly LoloManager _loloManager;
    private readonly UserAccessor _userAccessor;

    public UpdateLolosJob(EncryptionManager encryptionManager, UserAccessor userAccessor,
        LoloManager loloManager)
    {
        _encryptionManager = encryptionManager;
        _userAccessor = userAccessor;
        _loloManager = loloManager;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var masterKey = context.MergedJobDataMap.Get("masterKey") as string;

        _userAccessor.User = user;

        _encryptionManager.SetMasterKeyTemporarily(masterKey!);

        await _loloManager.GenerateAsync();

        //TODO: Send out a notification through websockets informing the user that their lolos have finished updating (we should refetch them afterwards)
    }
}