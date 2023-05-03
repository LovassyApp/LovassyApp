using WebApi.Core.Backboard.Models;
using WebApi.Core.Cryptography.Utils;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Backboard.Utils;

public static class GradeUtils
{
    private static readonly ILookup<string, int> TextGradeLookup = new Dictionary<string, int>
    {
        ["peldas"] = 5,
        ["jo"] = 4,
        ["valtozo"] = 3,
        ["rossz"] = 2,
        ["hanyag"] = 1
    }.ToLookup(dict => dict.Key, dict => dict.Value);

    public static Grade TransformBackboardGrade(BackboardGrade grade, Guid userId)
    {
        var recordDate = SplitDate(grade.RecordDate);
        var createDate = SplitDate(grade.CreateDate);

        var result = new Grade
        {
            UserId = userId,

            EvaluationDate = new DateTime(recordDate[0], recordDate[1], recordDate[3]).ToUniversalTime(),
            CreateDate = new DateTime(createDate[0], createDate[1], createDate[3]).ToUniversalTime(),

            Subject = char.ToUpper(grade.Subject[0]) + grade.Subject.Substring(1),
            SubjectCategory = char.ToUpper(grade.SubjectCategory[0]) + grade.SubjectCategory.Substring(1),

            GradeType = GetType(grade),

            Teacher = grade.Teacher ?? "Névtelen hős",
            Name = grade.Name == " - " ? "Névtelen jegy" : char.ToUpper(grade.Name[0]) + grade.Name.Substring(1),
            Type = grade.Type ?? "Nincs leírás",
            Group = grade.Group,

            Weight = GetWeight(grade.Type),

            TextGrade = ConvertTextGrade(GetTextGrade(grade)),
            ShortTextGrade = ConvertTextGrade(grade.ShortTextGrade),
            GradeValue = grade.Grade != null ? int.Parse(grade.Grade) : ConvertValueToInteger(grade.TextGrade)
        };

        result.Uid = HashingUtils.Hash(result.EvaluationDate.ToLongDateString() + result.CreateDate.ToLongDateString() +
                                       result.Subject + result.SubjectCategory + result.Teacher + result.Name +
                                       result.Type + result.Weight + result.TextGrade + result.ShortTextGrade +
                                       result.GradeValue);

        return result;
    }

    private static List<int> SplitDate(string date)
    {
        return date.Split(".").Where(d => !string.IsNullOrEmpty(d)).Select(int.Parse).ToList();
    }

    private static int GetWeight(string? type)
    {
        if (type == "Írásbeli témazáró dolgozat (dupla súllyal)")
            return 200;
        return 100;
    }

    private static string GetType(BackboardGrade grade)
    {
        if (grade.BehaviourGrade != " - ")
            return "MagatartasErtek";
        if (grade.DiligenceGrade != " - ")
            return "SzorgalomErtek";

        return "Osztalyzat";
    }

    private static string GetTextGrade(BackboardGrade grade)
    {
        if (grade.BehaviourGrade != " - ")
            return grade.BehaviourGrade;

        if (grade.DiligenceGrade != " - ")
            return grade.DiligenceGrade;

        return grade.TextGrade;
    }

    private static string ConvertTextGrade(string textGrade)
    {
        var splitGrade = textGrade.Split("(");
        return splitGrade[0];
    }

    private static int ConvertValueToInteger(string grade)
    {
        var textLower = grade.ToLower(); // If I'm right, we don't need the utf-8 cleaning here as opposed to in v3
        var value = TextGradeLookup.FirstOrDefault(x => x.Key == textLower);

        if (value == null)
            return 0;

        return value.First();
    }
}