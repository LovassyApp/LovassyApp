using System.Security.Claims;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Backboard.Services;
using WebApi.Core.Cryptography.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.School.Queries;

public static class IndexGrades
{
    public class Query : IRequest<IEnumerable<Response>>
    {
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
        public string Teacher { get; set; }
        public string Group { get; set; }

        public int GradeValue { get; set; }
        public string TextGrade { get; set; }
        public string ShortTextGrade { get; set; }

        public int Weight { get; set; }

        public DateTime EvaluationDate { get; set; }
        public DateTime CreateDate { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string GradeType { get; set; }
    }

    internal sealed class IndexGradesHandler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly BackboardAdapter _backboardAdapter;
        private readonly ApplicationDbContext _context;
        private readonly HashManager _hashManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexGradesHandler(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context,
            BackboardAdapter backboardAdapter,
            HashManager hashManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _backboardAdapter = backboardAdapter;
            _hashManager = hashManager;
        }

        public async Task<IEnumerable<Response>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            await _backboardAdapter.TryUpdatingAsync();

            var grades = await _context.Grades
                .Where(g => g.UserIdHashed ==
                            _hashManager.HashWithHasherSalt(
                                _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!))
                .Where(g => g.GradeType == GradeType.RegularGrade)
                .OrderByDescending(g => g.EvaluationDate)
                .GroupBy(g => g.Subject)
                .ToListAsync(cancellationToken);

            var response = grades.Select(g => new Response
            {
                Subject = g.Key,
                Grades = g.Adapt<List<ResponseGrade>>()
            });

            return response;
        }
    }
}