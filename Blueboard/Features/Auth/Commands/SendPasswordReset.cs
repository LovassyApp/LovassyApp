using System.Text.Json;
using Blueboard.Features.Auth.Jobs;
using Blueboard.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Blueboard.Features.Auth.Commands;

public static class SendPasswordReset
{
    public class Command : IRequest
    {
        public string PasswordResetUrl { get; set; }
        public string PasswordResetTokenQueryKey { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Email { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        private readonly ApplicationDbContext _context;

        public RequestBodyValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Email).NotEmpty().MustAsync(BeRegisteredEmail)
                .WithMessage("The email does not belong to a registered user");
        }

        private async Task<bool> BeRegisteredEmail(RequestBody model, string email,
            CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
        }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISchedulerFactory _schedulerFactory;

        public Handler(ApplicationDbContext context, ISchedulerFactory schedulerFactory)
        {
            _context = context;
            _schedulerFactory = schedulerFactory;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(x => x.Email == request.Body.Email).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            var sendPasswordResetJob = JobBuilder.Create<SendPasswordResetJob>()
                .UsingJobData("userJson", JsonSerializer.Serialize(user))
                .UsingJobData("passwordResetUrl", request.PasswordResetUrl)
                .UsingJobData("passwordResetTokenQueryKey", request.PasswordResetTokenQueryKey)
                .Build();

            var sendPasswordResetTrigger = TriggerBuilder.Create()
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(sendPasswordResetJob, sendPasswordResetTrigger, cancellationToken);

            return Unit.Value;
        }
    }
}