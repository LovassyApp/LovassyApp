using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Sieve.Attributes;

namespace WebApi.Infrastructure.Persistence.Entities;

public class LoloRequest : TimestampedEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    [Key]
    public int Id { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    [Required]
    public string Title { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    [Required]
    public string Body { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    [Required]
    public Guid UserId { get; set; }

    [JsonIgnore] public User User { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime? AcceptedAt { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime? DeniedAt { get; set; }
}