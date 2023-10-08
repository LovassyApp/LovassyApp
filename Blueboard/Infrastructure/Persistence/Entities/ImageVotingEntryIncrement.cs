using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class ImageVotingEntryIncrement : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string? AspectKey { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Increment { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int ImageVotingEntryId { get; set; }

    [JsonIgnore] public ImageVotingEntry ImageVotingEntry { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid UserId { get; set; }

    [JsonIgnore] public User User { get; set; }
}

public class ImageVotingEntryRatingConfiguration : IEntityTypeConfiguration<ImageVotingEntryIncrement>
{
    public void Configure(EntityTypeBuilder<ImageVotingEntryIncrement> builder)
    {
        builder.HasOne(i => i.ImageVotingEntry).WithMany(e => e.Increments).HasForeignKey(i => i.ImageVotingEntryId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(i => i.User).WithMany(u => u.ImageVotingEntryIncrements).HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}