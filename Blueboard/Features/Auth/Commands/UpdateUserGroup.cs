using Blueboard.Core.Auth.Utils;
using Blueboard.Features.Auth.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;

namespace Blueboard.Features.Auth.Commands;

public static class UpdateUserGroup
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Name { get; set; }
        public string[] Permissions { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Permissions).NotNull().Must(BeExistingPermissions)
                .WithMessage("A megadott jogosultságok közül legalább egy nem létezik.");
        }

        private bool BeExistingPermissions(RequestBody model, string[] permissions)
        {
            if (PermissionUtils.Permissions == null)
                throw new UnavailableException(
                    "A jogosultságok még nincsenek betöltve. (Ennek nem kéne megtörténnie, kérlek jelezd a hibát a fejlesztőknek)");

            return permissions.All(permission => PermissionUtils.Permissions.Select(p => p.Name).Contains(permission));
        }
    }

    internal sealed class Handler(ApplicationDbContext context, IPublisher publisher) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var userGroup = await context.UserGroups.FindAsync(request.Id);

            if (userGroup == null)
                throw new NotFoundException(nameof(UserGroup), request.Id);

            request.Body.Adapt(userGroup);
            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new UserGroupsUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}