namespace Blueboard.Core.Backboard.Models;

/// <summary>
///     The model representing a single grade from Backboard, that has not yet been imported.
/// </summary>
public class BackboardGrade
{
    public string SubjectCategory { get; set; }
    public string Subject { get; set; }
    public string Theme { get; set; }
    public string Group { get; set; }
    public string? Teacher { get; set; }
    public string? Type { get; set; }
    public string TextGrade { get; set; }
    public string? Grade { get; set; }
    public string ShortTextGrade { get; set; }
    public string BehaviorGrade { get; set; }
    public string DiligenceGrade { get; set; }
    public string CreateDate { get; set; }
    public string RecordDate { get; set; }
    public string StudentName { get; set; }
}