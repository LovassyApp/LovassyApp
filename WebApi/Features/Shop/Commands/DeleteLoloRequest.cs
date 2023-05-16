using MediatR;
using WebApi.Common.Exceptions;
using WebApi.Core.Auth.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Shop.Commands;

public static class DeleteLoloRequest
{
    public class Command : IRequest
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext context, UserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var loloRequest = await _context.LoloRequests.FindAsync(request.Id);

            if (loloRequest == null)
                throw new NotFoundException(nameof(LoloRequest), request.Id);

            if (loloRequest.UserId != _userAccessor.User!.Id) //TODO: Check for superuser here
                throw new ForbiddenException();

            if (loloRequest.AcceptedAt != null || loloRequest.DeniedAt != null)
                throw new BadRequestException("This request has already been handled");

            _context.LoloRequests.Remove(loloRequest);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}