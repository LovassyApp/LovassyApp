using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.ImageVotings.Queries;

public static class IndexImageVotings
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Type { get; set; }

        public bool Active { get; set; }
        public bool ShowUploaderInfo { get; set; }

        public int UploaderUserGroupId { get; set; }
        public int? BannedUserGroupId { get; set; }

        public int MaxUploadsPerUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, PermissionManager permissionManager,
            SieveProcessor sieveProcessor)
        : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var onlyActive = !permissionManager.CheckPermission(typeof(ImageVotingsPermissions.IndexImageVotings));
            var imageVotings = context.ImageVotings
                .Where(x => !onlyActive || x.Active)
                .AsNoTracking();

            var filteredImageVotings = sieveProcessor.Apply(request.SieveModel, imageVotings);

            return (await filteredImageVotings.ToListAsync(cancellationToken)).Adapt<IEnumerable<Response>>();
        }
    }
}