using WebApi.Core.Cryptography.Utils;

namespace WebApi.Core.Backboard.Models;

public class KretaGrade
{
    private readonly ILookup<string, int> _textGradeLookup = new Dictionary<string, int>
    {
        ["peldas"] = 5,
        ["jo"] = 4,
        ["valtozo"] = 3,
        ["rossz"] = 2,
        ["hanyag"] = 1
    }.ToLookup(dict => dict.Key, dict => dict.Value);

    public KretaGrade(BackboardGrade grade)
    {
        var recordDate = SplitDate(grade.RecordDate);
        var createDate = SplitDate(grade.CreateDate);
        Date = new DateTime(recordDate[0], recordDate[1], recordDate[3]).ToString("yyyy-MM-dd");
        CreateDate = new DateTime(createDate[0], createDate[1], createDate[3]).ToString("yyyy-MM-dd");

        Subject = char.ToUpper(grade.Subject[0]) + grade.Subject.Substring(1);
        SubjectCategory = char.ToUpper(grade.SubjectCategory[0]) + grade.SubjectCategory.Substring(1);

        EvaluationType = KretaGradeCategory.Interim;
        GradeType = GetType(grade);
        EvaluationTypeDescription = "Évközi jegy/értékelés";

        Teacher = grade.Teacher ?? "Névtelen hős";
        Name = grade.Name == " - " ? "Névtelen jegy" : char.ToUpper(grade.Name[0]) + grade.Name.Substring(1);
        Type = grade.Type ?? "Nincs leírás";
        Group = grade.Grade;

        Weight = GetWeight(Type);

        TextGrade = ConvertTextGrade(GetTextGrade(grade));
        ShortTextGrade = ConvertTextGrade(grade.ShortTextGrade);
        Grade = grade.Grade != null ? int.Parse(grade.Grade) : ConvertValueToInteger(grade.TextGrade);

        Uid = HashingUtils.Hash(Date + CreateDate + Subject + SubjectCategory + EvaluationType +
                                EvaluationTypeDescription + Teacher + Name + Type + Weight + TextGrade +
                                ShortTextGrade + Grade);
    }

    public string Uid { get; set; }
    public string Subject { get; set; }
    public string SubjectCategory { get; set; } //
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
    public KretaGradeCategory EvaluationType { get; set; }
    public string EvaluationTypeDescription { get; set; }
    public string Group { get; set; }

    private List<int> SplitDate(string date)
    {
        return date.Split(".").Where(d => !string.IsNullOrEmpty(d)).Select(int.Parse).ToList();
    }

    private int GetWeight(string type)
    {
        if (type == "Írásbeli témazáró dolgozat (dupla súllyal)")
            return 200;
        return 100;
    }

    private string GetType(BackboardGrade grade)
    {
        if (grade.BehaviourGrade != " - ")
            return "MagatartasErtek";
        if (grade.DiligenceGrade != " - ")
            return "SzorgalomErtek";

        return "Osztalyzat";
    }

    private string GetTextGrade(BackboardGrade grade)
    {
        if (grade.BehaviourGrade != " - ")
            return grade.BehaviourGrade;

        if (grade.DiligenceGrade != " - ")
            return grade.DiligenceGrade;

        return grade.TextGrade;
    }

    private string ConvertTextGrade(string textGrade)
    {
        var splitGrade = textGrade.Split("(");
        return splitGrade[0];
    }

    private int ConvertValueToInteger(string grade)
    {
        var textLower = grade.ToLower(); // If I'm right, we don't need the utf-8 cleaning here as opposed to in v3
        var value = _textGradeLookup.FirstOrDefault(x => x.Key == textLower);

        if (value == null)
            return 0;

        return value.First();
    }
}