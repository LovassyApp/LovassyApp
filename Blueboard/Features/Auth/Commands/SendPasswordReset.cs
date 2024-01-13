using Blueboard.Features.Auth.Jobs;
using Blueboard.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shimmer.Services;

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
                .WithMessage("Nem létezik ilyen email című felhasználó.");
        }

        private async Task<bool> BeRegisteredEmail(RequestBody model, string email,
            CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
        }
    }

    internal sealed class Handler(ApplicationDbContext context, IShimmerJobFactory jobFactory)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Where(x => x.Email == request.Body.Email).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            var sendPasswordResetJob =
                await jobFactory.CreateAsync<SendPasswordResetJob, SendPasswordResetJob.Data>(cancellationToken);

            sendPasswordResetJob.Data(new SendPasswordResetJob.Data
            {
                User = user!,
                PasswordResetUrl = request.PasswordResetUrl,
                PasswordResetTokenQueryKey = request.PasswordResetTokenQueryKey
            });

            await sendPasswordResetJob.FireAsync(cancellationToken);

            return Unit.Value;
        }
    }
}