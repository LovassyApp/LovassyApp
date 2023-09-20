using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class ImageVotingEntryRating : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string? AspectKey { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Rating { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int ImageVotingEntryId { get; set; }

    [JsonIgnore] public ImageVotingEntry ImageVotingEntry { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid UserId { get; set; }

    [JsonIgnore] public User User { get; set; }
}

public class ImageVotingEntryRatingConfiguration : IEntityTypeConfiguration<ImageVotingEntryRating>
{
    public void Configure(EntityTypeBuilder<ImageVotingEntryRating> builder)
    {
        builder.HasOne(i => i.ImageVotingEntry).WithMany(e => e.Ratings).HasForeignKey(i => i.ImageVotingEntryId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(i => i.User).WithMany(u => u.ImageVotingEntryRatings).HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}