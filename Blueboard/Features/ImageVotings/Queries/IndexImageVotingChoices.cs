using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.ImageVotings.Queries;

public static class IndexImageVotingChoices
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string? AspectKey { get; set; }

        public int ImageVotingId { get; set; }
        public ResponseImageVotingEntry ImageVotingEntry { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseImageVotingEntry
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, SieveProcessor sieveProcessor,
            PermissionManager permissionManager)
        : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var onlyActive =
                !permissionManager.CheckPermission(typeof(ImageVotingsPermissions.IndexImageVotingChoices));
            var imageVotingChoices = context.ImageVotingChoices.Include(c => c.ImageVoting).AsSplitQuery()
                .Include(c => c.ImageVotingEntry).AsSplitQuery()
                .Where(c => !onlyActive || c.ImageVoting.Active)
                .AsNoTracking(); // We want to avoid a cartesian explosion so we need to split the queries

            var filteredImageVotingChoices = sieveProcessor.Apply(request.SieveModel, imageVotingChoices);

            return (await filteredImageVotingChoices.ToListAsync(cancellationToken)).Adapt<IEnumerable<Response>>();
        }
    }
}