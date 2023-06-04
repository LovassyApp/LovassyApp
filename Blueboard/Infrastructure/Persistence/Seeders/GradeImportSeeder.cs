using System.Text.Json;
using Blueboard.Core.Backboard.Models;
using Blueboard.Infrastructure.Persistence.Entities;
using Bogus;
using Helpers.Cryptography.Implementations;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Infrastructure.Persistence.Seeders;

public class GradeImportSeeder
{
    private static readonly Faker Faker = new();
    private static readonly int GradeCount = 5;
    private static readonly int SubjectCount = 5;

    private static readonly string[] Numbers = { "9ny", "9", "10", "11", "12" };
    private static readonly string[] Letters = { "A", "B", "C", "D" };

    private static readonly Dictionary<int, string> TextGrades = new()
    {
        [5] = "Jeles(5)",
        [4] = "Jó(4)"
        //[3] = "Közepes(3)",
        //[2] = "Elégséges(2)",
        //[1] = "Elégtelen(1)"
    };

    private static readonly int MaxGrade = 5;
    private static readonly int MinGrade = 4;

    private static readonly string[] Types =
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
        var number = Faker.Random.ArrayElement(Numbers);
        var letter = Faker.Random.ArrayElement(Letters);

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

        for (var i = 0; i < SubjectCount; i++)
        {
            subjects.Add(Faker.Lorem.Word());
            for (var j = 0; j < GradeCount; j++)
            {
                var number = Faker.Random.ArrayElement(Numbers);
                var letter = Faker.Random.ArrayElement(Letters);

                var groupId = number + "." + letter + " " + Faker.Lorem.Word();
                var gradeValue = Faker.Random.Int(MinGrade, MaxGrade);

                var grade = new BackboardGrade
                {
                    SubjectCategory = subjects[i] + "_Category",
                    Subject = subjects[i],
                    Group = groupId,
                    Teacher = Faker.Name.FullName(),
                    Type = Faker.Random.ArrayElement(Types),
                    TextGrade = TextGrades[gradeValue],
                    Grade = gradeValue.ToString(),
                    ShortTextGrade = "",
                    DiligenceGrade = " - ",
                    BehaviourGrade = " - ",
                    CreateDate = DateTime.Now.ToString("yyyy.M.d"),
                    RecordDate = DateTime.Now.ToString("yyyy.M.d"),
                    Name = Faker.Lorem.Sentence(3)
                };

                grades.Add(grade);
            }
        }

        return grades;
    }
}