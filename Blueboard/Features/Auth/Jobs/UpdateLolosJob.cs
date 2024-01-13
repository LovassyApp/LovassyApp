using Blueboard.Core.Auth.Services;
using Blueboard.Core.Lolo.Services;
using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;
using Quartz;
using Shimmer.Core;

namespace Blueboard.Features.Auth.Jobs;

public class UpdateLolosJob(
    EncryptionManager encryptionManager,
    UserAccessor userAccessor,
    LoloManager loloManager,
    IPublisher publisher)
    : ShimmerJob<UpdateLolosJob.Data>
{
    protected override async Task Process(Data data, IJobExecutionContext context)
    {
        userAccessor.User = data.User;

        encryptionManager.SetMasterKeyTemporarily(data.MasterKey);

        await loloManager.GenerateAsync();

        await publisher.Publish(new LolosUpdatedEvent
        {
            UserId = data.User.Id
        });
    }

    public class Data
    {
        public User User { get; set; }
        public string MasterKey { get; set; }
    }
}