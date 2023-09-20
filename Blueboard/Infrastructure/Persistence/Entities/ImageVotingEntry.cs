using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class ImageVotingEntry : TimestampedEntity
{
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Id { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Title { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string ImageUrl { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid UserId { get; set; }

    [JsonIgnore] public User User { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int ImageVotingId { get; set; }

    [JsonIgnore] public ImageVoting ImageVoting { get; set; }

    [JsonIgnore] public List<ImageVotingEntryRating> Ratings { get; set; }

    [JsonIgnore] public List<ImageVotingChoice> Choices { get; set; }
}

public class ImageVotingEntryConfiguration : IEntityTypeConfiguration<ImageVotingEntry>
{
    public void Configure(EntityTypeBuilder<ImageVotingEntry> builder)
    {
        builder.HasOne(i => i.User).WithMany(u => u.ImageVotingEntries).HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(i => i.ImageVoting).WithMany(v => v.Entries).HasForeignKey(i => i.ImageVotingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}