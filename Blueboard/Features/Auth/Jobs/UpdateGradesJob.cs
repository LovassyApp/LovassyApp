using System.Text.Json;
using Blueboard.Core.Auth.Services;
using Blueboard.Core.Backboard.Services;
using Blueboard.Features.School.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;
using Quartz;

namespace Blueboard.Features.Auth.Jobs;

/// <summary>
///     The background job that updates the grades of a user. Fired when a session is created (login/refresh)
/// </summary>
public class UpdateGradesJob(ApplicationDbContext dbContext, UserAccessor userAccessor,
        EncryptionManager encryptionManager,
        BackboardAdapter backboardAdapter, IPublisher publisher)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var masterKey = context.MergedJobDataMap.Get("masterKey") as string;

        dbContext.Attach(user); // We have to attach the user to the context, because it's not tracked yet in this scope (It caused an issue back when we used hangfire, maybe it wouldn't now)
        userAccessor.User = user!;

        encryptionManager.SetMasterKeyTemporarily(masterKey!);

        await backboardAdapter.UpdateAsync();

        await publisher.Publish(new GradesUpdatedEvent
        {
            UserId = user!.Id
        });
    }
}