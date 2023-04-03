namespace WebApi.Contexts.Status.Models;

public class ServiceStatus
{
    public bool Database { get; set; }
    public bool Cache { get; set; }
    public bool Realtime { get; set; }
}