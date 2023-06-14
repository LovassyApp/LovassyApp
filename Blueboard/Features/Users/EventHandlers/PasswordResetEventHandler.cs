using System.Text.Json;
using Blueboard.Features.Auth.Events;
using Blueboard.Features.Users.Jobs;
using Blueboard.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using SessionOptions = Blueboard.Core.Auth.Services.Options.SessionOptions;

namespace Blueboard.Features.Users.EventHandlers;

public class PasswordResetEventHandler : INotificationHandler<PasswordResetEvent>
{
    private readonly ApplicationDbContext _context;
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly SessionOptions _sessionOptions;


    public PasswordResetEventHandler(ApplicationDbContext context, ISchedulerFactory schedulerFactory,
        IOptions<SessionOptions> sessionOptions)
    {
        _context = context;
        _schedulerFactory = schedulerFactory;
        _sessionOptions = sessionOptions.Value;
    }

    public async Task Handle(PasswordResetEvent notification, CancellationToken cancellationToken)
    {
        // We want to kick the user when they reset their password, because if their password was compromised, we want to make sure that the attacker can't use the old token to access the account
        var personalAccessTokens = await _context.PersonalAccessTokens.Where(t => t.UserId == notification.User.Id)
            .Where(t => t.CreatedAt >= DateTime.UtcNow.AddMinutes(-_sessionOptions.ExpiryMinutes)).AsNoTracking()
            .ToListAsync(cancellationToken);

        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

        var kickUsersJob = JobBuilder.Create<KickUsersJob>().WithIdentity("kickUsers", "userKickedJobs")
            .UsingJobData("tokensJson", JsonSerializer.Serialize(personalAccessTokens.Select(t => t.Token)))
            .Build();

        var kickUsersTrigger = TriggerBuilder.Create().WithIdentity("kickUsersTrigger", "userKickedJobs")
            .StartNow().Build();

        await scheduler.ScheduleJob(kickUsersJob, kickUsersTrigger, cancellationToken);
    }
}