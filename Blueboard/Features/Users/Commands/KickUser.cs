using System.Text.Json;
using Blueboard.Features.Users.Jobs;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using SessionOptions = Blueboard.Core.Auth.Services.Options.SessionOptions;

namespace Blueboard.Features.Users.Commands;

public static class KickUser
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
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
            var user = await _context.Users.Where(u => u.Id == request.Id).Include(u =>
                    u.PersonalAccessTokens.Where(t =>
                        t.CreatedAt >= DateTime.UtcNow.AddMinutes(-_sessionOptions.ExpiryMinutes)))
                .AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            if (user == null) throw new NotFoundException(nameof(User), request.Id);

            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            var kickUsersJob = JobBuilder.Create<KickUsersJob>().WithIdentity("kickUsers", "userKickedJobs")
                .UsingJobData("tokensJson", JsonSerializer.Serialize(user.PersonalAccessTokens.Select(t => t.Token)))
                .Build();

            var kickUsersTrigger = TriggerBuilder.Create().WithIdentity("kickUsersTrigger", "userKickedJobs")
                .StartNow().Build();

            await scheduler.ScheduleJob(kickUsersJob, kickUsersTrigger, cancellationToken);

            return Unit.Value;
        }
    }
}