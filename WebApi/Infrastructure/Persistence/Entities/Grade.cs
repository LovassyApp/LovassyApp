using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Common;

namespace WebApi.Infrastructure.Persistence.Entities;

[Index(nameof(Uid), IsUnique = true)]
public class Grade : TimestampedEntity, IHasDomainEvent
{
    [Key] public int Id { get; set; }

    [Required] public string UserIdHashed { get; set; }

    public string? LoloIdHashed { get; set; }

    [Required] public string Uid { get; set; }

    [Required] public string Subject { get; set; }
    [Required] public string SubjectCategory { get; set; }
    [Required] public string Teacher { get; set; }
    [Required] public string Group { get; set; }

    [Required] public int GradeValue { get; set; }
    [Required] public string TextGrade { get; set; }
    [Required] public string ShortTextGrade { get; set; }

    [Required] public int Weight { get; set; }

    [Required] public DateTime EvaluationDate { get; set; }
    [Required] public DateTime CreateDate { get; set; }

    [Required] public string Name { get; set; }
    [Required] public string Type { get; set; }
    [Required] public string GradeType { get; set; }

    public List<DomainEvent> DomainEvents { get; } = new();
}

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.Ignore(g => g.DomainEvents);
    }
}