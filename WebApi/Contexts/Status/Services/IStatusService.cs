using WebApi.Contexts.Status.Models;

namespace WebApi.Contexts.Status.Services;

public interface IStatusService
{
    public string WhoAmI { get; }
    public string Version { get; }
    public string DotNetVersion { get; }
    public List<string> Contributors { get; }
    public string Repository { get; }
    public List<string> MOTDs { get; }

    public ServiceStatus GetServiceStatus();
    public bool IsReady();
}