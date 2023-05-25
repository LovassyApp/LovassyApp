using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Infrastructure.Persistence.Entities;

public class GradeImport : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Required] public Guid UserId { get; set; }
    [JsonIgnore] public User User { get; set; }

    [Required] public string JsonEncrypted { get; set; }
}

public class GradeImportConfiguration : IEntityTypeConfiguration<GradeImport>
{
    public void Configure(EntityTypeBuilder<GradeImport> builder)
    {
        builder.HasOne(i => i.User).WithMany(u => u.GradeImports).HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}