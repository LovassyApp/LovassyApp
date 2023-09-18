using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.StudentParites.Queries;

public static class ViewStudentParty
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
        public string RichTextContent { get; set; }

        public List<ResponsePartyMember> Members { get; set; }

        public string ProgramPlanUrl { get; set; }
        public string VideoUrl { get; set; }
        public string PosterUrl { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public Guid UserId { get; set; }
    }

    public class ResponsePartyMember
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string? Role { get; set; }
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
            var studentParty = await _context.StudentParties
                .Where(p => p.Id == request.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (studentParty == null)
                throw new NotFoundException(nameof(StudentParty), request.Id);

            if (!_permissionManager.CheckPermission(typeof(StudentPartiesPermissions.ViewStudentParty)))
                if (studentParty.ApprovedAt == null)
                    throw new NotFoundException(nameof(StudentParty), request.Id);

            return studentParty.Adapt<Response>();
        }
    }
}