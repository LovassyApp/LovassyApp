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
        public int ImageVotingId { get; set; }
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
        public IFormFile File { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .AllowedMimeTypes(new[] { "image/bmp", "image/jpeg", "image/x-png", "image/png", "image/gif" })
                .MaxFileSize(4 * 1024 * 1024);
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly FilesService _filesService;
        private readonly PermissionManager _permissionManager;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext dbContext, UserAccessor userAccessor, FilesService filesService,
            PermissionManager permissionManager)
        {
            _dbContext = dbContext;
            _userAccessor = userAccessor;
            _filesService = filesService;
            _permissionManager = permissionManager;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var imageVoting = await _dbContext.ImageVotings
                .Include(x => x.Entries.Where(e => e.UserId == _userAccessor.User.Id))
                .FirstOrDefaultAsync(x => x.Id == request.ImageVotingId, cancellationToken);

            if (imageVoting == null) throw new NotFoundException(nameof(ImageVoting), request.ImageVotingId);

            if (!imageVoting.Active &&
                !_permissionManager.CheckPermission(typeof(ImageVotingsPermissions.UploadImageVotingEntryImage)))
                throw new BadRequestException("A megadott szavazás nem aktív");

            if (_userAccessor.User.UserGroups.Any(x => x.Id == imageVoting.BannedUserGroupId) ||
                _userAccessor.User.UserGroups.All(x => x.Id != imageVoting.UploaderUserGroupId))
                throw new BadRequestException("Nem tölthetsz fel képet erre a szavazásra");

            var fileUploadCount = await _dbContext.FileUploads.CountAsync(
                u => u.UserId == _userAccessor.User.Id && u.Purpose == $"ImageVoting-{imageVoting.Id}",
                cancellationToken);

            if (fileUploadCount >= imageVoting.MaxUploadsPerUser)
                throw new BadRequestException("Elérted a maximális feltöltések számát");

            var fileUploadResult = await _filesService.UploadFileAsync(request.Body.File, _userAccessor.User.Id,
                $"ImageVoting-{imageVoting.Id}");

            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = fileUploadResult.FileUpload.Adapt<Response>();
            response.Url = fileUploadResult.Url!;

            return response;
        }
    }
}