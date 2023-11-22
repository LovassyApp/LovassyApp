using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.School.Queries;

public static class IndexGrades
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public string Subject { get; set; }
        public List<ResponseGrade> Grades { get; set; }
    }

    public class ResponseGrade
    {
        public int Id { get; set; }

        public string Uid { get; set; }

        public string Subject { get; set; }
        public string SubjectCategory { get; set; }

        public string Theme { get; set; }

        public string Teacher { get; set; }
        public string Group { get; set; }

        public int GradeValue { get; set; }
        public string TextGrade { get; set; }
        public string ShortTextGrade { get; set; }

        public int Weight { get; set; }

        public DateTime EvaluationDate { get; set; }
        public DateTime CreateDate { get; set; }

        public string Type { get; set; }
        public string GradeType { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class Handler(UserAccessor userAccessor, ApplicationDbContext context,
            SieveProcessor sieveProcessor, HashManager hashManager)
        : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var userIdHashed = hashManager.HashWithHasherSalt(userAccessor.User.Id.ToString());

            var grades = context.Grades
                .Where(g => g.UserIdHashed == userIdHashed && g.GradeType == GradeType.RegularGrade).AsNoTracking();

            var filteredGrades = await sieveProcessor.Apply(request.SieveModel, grades).GroupBy(g => g.Subject)
                .ToListAsync(cancellationToken);

            var response = filteredGrades.Select(g => new Response
            {
                Subject = g.Key,
                Grades = g.Adapt<List<ResponseGrade>>()
            });

            return response;
        }
    }
}