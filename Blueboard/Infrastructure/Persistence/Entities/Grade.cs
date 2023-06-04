using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

[Index(nameof(Uid), IsUnique = true)]
public class Grade : TimestampedEntity
{
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Id { get; set; }

    [Required] public string UserIdHashed { get; set; }

    public string? LoloIdHashed { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Uid { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Subject { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string SubjectCategory { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Teacher { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Group { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int GradeValue { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string TextGrade { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string ShortTextGrade { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public int Weight { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime EvaluationDate { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime CreateDate { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Type { get; set; }

    [Required] public GradeType GradeType { get; set; }
}

public enum GradeType
{
    RegularGrade,
    BehaviourGrade,
    DiligenceGrade
}