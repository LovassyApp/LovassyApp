using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.ImageVotings.Queries;

public static class ViewImageVoting
{
    public class Query : IRequest<Response>
    {
        public int Id { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Type { get; set; }

        public List<ResponseImageVotingAspect> Aspects { get; set; }

        public bool Active { get; set; }
        public bool ShowUploaderInfo { get; set; }

        public int UploaderUserGroupId { get; set; }
        public int? BannedUserGroupId { get; set; }

        public int MaxUploadsPerUser { get; set; }

        public bool SuperIncrementAllowed { get; set; }
        public int SuperIncrementValue { get; set; }

        public bool CanUpload { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseImageVotingAspect
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool? CanChoose { get; set; }
        public int? ChosenEntryId { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, UserAccessor userAccessor,
            PermissionManager permissionManager)
        : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var imageVoting = await context.ImageVotings
                .Include(v => v.Entries.Where(e => e.UserId == userAccessor.User.Id))
                .Include(v => v.Choices.Where(e => e.UserId == userAccessor.User.Id))
                .Include(x => x.UploaderUserGroup)
                .Include(x => x.BannedUserGroup)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (imageVoting == null || (!imageVoting.Active &&
                                        !permissionManager.CheckPermission(
                                            typeof(ImageVotingsPermissions.ViewImageVoting))))
                throw new NotFoundException(nameof(ImageVoting), request.Id);

            var response = imageVoting.Adapt<Response>();

            foreach (var aspect in response.Aspects)
            {
                var choice = imageVoting.Choices.FirstOrDefault(c => c.AspectKey == aspect.Key);
                aspect.CanChoose = imageVoting.Type == ImageVotingType.SingleChoice &&
                                   (imageVoting.Active ||
                                    permissionManager.CheckPermission(
                                        typeof(ImageVotingsPermissions.ChooseImageVotingEntry)));
                aspect.ChosenEntryId = choice?.ImageVotingEntryId;
            }


            var inUploaderUserGroup =
                userAccessor.User.UserGroups.Any(g => g.Id == imageVoting.UploaderUserGroupId) &&
                userAccessor.User.UserGroups.All(g => g.Id != imageVoting.BannedUserGroupId);

            response.CanUpload =
                (permissionManager.CheckPermission(typeof(ImageVotingsPermissions.CreateImageVotingEntry)) ||
                 (imageVoting.Active &&
                  permissionManager.CheckPermission(typeof(ImageVotingsPermissions.CreateActiveImageVotingEntry)))) &&
                inUploaderUserGroup &&
                imageVoting.Entries.Count < imageVoting.MaxUploadsPerUser;

            return response;
        }
    }
}