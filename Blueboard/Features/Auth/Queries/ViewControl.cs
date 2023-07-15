using Blueboard.Core.Auth;
using Blueboard.Core.Auth.Services;
using Mapster;
using MediatR;
using Microsoft.FeatureManagement;

namespace Blueboard.Features.Auth.Queries;

public static class ViewControl
{
    public class Query : IRequest<Response>
    {
    }

    public class Response
    {
        public ResponseUser User { get; set; }
        public ResponseSession Session { get; set; }
        public bool IsSuperUser { get; set; }
        public string[] UserGroups { get; set; }
        public string[] Permissions { get; set; }
        public string[] Features { get; set; }
    }

    public class ResponseSession
    {
        // I chose to no longer include the session data in v4 anymore as it should only contain encrypted data
        public DateTime Expiry { get; set; }
    }

    public class ResponseUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public string? RealName { get; set; }
        public string? Class { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IFeatureManager _featureManager;
        private readonly PermissionManager _permissionManager;
        private readonly SessionManager _sessionManager;
        private readonly UserAccessor _userAccessor;

        public Handler(IHttpContextAccessor contextAccessor, SessionManager sessionManager,
            UserAccessor userAccessor, IFeatureManager featureManager, PermissionManager permissionManager)
        {
            _contextAccessor = contextAccessor;
            _sessionManager = sessionManager;
            _userAccessor = userAccessor;
            _featureManager = featureManager;
            _permissionManager = permissionManager;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var enabledFeatures = new List<string>();
            // ReSharper disable once UseCancellationTokenForIAsyncEnumerable
            await foreach (var feature in _featureManager.GetFeatureNamesAsync())
                if (await _featureManager.IsEnabledAsync(feature))
                    enabledFeatures.Add(feature);

            var response = new Response
            {
                User = _userAccessor.User.Adapt<ResponseUser>(),
                Session = _sessionManager.Session!.Adapt<ResponseSession>(),
                IsSuperUser = _permissionManager.CheckSuperUser(),
                Permissions = _contextAccessor.HttpContext!.User.FindAll(AuthConstants.PermissionClaim)
                    .Select(c => c.Value).ToArray(),
                UserGroups = _userAccessor.User.UserGroups.Select(userGroup => userGroup.Name).ToArray(),
                Features = enabledFeatures.ToArray()
            };

            return response;
        }
    }
}