namespace WebApi.Helpers.Backboard;

public class KretaGrade
{
    private ILookup<string, int> _textGradeLookup = new Dictionary<string, int>
    {
        ["peldas"] = 5,
        ["jo"] = 4,
        ["valtozo"] = 3,
        ["rossz"] = 2,
        ["hanyag"] = 1
    }.ToLookup(dict => dict.Key, dict => dict.Value);

    public string Uid { get; set; }
    public string Subject { get; set; }
    public string SubjectCategory { get; set; }
    public string Teacher { get; set; }
    public int Grade { get; set; }
    public string TextGrade { get; set; }
    public string ShortTextGrade { get; set; }
    public int Weight { get; set; }
    public string Date { get; set; }
    public string CreateDate { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string GradeType { get; set; }
    public string EvaluationType { get; set; }
    public string EvaluationTypeDescription { get; set; }
    public string Group { get; set; }
}