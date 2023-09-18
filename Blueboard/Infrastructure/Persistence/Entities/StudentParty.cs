using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Blueboard.Infrastructure.Persistence.Entities.Owned;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class StudentParty : TimestampedEntity
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
    [Column(TypeName = "jsonb")]
    public List<StudentPartyMember> Members { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string ProgramPlanUrl { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string VideoUrl { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string PosterUrl { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime ApprovedAt { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid UserId { get; set; }

    [JsonIgnore] public User User { get; set; }

    [JsonIgnore] public List<StudentPartyCampaignPost> StudentPartyCampaignPosts { get; set; }
}

public class StudentPartyConfiguration : IEntityTypeConfiguration<StudentParty>
{
    public void Configure(EntityTypeBuilder<StudentParty> builder)
    {
        builder.HasGeneratedTsVectorColumn(p => p.SearchVector, "hungarian", p => new
        {
            p.Name, p.Description, p.RichTextContent
        }).HasIndex(p => p.SearchVector).HasMethod("GIN");
        builder.HasOne(r => r.User).WithMany(u => u.StudentParties).HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}