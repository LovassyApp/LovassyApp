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

    internal sealed class Handler(ApplicationDbContext context, ISchedulerFactory schedulerFactory,
            IOptions<SessionOptions> sessionOptions)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var personalAccessTokens = await context.PersonalAccessTokens
                .Where(t => t.CreatedAt >= DateTime.UtcNow.AddMinutes(-sessionOptions.Value.ExpiryMinutes))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

            //Schedule a job because it could take some time to hash all the tokens and check them against the SessionService
            var kickUsersJob = JobBuilder.Create<KickUsersJob>()
                .UsingJobData("tokensJson", JsonSerializer.Serialize(personalAccessTokens.Select(t => t.Token)))
                .Build();

            var kickUsersTrigger = TriggerBuilder.Create()
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(kickUsersJob, kickUsersTrigger, cancellationToken);

            return Unit.Value;
        }
    }
}