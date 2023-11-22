using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;

namespace Blueboard.Features.Shop.Commands;

public static class UpdateLoloRequest
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        public RequestBody Body { get; set; }
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

    internal sealed class Handler(ApplicationDbContext context, UserAccessor userAccessor,
            PermissionManager permissionManager,
            IPublisher publisher)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var loloRequest = await context.LoloRequests.FindAsync(request.Id);

            if (loloRequest == null)
                throw new NotFoundException(nameof(LoloRequest), request.Id);

            if (loloRequest.UserId != userAccessor.User.Id &&
                !permissionManager.CheckPermission(typeof(ShopPermissions.UpdateLoloRequest)))
                throw new ForbiddenException();

            if (loloRequest.AcceptedAt != null || loloRequest.DeniedAt != null)
                throw new BadRequestException(
                    "Ez a kérvény már nem módosítható. (el lett fogadva vagy el lett utasítva)");

            request.Body.Adapt(loloRequest);
            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new LoloRequestUpdatedEvent
            {
                UserId = loloRequest.UserId
            }, cancellationToken).ConfigureAwait(false);

            return Unit.Value;
        }
    }
}