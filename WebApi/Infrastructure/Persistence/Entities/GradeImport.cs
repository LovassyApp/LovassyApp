using System.ComponentModel.DataAnnotations;

namespace WebApi.Infrastructure.Persistence.Entities;

public class GradeImport : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Required] public Guid UserId { get; set; }
    [Required] public User User { get; set; }

    [Required] public string KeyEncrypeted { get; set; }
    [Required] public string JsonEncrypted { get; set; }
}