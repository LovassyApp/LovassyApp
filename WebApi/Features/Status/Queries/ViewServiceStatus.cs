using MediatR;
using WebApi.Core.Cryptography.Services;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Status.Queries;

public class ViewServiceStatusQuery : IRequest<ViewServiceStatusResponse>
{
}

public class ViewServiceStatusResponse
{
    public bool Ready { get; set; }
    public ServiceStatus ServiceStatus { get; set; }
}

public class ServiceStatus
{
    public bool Database { get; set; }
    public bool Realtime { get; set; }
    public bool ResetKeyPassword { get; set; }
}

internal sealed class ViewServiceStatusQueryHandler : IRequestHandler<ViewServiceStatusQuery, ViewServiceStatusResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly ResetService _resetService;

    public ViewServiceStatusQueryHandler(ApplicationDbContext context, ResetService resetService)
    {
        _context = context;
        _resetService = resetService;
    }

    public async Task<ViewServiceStatusResponse> Handle(ViewServiceStatusQuery request,
        CancellationToken cancellationToken)
    {
        var status = new ServiceStatus
        {
            Database = _context.Database.CanConnect(),
            ResetKeyPassword = _resetService.IsResetKeyPasswordSet(),
            Realtime = true
        };

        var response = new ViewServiceStatusResponse
        {
            Ready = status is { Database: true, Realtime: true },
            ServiceStatus = status
        };

        return response;
    }
}