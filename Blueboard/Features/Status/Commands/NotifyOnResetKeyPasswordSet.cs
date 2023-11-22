using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Helpers.WebApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Status.Commands;

public static class NotifyOnResetKeyPasswordSet
{
    public class Command : IRequest
    {
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

            RuleFor(x => x.Email).NotEmpty().EmailAddress().MustAsync(BeUniqueEmailAsync)
                .WithMessage("Ez az e-mail cím már fel lett iratkozva.");
        }

        private async Task<bool> BeUniqueEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.ResetKeyPasswordSetNotifiers.AllAsync(x => x.Email != email, cancellationToken);
        }
    }

    internal sealed class Handler(ApplicationDbContext context, ResetService resetService) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (resetService.IsResetKeyPasswordSet())
                throw new UnavailableException("A visszaállítási jelszó már be lett állítva.");

            var notifier = new ResetKeyPasswordSetNotifier
            {
                Email = request.Body.Email
            };

            await context.ResetKeyPasswordSetNotifiers.AddAsync(notifier, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}