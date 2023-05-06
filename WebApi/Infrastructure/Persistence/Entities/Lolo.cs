using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sieve.Attributes;

namespace WebApi.Infrastructure.Persistence.Entities;

public class Lolo : TimestampedEntity
{
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Id { get; set; }

    [Required] public Guid UserId { get; set; }
    [Required] public User User { get; set; }

    //TODO: Add HistoryId

    [Sieve(CanFilter = true, CanSort = true)]
    public bool IsSpent { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Reason { get; set; }
}

public class LoloConfiguration : IEntityTypeConfiguration<Lolo>
{
    public void Configure(EntityTypeBuilder<Lolo> builder)
    {
        builder.Property(l => l.IsSpent).HasDefaultValue(false);
    }
}