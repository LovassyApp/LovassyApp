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

        public bool CanUpload { get; set; }
        public bool? CanChoose { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseImageVotingAspect
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool CanChoose { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
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

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var imageVoting = await _context.ImageVotings
                .Include(v => v.Entries.Where(e => e.UserId == _userAccessor.User.Id))
                .Include(v => v.Choices.Where(e => e.UserId == _userAccessor.User.Id))
                .Include(x => x.UploaderUserGroup)
                .Include(x => x.BannedUserGroup)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (imageVoting == null || (!imageVoting.Active &&
                                        !_permissionManager.CheckPermission(
                                            typeof(ImageVotingsPermissions.ViewImageVoting))))
                throw new NotFoundException(nameof(ImageVoting), request.Id);

            var response = imageVoting.Adapt<Response>();

            if (imageVoting.Aspects.Count == 0)
                response.CanChoose = imageVoting.Choices.Count == 0 && imageVoting.Active &&
                                     imageVoting is { Type: ImageVotingType.SingleChoice };
            else
                foreach (var aspect in response.Aspects)
                {
                    var choice = imageVoting.Choices.FirstOrDefault(c => c.AspectKey == aspect.Key);
                    aspect.CanChoose = choice == null && imageVoting is
                        { Active: true, Type: ImageVotingType.SingleChoice };
                }


            var inUploaderUserGroup =
                _userAccessor.User.UserGroups.Any(g => g.Id == imageVoting.UploaderUserGroupId) &&
                _userAccessor.User.UserGroups.All(g => g.Id != imageVoting.BannedUserGroupId);

            response.CanUpload = imageVoting.Active && inUploaderUserGroup &&
                                 imageVoting.Entries.Count < imageVoting.MaxUploadsPerUser;

            return response;
        }
    }
}