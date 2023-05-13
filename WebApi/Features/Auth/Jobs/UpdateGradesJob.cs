using System.Text.Json;
using Quartz;
using WebApi.Core.Auth.Services;
using WebApi.Core.Backboard.Services;
using WebApi.Core.Cryptography.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Jobs;

/// <summary>
///     The background job that updates the grades of a user. Fired when a session is created (login/refresh)
/// </summary>
public class UpdateGradesJob : IJob
{
    private readonly BackboardAdapter _backboardAdapter;
    private readonly ApplicationDbContext _context;
    private readonly EncryptionManager _encryptionManager;
    private readonly UserAccessor _userAccessor;

    public UpdateGradesJob(ApplicationDbContext context, UserAccessor userAccessor, EncryptionManager encryptionManager,
        BackboardAdapter backboardAdapter)
    {
        _context = context;
        _userAccessor = userAccessor;
        _encryptionManager = encryptionManager;
        _backboardAdapter = backboardAdapter;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var masterKey = context.MergedJobDataMap.Get("masterKey") as string;

        _context.Attach(user); // We have to attach the user to the context, because it's not tracked yet in this scope (It caused an issue back when we used hangfire, maybe it wouldn't now)
        _userAccessor.User = user;

        _encryptionManager.SetMasterKeyTemporarily(masterKey!);

        await _backboardAdapter.UpdateAsync();
    }
}