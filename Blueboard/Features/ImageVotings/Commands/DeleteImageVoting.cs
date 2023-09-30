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

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublisher _publisher;

        public Handler(ApplicationDbContext context, IPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var imageVoting = await _context.ImageVotings.FindAsync(request.Id);
            if (imageVoting == null) throw new NotFoundException(nameof(ImageVoting), request.Id);

            _context.ImageVotings.Remove(imageVoting);
            await _context.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new ImageVotingsUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}