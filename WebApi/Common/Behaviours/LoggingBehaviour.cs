using MediatR.Pipeline;

namespace WebApi.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        return Task.Run(() => _logger.LogInformation("Incoming request: {Name} {@Request}", requestName, request),
            cancellationToken);
    }
}