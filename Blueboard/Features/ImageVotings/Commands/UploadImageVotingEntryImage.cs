using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Files.Extensions;
using Blueboard.Infrastructure.Files.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.ImageVotings.Commands;

public static class UploadImageVotingEntryImage
{
    public class Command : IRequest<Response>
    {
        public RequestBody Body { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Filename { get; set; }
        public string OriginalFilename { get; set; }

        public string MimeType { get; set; }

        public string Path { get; set; }

        public Guid UserId { get; set; }
        public string Url { get; set; }
    }

    public class RequestBody
    {
        public int ImageVotingId { get; set; }
        public IFormFile File { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .AllowedMimeTypes(new[]
                    { "image/bmp", "image/jpeg", "image/x-png", "image/png", "image/gif" })
                .MaxFileSize(4 * 1024 * 1024);
        }
    }

    internal sealed class Handler(ApplicationDbContext dbContext, UserAccessor userAccessor, FilesService filesService,
            PermissionManager permissionManager)
        : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var imageVoting = await dbContext.ImageVotings
                .Include(x => x.Entries.Where(e => e.UserId == userAccessor.User.Id))
                .FirstOrDefaultAsync(x => x.Id == request.Body.ImageVotingId, cancellationToken);

            if (imageVoting == null) throw new NotFoundException(nameof(ImageVoting), request.Body.ImageVotingId);

            if (!imageVoting.Active &&
                !permissionManager.CheckPermission(typeof(ImageVotingsPermissions.UploadImageVotingEntryImage)))
                throw new BadRequestException("A megadott szavazás nem aktív");

            if (userAccessor.User.UserGroups.Any(x => x.Id == imageVoting.BannedUserGroupId) ||
                userAccessor.User.UserGroups.All(x => x.Id != imageVoting.UploaderUserGroupId))
                throw new BadRequestException("Nem tölthetsz fel képet erre a szavazásra");

            var fileUploadCount = await dbContext.FileUploads.CountAsync(
                u => u.UserId == userAccessor.User.Id && u.Purpose == $"ImageVoting-{imageVoting.Id}",
                cancellationToken);

            if (fileUploadCount >= imageVoting.MaxUploadsPerUser)
                throw new BadRequestException("Elérted a maximális feltöltések számát");

            var fileUploadResult = await filesService.UploadFileAsync(request.Body.File, userAccessor.User.Id,
                $"ImageVoting-{imageVoting.Id}");

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = fileUploadResult.FileUpload.Adapt<Response>();
            response.Url = fileUploadResult.Url!;

            return response;
        }
    }
}