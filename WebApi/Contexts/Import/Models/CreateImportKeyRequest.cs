using System.ComponentModel.DataAnnotations;

namespace WebApi.Contexts.Import.Models;

public class CreateImportKeyRequest
{
    [Required] public string Name { get; set; }
    [Required] public bool Enabled { get; set; }
}