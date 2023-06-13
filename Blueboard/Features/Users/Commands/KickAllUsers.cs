using System.Text.Json;
using Blueboard.Features.Users.Jobs;
using Blueboard.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using SessionOptions = Blueboard.Core.Auth.Services.Options.SessionOptions;

namespace Blueboard.Features.Users.Commands;

public static class KickAllUsers
{
    public class Command : IRequest
    {
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly SessionOptions _sessionOptions;

        public Handler(ApplicationDbContext context, ISchedulerFactory schedulerFactory,
            IOptions<SessionOptions> sessionOptions)
        {
            _context = context;
            _schedulerFactory = schedulerFactory;
            _sessionOptions = sessionOptions.Value;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var personalAccessTokens = await _context.PersonalAccessTokens
                .Where(t => t.CreatedAt >= DateTime.UtcNow.AddMinutes(-_sessionOptions.ExpiryMinutes)).AsNoTracking()
                .ToListAsync(cancellationToken);

            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            var stopSessionsJob = JobBuilder.Create<StopSessionsJob>().WithIdentity("stopSessions", "userKickedJobs")
                .UsingJobData("tokensJson", JsonSerializer.Serialize(personalAccessTokens.Select(t => t.Token)))
                .Build();

            var stopSessionsTrigger = TriggerBuilder.Create().WithIdentity("stopSessionsTrigger", "userKickedJobs")
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(stopSessionsJob, stopSessionsTrigger, cancellationToken);

            return Unit.Value;
        }
    }
}