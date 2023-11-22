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

    internal sealed class Handler(ApplicationDbContext context, UserAccessor userAccessor, FilesService filesService,
            PermissionManager permissionManager)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var fileUpload = await context.FileUploads.FindAsync(request.Id);

            if (fileUpload == null ||
                (!permissionManager.CheckPermission(typeof(ImageVotingsPermissions.DeleteImageVotingEntryImage)) &&
                 fileUpload.UserId != userAccessor.User.Id))
                throw new NotFoundException(nameof(FileUpload), request.Id);

            filesService.DeleteFile(fileUpload);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}