using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Features.ImageVotings.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation.Results;
using Helpers.WebApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.ImageVotings.Commands;

public class UnchooseImageVotingEntry
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string? AspectKey { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, UserAccessor userAccessor,
            PermissionManager permissionManager,
            IPublisher publisher)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var entry = await context.ImageVotingEntries.Include(e => e.ImageVoting)
                .ThenInclude(v => v.Choices.Where(c => c.UserId == userAccessor.User.Id))
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (entry == null)
                throw new NotFoundException(nameof(ImageVotingEntry), request.Id);

            if (entry.ImageVoting.Type != ImageVotingType.SingleChoice)
                throw new BadRequestException("A megadott kép nem egy választásos szavazás része");

            CheckUnchoosability(entry);

            if (entry.ImageVoting.Aspects.Any())
            {
                if (string.IsNullOrWhiteSpace(request.Body.AspectKey))
                    throw new ValidationException(new[]
                    {
                        new ValidationFailure(nameof(RequestBody.AspectKey), "A(z) 'AspectKey' mező nem lehet üres.")
                    });

                if (entry.ImageVoting.Aspects.All(a => a.Key != request.Body.AspectKey))
                    throw new ValidationException(new[]
                        { new ValidationFailure(nameof(RequestBody.AspectKey), "A megadott szempont nem létezik.") });

                var choice = entry.ImageVoting.Choices.FirstOrDefault(c =>
                    c.AspectKey == request.Body.AspectKey && c.ImageVotingEntryId == entry.Id);

                if (choice == null)
                    throw new BadRequestException("A megadott szempontban nem a megadott képet választottad");

                context.ImageVotingChoices.Remove(choice);
            }
            else
            {
                var choice = entry.ImageVoting.Choices.FirstOrDefault(c => c.ImageVotingEntryId == entry.Id);

                if (choice == null)
                    throw new BadRequestException("Nem a megadott képet választottad");

                context.ImageVotingChoices.Remove(choice);
            }

            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new ImageVotingChoicesUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }

        private void CheckUnchoosability(ImageVotingEntry entry)
        {
            if (entry.ImageVoting.Type != ImageVotingType.SingleChoice)
                throw new BadRequestException("A megadott szavazás nem egy választásos szavazás");

            if (!entry.ImageVoting.Active &&
                !permissionManager.CheckPermission(typeof(ImageVotingsPermissions.UnchooseImageVotingEntry)))
                throw new BadRequestException("A megadott szavazás nem aktív");
        }
    }
}