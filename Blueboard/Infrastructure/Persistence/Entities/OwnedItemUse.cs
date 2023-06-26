using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class OwnedItemUse : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Required]
    [Column(TypeName = "jsonb")]
    public Dictionary<string, string> Values { get; set; } //Input values

    [Required] public int OwnedItemId { get; set; }

    [JsonIgnore] public OwnedItem OwnedItem { get; set; }
}

public class OwnedItemUseConfiguration : IEntityTypeConfiguration<OwnedItemUse>
{
    public void Configure(EntityTypeBuilder<OwnedItemUse> builder)
    {
        builder.HasOne(u => u.OwnedItem).WithMany(i => i.OwnedItemUses).HasForeignKey(u => u.OwnedItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}