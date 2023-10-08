using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Files.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.ImageVotings.Commands;

public static class DeleteImageVotingEntryImage
{
    public class Command : IRequest
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly FilesService _filesService;
        private readonly PermissionManager _permissionManager;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext context, UserAccessor userAccessor, FilesService filesService,
            PermissionManager permissionManager)
        {
            _context = context;
            _userAccessor = userAccessor;
            _filesService = filesService;
            _permissionManager = permissionManager;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var fileUpload = await _context.FileUploads.FindAsync(request.Id);

            if (fileUpload == null ||
                (!_permissionManager.CheckPermission(typeof(ImageVotingsPermissions.DeleteImageVotingEntryImage)) &&
                 fileUpload.UserId != _userAccessor.User.Id))
                throw new NotFoundException(nameof(FileUpload), request.Id);

            _filesService.DeleteFile(fileUpload);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}