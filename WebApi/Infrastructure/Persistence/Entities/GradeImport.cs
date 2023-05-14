using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Infrastructure.Persistence.Entities;

public class GradeImport : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Required] public Guid UserId { get; set; }
    [JsonIgnore] public User User { get; set; }

    [Required] public string JsonEncrypted { get; set; }
}