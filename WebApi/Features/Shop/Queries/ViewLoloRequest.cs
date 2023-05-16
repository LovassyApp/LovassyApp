using Mapster;
using MediatR;
using WebApi.Common.Exceptions;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Shop.Queries;

public static class ViewLoloRequest
{
    public class Query : IRequest<Response>
    {
        public int Id { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }

        public Guid UserId { get; set; }

        public DateTime? AcceptedAt { get; set; }
        public DateTime? DeniedAt { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var loloRequest = _context.LoloRequests.Find(request.Id);

            if (loloRequest is null)
                throw new NotFoundException(nameof(LoloRequest), request.Id);

            return Task.FromResult(loloRequest.Adapt<Response>());
        }
    }
}