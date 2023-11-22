using Blueboard.Core.Lolo.Services;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.Shop.Queries;

public static class IndexOwnLolos
{
    public class Query : IRequest<Response>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public int Balance { get; set; }
        public List<ResponseCoin> Coins { get; set; }
    }

    public class ResponseCoin
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public bool IsSpent { get; set; }

        public string LoloType { get; set; }
        public string Reason { get; set; }

        public List<ResponseGrade>? Grades { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
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

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class Handler(LoloManager loloManager, SieveProcessor sieveProcessor) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            await loloManager.LoadAsync();

            var filteredLolos = sieveProcessor.Apply(request.SieveModel, loloManager.Coins!.AsQueryable()).ToList();

            return new Response
            {
                Balance = (int)loloManager.Balance!,
                Coins = filteredLolos.Adapt<List<ResponseCoin>>()
            };
        }
    }
}