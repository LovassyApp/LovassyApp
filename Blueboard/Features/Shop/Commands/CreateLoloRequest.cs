using Blueboard.Core.Auth.Services;
using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Mapster;
using MediatR;

namespace Blueboard.Features.Shop.Commands;

public static class CreateLoloRequest
{
    public class Command : IRequest<Response>
    {
        public RequestBody Body { get; set; }
        public string LoloRequestsUrl { get; set; }
    }

    public class RequestBody
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Body).NotEmpty().MaximumLength(65535);
        }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }

        public Guid UserId { get; set; }

        public DateTime? AcceptedAt { get; set; }
        public DateTime? DeniedAt { get; set; }
    }

    internal sealed class Handler(UserAccessor userAccessor, ApplicationDbContext context, IPublisher publisher)
        : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var loloRequest = request.Body.Adapt<LoloRequest>();
            loloRequest.UserId = userAccessor.User.Id;

            await context.LoloRequests.AddAsync(loloRequest, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new LoloRequestUpdatedEvent
            {
                UserId = userAccessor.User.Id
            }, cancellationToken);

            await publisher.Publish(new LoloRequestCreatedEvent
            {
                LoloRequest = loloRequest,
                LoloRequestsUrl = request.LoloRequestsUrl
            }, cancellationToken);

            return loloRequest.Adapt<Response>();
        }
    }
}