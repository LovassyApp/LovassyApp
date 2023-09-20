using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class ImageVotingChoice : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int ImageVotingId { get; set; }

    [JsonIgnore] public ImageVoting ImageVoting { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int ImageVotingEntryId { get; set; }

    [JsonIgnore] public ImageVotingEntry ImageVotingEntry { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid UserId { get; set; }

    [JsonIgnore] public User User { get; set; }
}

public class ImageVotingChoiceConfiguration : IEntityTypeConfiguration<ImageVotingChoice>
{
    public void Configure(EntityTypeBuilder<ImageVotingChoice> builder)
    {
        builder.HasOne(i => i.ImageVoting).WithMany(v => v.Choices).HasForeignKey(i => i.ImageVotingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(i => i.ImageVotingEntry).WithMany(e => e.Choices).HasForeignKey(i => i.ImageVotingEntryId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(i => i.User).WithMany(u => u.ImageVotingChoices).HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}