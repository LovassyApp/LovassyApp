using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Auth.Queries;

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

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var userGroup = await context.UserGroups.Where(g => g.Id == request.Id).AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (userGroup == null) throw new NotFoundException(nameof(UserGroup), request.Id);

            return userGroup.Adapt<Response>();
        }
    }
}