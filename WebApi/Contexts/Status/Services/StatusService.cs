using Microsoft.Extensions.Options;
using WebApi.Contexts.Status.Models;
using WebApi.Contexts.Status.Services.Options;
using WebApi.Helpers.Configuration.Exceptions;
using WebApi.Helpers.Cryptography.Services;
using WebApi.Persistence;

namespace WebApi.Contexts.Status.Services;

public class StatusService
{
    private readonly ApplicationDbContext _context;
    private readonly ResetService _resetService;

    public StatusService(IOptions<StatusOptions> options, ApplicationDbContext context, ResetService resetService)
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
        _resetService = resetService;
    }

    public string WhoAmI { get; }
    public string Version { get; }
    public string DotNetVersion { get; }
    public List<string> Contributors { get; }
    public string Repository { get; }
    public List<string> MOTDs { get; }

    /// <summary>
    ///     Shows a detailed summary of the service status.
    /// </summary>
    /// <returns>The service status broken into parts.</returns>
    public ServiceStatus GetServiceStatus()
    {
        return new ServiceStatus
        {
            Database = _context.Database.CanConnect(),
            ResetKeyPassword = _resetService.IsResetKeyPasswordSet(),
            Realtime = true
        };
    }

    /// <summary>
    ///     Returns weather the service is ready to be used. True does not mean that every part of the service is functional.
    /// </summary>
    /// <returns>Whether the app is operational or not.</returns>
    public bool IsReady()
    {
        var serviceStatus = GetServiceStatus();

        return serviceStatus is { Database: true, Realtime: true };
    }
}