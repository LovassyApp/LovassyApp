namespace WebApi.Core.Backboard.Models;

public class BackboardUser
{
    public Guid Id { get; set; }
    public string PublicKey { get; set; }
    public string OmCodeHashed { get; set; }
}