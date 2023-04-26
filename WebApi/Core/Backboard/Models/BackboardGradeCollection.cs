namespace WebApi.Core.Backboard.Models;

public class BackboardGradeCollection
{
    public string SchoolClass { get; set; }
    public string StudentName { get; set; }
    public BackboardUser User { get; set; }
    public List<BackboardGrade> Grades { get; set; }
}