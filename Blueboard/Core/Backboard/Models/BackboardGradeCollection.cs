namespace Blueboard.Core.Backboard.Models;

/// <summary>
///     The model representing a collection of grades from Backboard, which have not yet been imported belonging to a
///     single <see cref="BackboardUser" />.
/// </summary>
public class BackboardGradeCollection
{
    public string? SchoolClass { get; set; }
    public string StudentName { get; set; }
    public BackboardUser User { get; set; }
    public List<BackboardGrade> Grades { get; set; }
}