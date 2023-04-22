using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Persistence.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(OmCodeHashed), IsUnique = true)]
[Index(nameof(IdSaltHashed), IsUnique = true)]
[Index(nameof(HasherSalt), IsUnique = true)]
public class User : BaseEntity
{
    [Key] public Guid Id { get; set; }

    [Required] [MaxLength(255)] public string Name { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string PasswordHashed { get; set; }

    [Required] public string PublicKey { get; set; }
    [Required] public string PrivateKeyEncrypted { get; set; }
    [Required] public string MasterKeyEncrypted { get; set; }
    [Required] public string ResetKeyEncrypted { get; set; }
    [Required] public string IdSaltEncrypted { get; set; }
    [Required] public string IdSaltHashed { get; set; }

    [Required] public string HasherSalt { get; set; }

    [Required] public string OmCodeEncrypted { get; set; }
    [Required] public string OmCodeHashed { get; set; }

    public string? RealName { get; set; }
    public string? Class { get; set; }

    [Required] public bool ImportAvailable { get; set; }

    public List<GradeImport> GradeImports { get; set; }
}