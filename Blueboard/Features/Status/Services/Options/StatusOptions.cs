namespace Blueboard.Features.Status.Services.Options;

public class StatusOptions
{
    public string WhoAmI { get; set; }
    public string Version { get; set; }
    public List<string> Contributors { get; set; }
    public string Repository { get; set; }
    public List<string> MOTDs { get; set; }
}