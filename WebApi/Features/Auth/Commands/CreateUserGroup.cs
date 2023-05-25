using FluentValidation;
using Helpers.Framework.Exceptions;
using Mapster;
using MediatR;
using WebApi.Core.Auth.Utils;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Commands;

public static class CreateUserGroup
{
    public class Command : IRequest<Response>
    {
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
                .WithMessage("The provided permissions are not all valid");
        }

        private bool BeExistingPermissions(RequestBody model, string[] permissions)
        {
            if (PermissionUtils.Permissions == null) throw new UnavailableException("Permissions are not yet loaded");

            return permissions.All(permission => PermissionUtils.Permissions.Select(p => p.Name).Contains(permission));
        }
    }

    public class Response
    {
        public string Name { get; set; }
        public string[] Permissions { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var userGroup = request.Body.Adapt<UserGroup>();

            await _context.UserGroups.AddAsync(userGroup, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return userGroup.Adapt<Response>();
        }
    }
}