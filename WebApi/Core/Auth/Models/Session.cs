namespace WebApi.Core.Auth.Models;

public class Session
{
    public Dictionary<string, string> Data { get; set; }

    public string Hash { get; set; }
    public string Salt { get; set; }

    public DateTime Expiry { get; set; }
}