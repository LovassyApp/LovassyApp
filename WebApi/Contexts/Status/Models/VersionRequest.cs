namespace WebApi.Contexts.Status.Models;

public class VersionRequest
{
    public bool SendOk { get; set; } = true;
    public bool SendMOTD { get; set; } = true;
}