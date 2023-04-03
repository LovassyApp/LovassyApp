namespace WebApi.Contexts.Status.Models;

public class VersionResponse
{
    public string WhoAmI { get; set; }
    public string Version { get; set; }
    public string DotNetVersion { get; set; }
    public List<string> Contributors { get; set; }
    public string Repository { get; set; }
    public string? MOTD { get; set; }
}