using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

[Index(nameof(KeyHashed), IsUnique = true)]
public class ImportKey : TimestampedEntity
{
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Id { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; }

    [Required] public string KeyProtected { get; set; }
    [Required] public string KeyHashed { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public bool Enabled { get; set; }
}