using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class StoreHistory : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Required] public Guid UserId { get; set; }
    [JsonIgnore] public User User { get; set; }

    [Required] public int ProductId { get; set; }
    [JsonIgnore] public Product Product { get; set; }

    public int? OwnedItemId { get; set; }
    [JsonIgnore] public OwnedItem OwnedItem { get; set; }

    [Required] public string Reason { get; set; }
}

public class StoreHistoryConfiguration : IEntityTypeConfiguration<StoreHistory>
{
    public void Configure(EntityTypeBuilder<StoreHistory> builder)
    {
        builder.HasOne(h => h.Product).WithMany(p => p.StoreHistories).HasForeignKey(h => h.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(h => h.User).WithMany(u => u.StoreHistories).HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(h => h.OwnedItem).WithOne(p => p.StoreHistory)
            .HasForeignKey<StoreHistory>(h => h.OwnedItemId).OnDelete(DeleteBehavior.SetNull);
    }
}