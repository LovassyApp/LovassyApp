using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class StudentPartyCampaignPost : TimestampedEntity
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

    [JsonIgnore] public NpgsqlTsVector SearchVector { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int StudentPartyId { get; set; }

    [JsonIgnore] public StudentParty StudentParty { get; set; }
}

public class StudentPartyCampaignPostConfiguration : IEntityTypeConfiguration<StudentPartyCampaignPost>
{
    public void Configure(EntityTypeBuilder<StudentPartyCampaignPost> builder)
    {
        builder.HasGeneratedTsVectorColumn(p => p.SearchVector, "hungarian", p => new
        {
            p.Name, p.Description, p.RichTextContent
        }).HasIndex(p => p.SearchVector).HasMethod("GIN");
        builder.HasOne(r => r.StudentParty).WithMany(u => u.StudentPartyCampaignPosts)
            .HasForeignKey(r => r.StudentPartyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}