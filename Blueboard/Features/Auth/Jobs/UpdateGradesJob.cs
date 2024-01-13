using Blueboard.Core.Auth.Services;
using Blueboard.Core.Backboard.Services;
using Blueboard.Features.School.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;
using Quartz;
using Shimmer.Core;

namespace Blueboard.Features.Auth.Jobs;

/// <summary>
///     The background job that updates the grades of a user. Fired when a session is created (login/refresh)
/// </summary>
public class UpdateGradesJob(
    ApplicationDbContext dbContext,
    UserAccessor userAccessor,
    EncryptionManager encryptionManager,
    BackboardAdapter backboardAdapter,
    IPublisher publisher)
    : ShimmerJob<UpdateGradesJob.Data>
{
    protected override async Task Process(Data data, IJobExecutionContext context)
    {
        dbContext.Attach(data
            .User); // We have to attach the user to the context, because it's not tracked yet in this scope (It caused an issue back when we used hangfire, maybe it wouldn't now)
        userAccessor.User = data.User;

        encryptionManager.SetMasterKeyTemporarily(data.MasterKey);

        await backboardAdapter.UpdateAsync();

        await publisher.Publish(new GradesUpdatedEvent
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