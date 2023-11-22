using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Features.ImageVotings.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;

namespace Blueboard.Features.ImageVotings.Commands;

public static class UpdateImageVotingEntry
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.ImageUrl).NotEmpty().MaximumLength(255);
        }
    }

    internal sealed class Handler(ApplicationDbContext context, UserAccessor userAccessor, IPublisher publisher,
            PermissionManager permissionManager)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var entry = await context.ImageVotingEntries.FindAsync(request.Id);

            if (entry == null)
                throw new NotFoundException(nameof(ImageVotingEntry), request.Id);

            if (entry.UserId != userAccessor.User.Id &&
                !permissionManager.CheckPermission(typeof(ImageVotingsPermissions.UpdateImageVotingEntry)))
                throw new NotFoundException(nameof(ImageVotingEntry), request.Id);

            request.Body.Adapt(entry);

            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new ImageVotingEntriesUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}