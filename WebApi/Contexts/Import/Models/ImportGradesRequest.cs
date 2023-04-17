using System.ComponentModel.DataAnnotations;

namespace WebApi.Contexts.Import.Models;

public class ImportGradesRequest
{
    [Required] public string KeyEncrypeted { get; set; }
    [Required] public string JsonEncrypted { get; set; }
}