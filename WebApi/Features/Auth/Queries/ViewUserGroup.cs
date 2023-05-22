using Helpers.Framework.Exceptions;
using Mapster;
using MediatR;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Queries;

public static class ViewUserGroup
{
    public class Query : IRequest<Response>
    {
        public int Id { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Permissions { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var userGroup = await _context.UserGroups.FindAsync(request.Id);

            if (userGroup == null) throw new NotFoundException(nameof(UserGroup), request.Id);

            return userGroup.Adapt<Response>();
        }
    }
}