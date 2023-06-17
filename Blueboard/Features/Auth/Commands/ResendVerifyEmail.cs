using System.Text.Json;
using Blueboard.Core.Auth.Services;
using Blueboard.Features.Auth.Jobs;
using MediatR;
using Quartz;

namespace Blueboard.Features.Auth.Commands;

public static class ResendVerifyEmail
{
    public class Command : IRequest
    {
        public string VerifyUrl { get; set; }
        public string VerifyTokenQueryKey { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly UserAccessor _userAccessor;

        public Handler(UserAccessor userAccessor, ISchedulerFactory schedulerFactory)
        {
            _userAccessor = userAccessor;
            _schedulerFactory = schedulerFactory;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.User;

            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            var sendVerifyEmailJob = JobBuilder.Create<SendVerifyEmailJob>()
                .UsingJobData("userJson", JsonSerializer.Serialize(user))
                .UsingJobData("verifyUrl", request.VerifyUrl)
                .UsingJobData("verifyTokenQueryKey", request.VerifyTokenQueryKey)
                .Build();

            var sendVerifyEmailTrigger = TriggerBuilder.Create()
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(sendVerifyEmailJob, sendVerifyEmailTrigger, cancellationToken);

            return Unit.Value;
        }
    }
}