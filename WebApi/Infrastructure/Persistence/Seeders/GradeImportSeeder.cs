using System.Text.Json;
using Bogus;
using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Backboard.Models;
using WebApi.Core.Cryptography.Models;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Infrastructure.Persistence.Seeders;

public class GradeImportSeeder
{
    private static readonly Faker _faker = new();
    private static readonly int _gradeCount = 5;
    private static readonly int _subjectCount = 5;

    private static readonly string[] _numbers = { "9ny", "9", "10", "11", "12" };
    private static readonly string[] _letters = { "A", "B", "C", "D" };

    private static readonly Dictionary<int, string> _textGrades = new()
    {
        [5] = "Jeles(5)",
        [4] = "Jó(4)"
        //[3] = "Közepes(3)",
        //[2] = "Elégséges(2)",
        //[1] = "Elégtelen(1)"
    };

    private static readonly int _maxGrade = 5;
    private static readonly int _minGrade = 4;

    private static readonly string[] _types =
    {
        "Szorgalmi feladat",
        "Írásbeli röpdolgozat",
        "Írásbeli dolgozat",
        "Gyakorlati feladat",
        "Szóbeli feladat",
        "Írásbeli témazáró dolgozat",
        "Írásbeli témazáró dolgozat (dupla súllyal)"
    };

    private readonly ApplicationDbContext _context;

    public GradeImportSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task RunAsync()
    {
        var users = await _context.Users.Where(u => !u.ImportAvailable).ToListAsync();

        foreach (var user in users)
        {
            var gradeCollection = MakeGradeCollection(user);
            var gradeCollectionSerialized = JsonSerializer.Serialize(gradeCollection);
            var encrypter = new KyberAsymmetricEncrypter(user.PublicKey);
            var gradeCollectionEncrypted = encrypter.Encrypt(gradeCollectionSerialized);

            var gradeImport = new GradeImport
            {
                User = user,
                JsonEncrypted = gradeCollectionEncrypted
            };
            await _context.GradeImports.AddAsync(gradeImport);
            user.ImportAvailable = true;
        }

        await _context.SaveChangesAsync();
    }

    private BackboardGradeCollection MakeGradeCollection(User user)
    {
        var number = _faker.Random.ArrayElement(_numbers);
        var letter = _faker.Random.ArrayElement(_letters);

        return new BackboardGradeCollection
        {
            Grades = GenerateFakeGradeData(),
            StudentName = user.Name,
            SchoolClass = number + "." + letter,
            User = user.Adapt<BackboardUser>()
        };
    }

    private List<BackboardGrade> GenerateFakeGradeData()
    {
        var subjects = new List<string>();
        var grades = new List<BackboardGrade>();

        for (var i = 0; i < _subjectCount; i++)
        {
            subjects.Add(_faker.Lorem.Word());
            for (var j = 0; j < _gradeCount; j++)
            {
                var number = _faker.Random.ArrayElement(_numbers);
                var letter = _faker.Random.ArrayElement(_letters);

                var groupId = number + "." + letter + " " + _faker.Lorem.Word();
                var gradeValue = _faker.Random.Int(_minGrade, _maxGrade);

                var grade = new BackboardGrade
                {
                    SubjectCategory = subjects[i] + "_Category",
                    Subject = subjects[i],
                    Group = groupId,
                    Teacher = _faker.Name.FullName(),
                    Type = _faker.Random.ArrayElement(_types),
                    TextGrade = _textGrades[gradeValue],
                    Grade = gradeValue.ToString(),
                    ShortTextGrade = "",
                    DiligenceGrade = " - ",
                    BehaviourGrade = " - ",
                    CreateDate = DateTime.Now.ToString("yyyy.M.d"),
                    RecordDate = DateTime.Now.ToString("yyyy.M.d"),
                    Name = _faker.Lorem.Sentence(3)
                };

                grades.Add(grade);
            }
        }

        return grades;
    }
}