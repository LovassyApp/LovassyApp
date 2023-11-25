using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Features.ImageVotings.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using FluentValidation.Results;
using Helpers.WebApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ValidationException = Helpers.WebApi.Exceptions.ValidationException;

namespace Blueboard.Features.ImageVotings.Commands;

public static class CreateImageVotingEntryIncrement
{
    public enum ImageVotingEntryIncrementType
    {
        Increment,
        Decrement,
        SuperIncrement
    }

    public class Command : IRequest
    {
        public int Id { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Type { get; set; }

        public string? AspectKey { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Type).IsEnumName(typeof(ImageVotingEntryIncrementType));
        }
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

            CheckIncrementabilityCreatability(entry, Enum.Parse<ImageVotingEntryIncrementType>(request.Body.Type));

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

                if (Enum.Parse<ImageVotingEntryIncrementType>(request.Body.Type) ==
                    ImageVotingEntryIncrementType.SuperIncrement)
                {
                    var superIncrement = context.ImageVotingEntryIncrements.Include(i => i.ImageVotingEntry)
                        .FirstOrDefault(i =>
                            i.UserId == userAccessor.User.Id && i.AspectKey == request.Body.AspectKey &&
                            i.ImageVotingEntry.ImageVotingId == entry.ImageVotingId &&
                            i.Increment == entry.ImageVoting.SuperIncrementValue && i.ImageVotingEntryId != entry.Id);

                    if (superIncrement != null)
                        context.Remove(superIncrement);
                }

                var increment = entry.Increments.FirstOrDefault(i => i.AspectKey == request.Body.AspectKey);

                if (increment == null)
                    entry.Increments.Add(new ImageVotingEntryIncrement
                    {
                        AspectKey = request.Body.AspectKey,
                        UserId = userAccessor.User.Id,
                        Increment = GetIncrementValue(Enum.Parse<ImageVotingEntryIncrementType>(request.Body.Type),
                            entry.ImageVoting)
                    });
                else
                    increment.Increment = GetIncrementValue(
                        Enum.Parse<ImageVotingEntryIncrementType>(request.Body.Type),
                        entry.ImageVoting);
            }
            else
            {
                if (Enum.Parse<ImageVotingEntryIncrementType>(request.Body.Type) ==
                    ImageVotingEntryIncrementType.SuperIncrement)
                {
                    var superIncrement = context.ImageVotingEntryIncrements.Include(i => i.ImageVotingEntry)
                        .FirstOrDefault(i =>
                            i.UserId == userAccessor.User.Id &&
                            i.ImageVotingEntry.ImageVotingId == entry.ImageVotingId &&
                            i.Increment == entry.ImageVoting.SuperIncrementValue && i.ImageVotingEntryId != entry.Id);

                    if (superIncrement != null)
                        context.Remove(superIncrement);
                }

                var increment = entry.Increments.FirstOrDefault();

                if (increment == null)
                    entry.Increments.Add(new ImageVotingEntryIncrement
                    {
                        UserId = userAccessor.User.Id,
                        Increment = GetIncrementValue(Enum.Parse<ImageVotingEntryIncrementType>(request.Body.Type),
                            entry.ImageVoting)
                    });
                else
                    increment.Increment = GetIncrementValue(
                        Enum.Parse<ImageVotingEntryIncrementType>(request.Body.Type),
                        entry.ImageVoting);
            }

            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new ImageVotingEntryIncrementsUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }

        private void CheckIncrementabilityCreatability(ImageVotingEntry entry, ImageVotingEntryIncrementType type)
        {
            if (entry.ImageVoting.Type != ImageVotingType.Increment)
                throw new BadRequestException("A megadott kép nem egy inkrementáló szavazás része");

            if (!entry.ImageVoting.Active &&
                !permissionManager.CheckPermission(typeof(ImageVotingsPermissions.CreateImageVotingEntryIncrement)))
                throw new BadRequestException("A megadott szavazás nem aktív");

            if (!entry.ImageVoting.SuperIncrementAllowed && type == ImageVotingEntryIncrementType.SuperIncrement)
                throw new BadRequestException("A megadott szavazás nem támogatja a 'szuper inkrementálást'");

            if (entry.UserId == userAccessor.User.Id)
                throw new BadRequestException("Nem szavazhatsz a saját képedre");
        }

        private int GetIncrementValue(ImageVotingEntryIncrementType type, ImageVoting imageVoting)
        {
            return type switch
            {
                ImageVotingEntryIncrementType.Increment => 1,
                ImageVotingEntryIncrementType.Decrement => -1,
                ImageVotingEntryIncrementType.SuperIncrement => imageVoting.SuperIncrementValue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}