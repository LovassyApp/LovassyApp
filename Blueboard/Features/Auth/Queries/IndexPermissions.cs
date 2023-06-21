using Blueboard.Core.Auth.Utils;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.Auth.Queries;

public static class IndexPermissions
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Dangerous { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly SieveProcessor _sieveProcessor;

        public Handler(SieveProcessor sieveProcessor)
        {
            _sieveProcessor = sieveProcessor;
        }

        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var permissions = PermissionUtils.Permissions;

            if (permissions == null)
                throw new UnavailableException(
                    "A jogosultságok még nincsenek betöltve. (Ennek nem kéne megtörténnie, kérlek jelezd a hibát a fejlesztőknek)");

            var filteredPermissions = _sieveProcessor.Apply(request.SieveModel, permissions.AsQueryable()).ToList();

            return filteredPermissions.Adapt<IEnumerable<Response>>();
        }
    }
}