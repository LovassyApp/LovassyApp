using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Sieve.Attributes;

namespace WebApi.Infrastructure.Persistence.Entities;

[Index(nameof(Secret), IsUnique = true)]
public class QRCode : TimestampedEntity
{
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Id { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; }

    [Required] public string Secret { get; set; }

    [JsonIgnore] public List<Product> Products { get; set; }
}