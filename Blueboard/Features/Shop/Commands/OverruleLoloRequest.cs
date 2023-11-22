using Blueboard.Core.Lolo.Services;
using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Helpers.WebApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Shop.Commands;

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

    internal sealed class Handler(ApplicationDbContext context, LoloManager loloManager, IPublisher publisher)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var loloRequest = await context.LoloRequests.FindAsync(request.Id, cancellationToken);

            if (loloRequest is null)
                throw new NotFoundException(nameof(LoloRequest), request.Id);

            if (loloRequest.AcceptedAt != null || loloRequest.DeniedAt != null)
                throw new BadRequestException("Ez a kérvény már el lett fogadva vagy el lett utasítva.");

            if (request.Body.Accepted)
            {
                loloRequest.AcceptedAt = DateTime.UtcNow;

                await loloManager.SaveFromRequestAsync(loloRequest, request.Body.LoloAmount);

                await publisher.Publish(new LolosUpdatedEvent
                {
                    UserId = loloRequest.UserId
                }, cancellationToken);
            }
            else
            {
                loloRequest.DeniedAt = DateTime.UtcNow;
            }

            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new LoloRequestUpdatedEvent
            {
                UserId = loloRequest.UserId
            }, cancellationToken);

            return Unit.Value;
        }
    }
}