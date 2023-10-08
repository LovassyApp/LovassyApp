using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class FileUpload : TimestampedEntity
{
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Id { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Filename { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string OriginalFilename { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string MimeType { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Path { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Purpose { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid UserId { get; set; }

    [JsonIgnore] public User User { get; set; }
}

public class FileUploadConfiguration : IEntityTypeConfiguration<FileUpload>
{
    public void Configure(EntityTypeBuilder<FileUpload> builder)
    {
        builder.HasOne(i => i.User).WithMany(u => u.FileUploads).HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}