namespace WebApi.Contexts.Import.Models;

public class IndexUsersResponse
{
    public Guid Id { get; set; }
    public string OmCodeHashed { get; set; }
    public string PublicKey { get; set; }
}