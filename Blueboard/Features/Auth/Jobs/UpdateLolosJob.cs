using System.Text.Json;
using Blueboard.Core.Auth.Services;
using Blueboard.Core.Lolo.Services;
using Blueboard.Features.Auth.Events;
using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;
using Quartz;

namespace Blueboard.Features.Auth.Jobs;

public class UpdateLolosJob : IJob
{
    private readonly EncryptionManager _encryptionManager;
    private readonly LoloManager _loloManager;
    private readonly IPublisher _publisher;
    private readonly UserAccessor _userAccessor;

    public UpdateLolosJob(EncryptionManager encryptionManager, UserAccessor userAccessor,
        LoloManager loloManager, IPublisher publisher)
    {
        _encryptionManager = encryptionManager;
        _userAccessor = userAccessor;
        _loloManager = loloManager;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var masterKey = context.MergedJobDataMap.Get("masterKey") as string;

        _userAccessor.User = user;

        _encryptionManager.SetMasterKeyTemporarily(masterKey!);

        await _loloManager.GenerateAsync();

        await _publisher.Publish(new LolosUpdatedEvent
        {
            UserId = user!.Id
        });
    }
}