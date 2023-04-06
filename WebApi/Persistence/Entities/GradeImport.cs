using System.ComponentModel.DataAnnotations;

namespace WebApi.Persistence.Entities;

public class GradeImport : BaseEntity
{
    [Key] public int Id { get; set; }

    [Required] public Guid UserId { get; set; }
    [Required] public User User { get; set; }

    [Required] public string EncryptionKey { get; set; }
    [Required] public string JsonEncrypted { get; set; }
}