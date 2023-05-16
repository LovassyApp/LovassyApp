using FluentValidation;
using MediatR;
using WebApi.Common.Exceptions;
using WebApi.Core.Lolo.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Shop.Commands;

public static class OverruleLoloRequest
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public bool Accepted { get; set; }
        public int LoloAmount { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Accepted).NotNull();
            When(x => x.Accepted, () => { RuleFor(x => x.LoloAmount).NotNull().GreaterThan(0); });
        }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly LoloManager _loloManager;

        public Handler(ApplicationDbContext context, LoloManager loloManager)
        {
            _context = context;
            _loloManager = loloManager;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var loloRequest = await _context.LoloRequests.FindAsync(request.Id, cancellationToken);

            if (loloRequest is null)
                throw new NotFoundException(nameof(LoloRequest), request.Id);

            if (loloRequest.AcceptedAt != null || loloRequest.DeniedAt != null)
                throw new BadRequestException("This request has already been handled");

            if (request.Body.Accepted)
            {
                loloRequest.AcceptedAt = DateTime.UtcNow;

                await _loloManager.SaveFromRequestAsync(loloRequest, request.Body.LoloAmount);

                //TODO: Send out an event and a notification through websockets informing the user that their lolo amount has been updated
            }
            else
            {
                loloRequest.DeniedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}