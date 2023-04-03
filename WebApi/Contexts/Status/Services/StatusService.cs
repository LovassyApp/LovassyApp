using Microsoft.Extensions.Options;
using WebApi.Contexts.Status.Models;
using WebApi.Contexts.Status.Services.Options;
using WebApi.Helpers.Configuration.Exceptions;
using WebApi.Persistence;

namespace WebApi.Contexts.Status.Services;

public class StatusService : IStatusService
{
    private readonly ApplicationDbContext _context;

    public StatusService(IOptions<StatusOptions> options, ApplicationDbContext context)
    {
        WhoAmI = options.Value.WhoAmI ??
                 throw new ConfigurationMissingException(nameof(options.Value.WhoAmI), "Status");
        Version = options.Value.Version ??
                  throw new ConfigurationMissingException(nameof(options.Value.Version), "Status");
        Contributors = options.Value.Contributors ??
                       throw new ConfigurationMissingException(nameof(options.Value.Contributors), "Status");
        Repository = options.Value.Repository ??
                     throw new ConfigurationMissingException(nameof(options.Value.Repository), "Status");
        MOTDs = options.Value.MOTDs ?? throw new ConfigurationMissingException(nameof(options.Value.MOTDs), "Status");

        DotNetVersion = Environment.Version.ToString();

        _context = context;
    }

    public string WhoAmI { get; }
    public string Version { get; }
    public string DotNetVersion { get; }
    public List<string> Contributors { get; }
    public string Repository { get; }
    public List<string> MOTDs { get; }

    public ServiceStatus GetServiceStatus()
    {
        return new ServiceStatus
        {
            Database = _context.Database.CanConnect(),
            Cache = true,
            Realtime = true
        };
    }

    public bool IsReady()
    {
        var serviceStatus = GetServiceStatus();

        return serviceStatus is { Database: true, Cache: true, Realtime: true };
    }
}