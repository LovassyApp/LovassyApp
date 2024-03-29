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

public static class ChooseImageVotingEntry
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
                .ThenInclude(v => v.Choices.Where(c => c.UserId == userAccessor.User.Id))
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (entry == null)
                throw new NotFoundException(nameof(ImageVotingEntry), request.Id);

            CheckChoosability(entry);

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

                var choice = entry.ImageVoting.Choices.FirstOrDefault(a => a.AspectKey == request.Body.AspectKey);

                if (choice == null)
                    entry.ImageVoting.Choices.Add(new ImageVotingChoice
                    {
                        AspectKey = request.Body.AspectKey, UserId = userAccessor.User.Id,
                        ImageVotingId = entry.ImageVotingId, ImageVotingEntryId = entry.Id
                    });
                else
                    choice.ImageVotingEntryId = entry.Id;
            }
            else
            {
                var choice = entry.ImageVoting.Choices.FirstOrDefault();
                if (choice == null)
                    entry.ImageVoting.Choices.Add(new ImageVotingChoice
                    {
                        UserId = userAccessor.User.Id, ImageVotingId = entry.ImageVotingId,
                        ImageVotingEntryId = entry.Id
                    });
                else
                    choice.ImageVotingEntryId = entry.Id;
            }

            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new ImageVotingChoicesUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }

        private void CheckChoosability(ImageVotingEntry entry)
        {
            if (entry.ImageVoting.Type != ImageVotingType.SingleChoice)
                throw new BadRequestException("A megadott szavazás nem egy választásos szavazás");

            if (!entry.ImageVoting.Active &&
                !permissionManager.CheckPermission(typeof(ImageVotingsPermissions.ChooseImageVotingEntry)))
                throw new BadRequestException("A megadott szavazás nem aktív");

            if (entry.UserId == userAccessor.User.Id)
                throw new BadRequestException("Nem szavazhatsz a saját képedre");
        }
    }
}