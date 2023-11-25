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

public static class DeleteImageVotingEntryIncrement
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

    internal sealed class Handler(ApplicationDbContext context, UserAccessor userAccessor, IPublisher publisher,
            PermissionManager permissionManager)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var entry = await context.ImageVotingEntries.Include(e => e.ImageVoting)
                .Include(e => e.Increments.Where(i => i.UserId == userAccessor.User.Id))
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (entry == null)
                throw new NotFoundException(nameof(ImageVotingEntry), request.Id);

            CheckIncrementDeletability(entry);

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

                var increment = entry.Increments.FirstOrDefault(i => i.AspectKey == request.Body.AspectKey);

                if (increment == null)
                    throw new BadRequestException("A megadott szempontban nem szabvaztál a megadott képre");

                context.ImageVotingEntryIncrements.Remove(increment);
            }
            else
            {
                var increment = entry.Increments.FirstOrDefault();

                if (increment == null)
                    throw new BadRequestException("A megadott képre nem szavaztál");

                context.ImageVotingEntryIncrements.Remove(increment);
            }

            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new ImageVotingEntryIncrementsUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }

        private void CheckIncrementDeletability(ImageVotingEntry entry)
        {
            if (entry.ImageVoting.Type != ImageVotingType.Increment)
                throw new BadRequestException("A megadott kép nem egy inkrementáló szavazás része");

            if (!entry.ImageVoting.Active &&
                !permissionManager.CheckPermission(typeof(ImageVotingsPermissions.DeleteImageVotingEntryIncrement)))
                throw new BadRequestException("A megadott szavazás nem aktív");
        }
    }
}