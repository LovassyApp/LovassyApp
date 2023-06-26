using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class OwnedItem : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Required] public Guid UserId { get; set; }

    [JsonIgnore] public User User { get; set; }

    [Required] public int ProductId { get; set; }

    [JsonIgnore] public Product Product { get; set; }

    [JsonIgnore] public StoreHistory StoreHistory { get; set; }

    public DateTime? UsedAt { get; set; }

    [JsonIgnore] public List<OwnedItemUse> OwnedItemUses { get; set; }
}

public class OwnedProductConfiguration : IEntityTypeConfiguration<OwnedItem>
{
    public void Configure(EntityTypeBuilder<OwnedItem> builder)
    {
        builder.HasOne(p => p.Product).WithMany(p => p.OwnedItems).HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(p => p.User).WithMany(u => u.OwnedItems).HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}