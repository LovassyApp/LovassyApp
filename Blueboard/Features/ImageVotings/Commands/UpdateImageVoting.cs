using Blueboard.Features.ImageVotings.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.ImageVotings.Commands;

public static class UpdateImageVoting
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Type { get; set; }

        public List<RequestBodyImageVotingAspect> Aspects { get; set; }

        public bool Active { get; set; }
        public bool ShowUploaderInfo { get; set; }

        public int UploaderUserGroupId { get; set; }
        public int? BannedUserGroupId { get; set; }

        public int MaxUploadsPerUser { get; set; }

        public bool SuperIncrementAllowed { get; set; }
        public int SuperIncrementValue { get; set; }
    }

    public class RequestBodyImageVotingAspect
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        private readonly ApplicationDbContext _context;

        public RequestBodyValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(255);

            RuleFor(x => x.Type).NotEmpty().IsEnumName(typeof(ImageVotingType));

            RuleFor(x => x.Aspects).NotNull().Must(a => a.DistinctBy(e => e.Key).Count() == a.Count);
            RuleForEach(x => x.Aspects).ChildRules(v =>
            {
                v.RuleFor(i => i.Key).NotEmpty().Matches("^[a-zA-Z][a-zA-Z0-9_]*$").WithMessage(
                    "A kulcs csak angol kis- és nagybetűket, számokat és alulvonásokat tartalmazhat, és nem kezdődhet számmal.");
                v.RuleFor(a => a.Name).NotEmpty().MaximumLength(255);
                v.RuleFor(a => a.Description).NotEmpty().MaximumLength(255);
            });

            RuleFor(x => x.Active).NotNull();
            RuleFor(x => x.ShowUploaderInfo).NotNull();

            RuleFor(x => x.UploaderUserGroupId).NotNull().MustAsync(BeExistingUserGroupAsync)
                .WithMessage("A megadott felhasználói csoport nem létezik.");
            RuleFor(x => x.BannedUserGroupId).MustAsync(BeExistingUserGroupAsync)
                .WithMessage("A megadott felhasználói csoport nem létezik.");

            RuleFor(x => x.MaxUploadsPerUser).NotNull().GreaterThanOrEqualTo(1);

            RuleFor(x => x.SuperIncrementAllowed).NotNull();
            RuleFor(x => x.SuperIncrementValue).NotNull().GreaterThanOrEqualTo(2);
        }

        private async Task<bool> BeExistingUserGroupAsync(RequestBody model, int userGroupId,
            CancellationToken cancellationToken)
        {
            return await _context.UserGroups.AnyAsync(g => g.Id == userGroupId, cancellationToken);
        }

        private async Task<bool> BeExistingUserGroupAsync(RequestBody model, int? userGroupId,
            CancellationToken cancellationToken)
        {
            return userGroupId == null ||
                   await _context.UserGroups.AnyAsync(g => g.Id == userGroupId, cancellationToken);
        }
    }

    internal sealed class Handler(ApplicationDbContext context, IPublisher publisher) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var imageVoting = await context.ImageVotings.Include(v => v.Choices).Include(v => v.Entries)
                .ThenInclude(e => e.Increments).FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            if (imageVoting == null)
                throw new NotFoundException(nameof(ImageVoting), request.Id);

            var originalImageVoting = imageVoting;

            request.Body.Adapt(imageVoting);

            var didDeleteChoices = false;
            var didDeleteIncrements = false;

            //Iterate through the deleted aspects and delete the choices and increments related to them
            foreach (var aspect in
                     originalImageVoting.Aspects.Where(a => imageVoting.Aspects.All(a2 => a2.Key != a.Key)))
            {
                foreach (var choice in originalImageVoting.Choices.Where(c => c.AspectKey == aspect.Key))
                {
                    context.ImageVotingChoices.Remove(choice);
                    didDeleteChoices = true;
                }

                foreach (var entry in originalImageVoting.Entries)
                foreach (var increment in entry.Increments.Where(i => i.AspectKey == aspect.Key))
                {
                    context.ImageVotingEntryIncrements.Remove(increment);
                    didDeleteIncrements = true;
                }
            }

            if (didDeleteChoices)
                await publisher.Publish(new ImageVotingChoicesUpdatedEvent(), cancellationToken);

            if (didDeleteIncrements)
                await publisher.Publish(new ImageVotingEntryIncrementsUpdatedEvent(), cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new ImageVotingsUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}