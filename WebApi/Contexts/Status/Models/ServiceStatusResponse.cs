namespace WebApi.Contexts.Status.Models;

public class ServiceStatusResponse
{
    public bool Ready { get; set; }
    public ServiceStatus ServiceStatus { get; set; }
}