using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

[Index(nameof(Email), IsUnique = true)]
public class LoloRequestCreatedNotifier : TimestampedEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    [Key]
    public int Id { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    [Required]
    public string Email { get; set; }
}