using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using MediatR;

namespace Blueboard.Features.Status.Queries;

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

    internal sealed class Handler(ApplicationDbContext context, ResetService resetService) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var status = new ResponseServiceStatus
            {
                Database = await context.Database.CanConnectAsync(cancellationToken),
                ResetKeyPassword = resetService.IsResetKeyPasswordSet(),
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