using System.Text.Json;
using Blueboard.Core.Auth.Services;
using Blueboard.Core.Lolo.Services;
using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;
using Quartz;

namespace Blueboard.Features.Auth.Jobs;

public class UpdateLolosJob(EncryptionManager encryptionManager, UserAccessor userAccessor,
        LoloManager loloManager, IPublisher publisher)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var masterKey = context.MergedJobDataMap.Get("masterKey") as string;

        userAccessor.User = user;

        encryptionManager.SetMasterKeyTemporarily(masterKey!);

        await loloManager.GenerateAsync();

        await publisher.Publish(new LolosUpdatedEvent
        {
            UserId = user!.Id
        });
    }
}