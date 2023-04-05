namespace WebApi.Contexts.Import.Models;

public class CreateImportKeyResponse
{
    public string Name { get; set; }
    public bool Enabled { get; set; }

    public string Key { get; set; }
}