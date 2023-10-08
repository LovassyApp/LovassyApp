using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Blueboard.Infrastructure.Persistence.Entities.Owned;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class ImageVoting : TimestampedEntity
{
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Id { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Description { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public ImageVotingType Type { get; set; }

    [Required]
    [Column(TypeName = "jsonb")]
    public List<ImageVotingAspect> Aspects { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public bool Active { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public bool ShowUploaderInfo { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int UploaderUserGroupId { get; set; }

    [JsonIgnore] public UserGroup UploaderUserGroup { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public int? BannedUserGroupId { get; set; }

    [JsonIgnore] public UserGroup? BannedUserGroup { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int MaxUploadsPerUser { get; set; }

    [JsonIgnore] public List<ImageVotingEntry> Entries { get; set; }

    [JsonIgnore] public List<ImageVotingChoice> Choices { get; set; }
}

public enum ImageVotingType
{
    SingleChoice,
    Increment
}

public class ImageVotingConfiguration : IEntityTypeConfiguration<ImageVoting>
{
    public void Configure(EntityTypeBuilder<ImageVoting> builder)
    {
        builder.HasOne(v => v.UploaderUserGroup).WithMany(g => g.UploadableImageVotings)
            .HasForeignKey(v => v.UploaderUserGroupId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(v => v.BannedUserGroup).WithMany(g => g.BannedImageVotings)
            .HasForeignKey(v => v.BannedUserGroupId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}