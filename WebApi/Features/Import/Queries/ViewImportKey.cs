using Helpers.Cryptography.Services;
using Helpers.Framework.Exceptions;
using Mapster;
using MediatR;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Import.Queries;

public static class ViewImportKey
{
    public class Query : IRequest<Response>
    {
        public int Id { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool Enabled { get; set; }

        public string Key { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly EncryptionService _encryptionService;

        public Handler(ApplicationDbContext context, EncryptionService encryptionService)
        {
            _context = context;
            _encryptionService = encryptionService;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var importKey = await _context.ImportKeys.FindAsync(request.Id);

            if (importKey is null)
                throw new NotFoundException(nameof(ImportKey), request.Id);

            var key = _encryptionService.Unprotect(importKey.KeyProtected);

            var response = importKey.Adapt<Response>();
            response.Key = key;

            return response;
        }
    }
}