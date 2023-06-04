using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class Lolo : TimestampedEntity
{
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Id { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    [Required]
    public Guid UserId { get; set; }

    [JsonIgnore] public User User { get; set; }

    //TODO: Add HistoryId

    [Sieve(CanFilter = true, CanSort = true)]
    public bool IsSpent { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public LoloType LoloType { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Reason { get; set; }

    public List<Grade>? Grades { get; set; } // This is not a navigation property, it has to be populated manually
}

public enum LoloType
{
    FromGrades,
    FromRequest
}

public class LoloConfiguration : IEntityTypeConfiguration<Lolo>
{
    public void Configure(EntityTypeBuilder<Lolo> builder)
    {
        builder.HasOne(l => l.User).WithMany(u => u.Lolos).HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(l => l.IsSpent).HasDefaultValue(false);
        builder.Ignore(l => l.Grades);
    }
}