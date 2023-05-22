using FluentValidation;
using Helpers.Framework.Exceptions;
using Mapster;
using MediatR;
using WebApi.Core.Auth.Permissions;
using WebApi.Core.Auth.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Shop.Commands;

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

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly PermissionManager _permissionManager;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext context, UserAccessor userAccessor, PermissionManager permissionManager)
        {
            _context = context;
            _userAccessor = userAccessor;
            _permissionManager = permissionManager;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var loloRequest = await _context.LoloRequests.FindAsync(request.Id);

            if (loloRequest == null)
                throw new NotFoundException(nameof(LoloRequest), request.Id);

            if (loloRequest.UserId != _userAccessor.User!.Id &&
                !_permissionManager.CheckPermission(typeof(ShopPermissions.UpdateLoloRequest)))
                throw new ForbiddenException();

            if (loloRequest.AcceptedAt != null || loloRequest.DeniedAt != null)
                throw new BadRequestException("This request has already been handled");

            request.Body.Adapt(loloRequest);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}