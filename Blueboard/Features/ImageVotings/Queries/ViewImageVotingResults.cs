using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.ImageVotings.Queries;

public static class ViewImageVotingResults
{
    public class Query : IRequest<Response>
    {
        public int Id { get; set; }
    }

    public class Response
    {
        public int ChooserCount { get; set; }
        public int IncrementerCount { get; set; }
        public int UploaderCount { get; set; }

        public List<ResponseEntry> Entries { get; set; }
    }

    public class ResponseEntry
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public int ChoicesCount { get; set; }
        public int IncrementSum { get; set; }

        public List<ResponseEntryAspect> Aspects { get; set; }
    }

    public class ResponseEntryAspect
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ChoicesCount { get; set; }
        public int IncrementSum { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly PermissionManager _permissionManager;

        public Handler(ApplicationDbContext context, PermissionManager permissionManager)
        {
            _context = context;
            _permissionManager = permissionManager;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var imageVoting = await _context.ImageVotings
                .Include(v => v.Entries)
                .ThenInclude(e => e.Increments)
                .Include(v => v.Choices)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            if (imageVoting == null)
                throw new NotFoundException(nameof(ImageVoting), request.Id);

            if (!imageVoting.Active &&
                !_permissionManager.CheckPermission(typeof(ImageVotingsPermissions.ViewImageVotingResults)))
                throw new NotFoundException(nameof(ImageVoting), request.Id);

            var entries = imageVoting.Entries.Adapt<List<ResponseEntry>>();

            if (imageVoting.Aspects.Count > 0)
                foreach (var entry in entries)
                {
                    entry.Aspects = imageVoting.Aspects.Adapt<List<ResponseEntryAspect>>();

                    foreach (var aspect in entry.Aspects)
                    {
                        aspect.ChoicesCount = imageVoting.Choices.Count(c =>
                            c.AspectKey == aspect.Key && c.ImageVotingEntryId == entry.Id);
                        aspect.IncrementSum = imageVoting.Entries
                            .Where(e => e.Id == entry.Id)
                            .SelectMany(e => e.Increments)
                            .Where(i => i.AspectKey == aspect.Key)
                            .Sum(i => i.Increment);
                    }

                    entry.ChoicesCount = entry.Aspects.Sum(a => a.ChoicesCount);
                    entry.IncrementSum = entry.Aspects.Sum(a => a.IncrementSum);
                }
            else
                foreach (var entry in entries)
                {
                    entry.ChoicesCount = imageVoting.Choices.Count(c => c.ImageVotingEntryId == entry.Id);
                    entry.IncrementSum = imageVoting.Entries
                        .Where(e => e.Id == entry.Id)
                        .SelectMany(e => e.Increments)
                        .Sum(i => i.Increment);
                }

            return new Response
            {
                ChooserCount = imageVoting.Choices.DistinctBy(c => c.UserId).Count(),
                UploaderCount = imageVoting.Entries.DistinctBy(e => e.UserId).Count(),
                IncrementerCount = imageVoting.Entries.SelectMany(e => e.Increments).DistinctBy(i => i.UserId).Count(),
                Entries = entries
            };
        }
    }
}