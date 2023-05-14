using System.Text.Json;
using MediatR;
using Quartz;
using WebApi.Core.Auth.Services;
using WebApi.Features.Auth.Jobs;

namespace WebApi.Features.Auth.Commands;

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
                .WithIdentity("sendVerifyEmail", "userCreatedJobs")
                .UsingJobData("userJson", JsonSerializer.Serialize(user))
                .UsingJobData("verifyUrl", request.VerifyUrl)
                .UsingJobData("verifyTokenQueryKey", request.VerifyTokenQueryKey)
                .Build();

            var sendVerifyEmailTrigger = TriggerBuilder.Create()
                .WithIdentity("sendVerifyEmailTrigger", "userCreatedJobs")
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(sendVerifyEmailJob, sendVerifyEmailTrigger, cancellationToken);

            return Unit.Value;
        }
    }
}