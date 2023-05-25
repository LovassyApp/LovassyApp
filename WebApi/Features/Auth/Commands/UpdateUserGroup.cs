using FluentValidation;
using Helpers.Framework.Exceptions;
using Mapster;
using MediatR;
using WebApi.Core.Auth.Utils;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Commands;

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
            RuleFor(x => x.Permissions).NotEmpty();
            RuleFor(x => x.Permissions).NotNull().Must(BeExistingPermissions)
                .WithMessage("The provided permissions are not all valid");
        }

        private bool BeExistingPermissions(RequestBody model, string[] permissions)
        {
            if (PermissionUtils.Permissions == null) throw new UnavailableException("Permissions are not yet loaded");

            return permissions.All(permission => PermissionUtils.Permissions.Select(p => p.Name).Contains(permission));
        }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var userGroup = await _context.UserGroups.FindAsync(request.Id);

            if (userGroup == null)
                throw new NotFoundException(nameof(UserGroup), request.Id);

            request.Body.Adapt(userGroup);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}