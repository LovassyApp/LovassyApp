using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;
using WebApi.Core.Auth.Extensions;
using WebApi.Core.Lolo.Services;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Shop.Queries;

public static class IndexLolos
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

        public LoloType LoloType { get; set; }
        public string Reason { get; set; }

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

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoloManager _loloManager;
        private readonly SieveProcessor _sieveProcessor;

        public Handler(IHttpContextAccessor httpContextAccessor, LoloManager loloManager, SieveProcessor sieveProcessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _loloManager = loloManager;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetId();

            _loloManager.Init(userId);
            await _loloManager.LoadAsync();

            var filteredLolos = _sieveProcessor.Apply(request.SieveModel, _loloManager.Coins!.AsQueryable());

            var coins = filteredLolos.Adapt<List<ResponseCoin>>();

            return new Response
            {
                Balance = (int)_loloManager.Balance!,
                Coins = coins
            };
        }
    }
}