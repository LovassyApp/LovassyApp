using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Persistence.Entities;

[Index(nameof(KeyHashed), IsUnique = true)]
public class ImportKey : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string KeyProtected { get; set; }
    [Required] public string KeyHashed { get; set; }
    [Required] public bool Enabled { get; set; }
}