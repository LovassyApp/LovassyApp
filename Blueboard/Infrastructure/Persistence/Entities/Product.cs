using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Blueboard.Infrastructure.Persistence.Entities.Owned;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class Product : TimestampedEntity
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
    public string RichTextContent { get; set; }

    public NpgsqlTsVector SearchVector { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public bool Visible { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public bool QRCodeActivated { get; set; }

    [JsonIgnore] public List<QRCode> QRCodes { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Price { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "jsonb")]
    public List<ProductInput> Inputs { get; set; }

    [Required] public string[] NotifiedEmails { get; set; }

    [Required] public string ThumbnailUrl { get; set; }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasGeneratedTsVectorColumn(p => p.SearchVector, "hungarian", p => new
        {
            p.Name, p.Description, p.RichTextContent
        }).HasIndex(p => p.SearchVector).HasMethod("GIN");
    }
}