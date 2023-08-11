using System.Text.Json;
using Blueboard.Core.Auth.Services;
using Blueboard.Core.Backboard.Services;
using Blueboard.Features.Auth.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;
using Quartz;

namespace Blueboard.Features.Auth.Jobs;

/// <summary>
///     The background job that updates the grades of a user. Fired when a session is created (login/refresh)
/// </summary>
public class UpdateGradesJob : IJob
{
    private readonly BackboardAdapter _backboardAdapter;
    private readonly ApplicationDbContext _context;
    private readonly EncryptionManager _encryptionManager;
    private readonly IPublisher _publisher;
    private readonly UserAccessor _userAccessor;

    public UpdateGradesJob(ApplicationDbContext context, UserAccessor userAccessor, EncryptionManager encryptionManager,
        BackboardAdapter backboardAdapter, IPublisher publisher)
    {
        _context = context;
        _userAccessor = userAccessor;
        _encryptionManager = encryptionManager;
        _backboardAdapter = backboardAdapter;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var masterKey = context.MergedJobDataMap.Get("masterKey") as string;

        _context.Attach(user); // We have to attach the user to the context, because it's not tracked yet in this scope (It caused an issue back when we used hangfire, maybe it wouldn't now)
        _userAccessor.User = user!;

        _encryptionManager.SetMasterKeyTemporarily(masterKey!);

        await _backboardAdapter.UpdateAsync();

        await _publisher.Publish(new GradesUpdatedEvent
        {
            UserId = user!.Id
        });
    }
}