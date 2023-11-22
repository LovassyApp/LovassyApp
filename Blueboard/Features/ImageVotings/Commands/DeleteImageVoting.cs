using Blueboard.Features.ImageVotings.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.ImageVotings.Commands;

public static class DeleteImageVoting
{
    public class Command : IRequest
    {
        public int Id { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, IPublisher publisher) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var imageVoting = await context.ImageVotings.FindAsync(request.Id);
            if (imageVoting == null) throw new NotFoundException(nameof(ImageVoting), request.Id);

            context.ImageVotings.Remove(imageVoting);
            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new ImageVotingsUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}