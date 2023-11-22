using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Shop.Commands;

public static class UpdateLoloRequestCreatedNotifiers
{
    public class Command : IRequest
    {
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public List<string> Emails { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Emails)
                .NotEmpty()
                .Must(NotContainDuplicates)
                .WithMessage("A megadott emailek között van két azonos elem.");
            RuleForEach(x => x.Emails).EmailAddress();
        }

        private bool NotContainDuplicates(RequestBody model, List<string> emails)
        {
            var emailSet = new HashSet<string>(emails);
            return emailSet.Count == emails.Count;
        }
    }

    internal sealed class Handler(ApplicationDbContext context, IPublisher publisher) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            await context.Database.ExecuteSqlRawAsync(
                @$"truncate table ""{context.Model.FindEntityType(typeof(LoloRequestCreatedNotifier))!.GetTableName()}""",
                cancellationToken);

            var notifiers = request.Body.Emails.Select(e => new LoloRequestCreatedNotifier { Email = e }).ToList();
            await context.LoloRequestCreatedNotifiers.AddRangeAsync(notifiers, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new LoloRequestCreatedNotifiersUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}