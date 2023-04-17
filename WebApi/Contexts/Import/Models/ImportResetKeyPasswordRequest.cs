using System.ComponentModel.DataAnnotations;

namespace WebApi.Contexts.Import.Models;

public class ImportResetKeyPasswordRequest
{
    [Required] public string ResetKeyPassword { get; set; }
}