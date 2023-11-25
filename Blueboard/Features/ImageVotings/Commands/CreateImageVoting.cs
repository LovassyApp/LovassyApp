using Blueboard.Features.ImageVotings.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.ImageVotings.Commands;

public static class CreateImageVoting
{
    public class Command : IRequest<Response>
    {
        public RequestBody Body { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Type { get; set; }

        public List<ResponseImageVotingAspect> Aspects { get; set; }

        public bool Active { get; set; }
        public bool ShowUploaderInfo { get; set; }

        public int UploaderUserGroupId { get; set; }
        public int? BannedUserGroupId { get; set; }

        public int MaxUploadsPerUser { get; set; }

        public bool SuperIncrementAllowed { get; set; }
        public int SuperIncrementValue { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseImageVotingAspect
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
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

    internal sealed class Handler
        (ApplicationDbContext context, IPublisher publisher) : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var imageVoting = request.Body.Adapt<ImageVoting>();

            await context.ImageVotings.AddAsync(imageVoting, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new ImageVotingsUpdatedEvent(), cancellationToken);

            return imageVoting.Adapt<Response>();
        }
    }
}