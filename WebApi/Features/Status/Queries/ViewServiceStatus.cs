using MediatR;
using WebApi.Core.Auth.Services;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Status.Queries;

public static class ViewServiceStatus
{
    public class Query : IRequest<Response>
    {
    }

    public class Response
    {
        public bool Ready { get; set; }
        public ResponseServiceStatus ServiceStatus { get; set; }
    }

    public class ResponseServiceStatus
    {
        public bool Database { get; set; }
        public bool Realtime { get; set; }
        public bool ResetKeyPassword { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly ResetService _resetService;

        public Handler(ApplicationDbContext context, ResetService resetService)
        {
            _context = context;
            _resetService = resetService;
        }

        public async Task<Response> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var status = new ResponseServiceStatus
            {
                Database = await _context.Database.CanConnectAsync(cancellationToken),
                ResetKeyPassword = _resetService.IsResetKeyPasswordSet(),
                Realtime = true
            };

            var response = new Response
            {
                Ready = status is { Database: true, Realtime: true },
                ServiceStatus = status
            };

            return response;
        }
    }
}