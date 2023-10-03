using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Features.ImageVotings.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.ImageVotings.Commands;

public static class CreateImageVotingEntry
{
    public class Command : IRequest<Response>
    {
        public RequestBody Body { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public Guid UserId { get; set; }
        public int ImageVotingId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class RequestBody
    {
        public int ImageVotingId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.ImageUrl).NotEmpty().MaximumLength(255);
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly PermissionManager _permissionManager;
        private readonly IPublisher _publisher;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext context, UserAccessor userAccessor, IPublisher publisher,
            PermissionManager permissionManager)
        {
            _context = context;
            _userAccessor = userAccessor;
            _publisher = publisher;
            _permissionManager = permissionManager;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var imageVoting = await _context.ImageVotings
                .Include(v => v.Entries.Where(e => e.UserId == _userAccessor.User.Id)).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Body.ImageVotingId, cancellationToken);

            if (imageVoting == null)
                throw new BadRequestException("A megadott szavazás nem létezik");

            if (!imageVoting.Active &&
                !_permissionManager.CheckPermission(typeof(ImageVotingsPermissions.CreateImageVotingEntry)))
                throw new BadRequestException("A megadott szavazás nem aktív");

            if (_userAccessor.User.UserGroups.Any(g => g.Id == imageVoting.BannedUserGroupId) ||
                _userAccessor.User.UserGroups.All(g => g.Id != imageVoting.UploaderUserGroupId))
                throw new BadRequestException("Nem tölthetsz fel képet erre a szavazásra");

            if (imageVoting.Entries.Count >= imageVoting.MaxUploadsPerUser)
                throw new BadRequestException("Elérted a maximális feltöltések számát");

            var imageVotingEntry = request.Body.Adapt<ImageVotingEntry>();

            imageVotingEntry.UserId = _userAccessor.User.Id;

            await _context.ImageVotingEntries.AddAsync(imageVotingEntry, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new ImageVotingEntriesUpdatedEvent(), cancellationToken);

            return imageVotingEntry.Adapt<Response>();
        }
    }
}