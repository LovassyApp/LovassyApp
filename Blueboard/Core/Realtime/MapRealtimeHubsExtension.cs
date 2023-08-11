using Blueboard.Core.Realtime.Hubs;
using Microsoft.AspNetCore.Http.Connections;

namespace Blueboard.Core.Realtime;

public static class MapRealtimeHubsExtension
{
    /// <summary>
    ///     Maps all SignalR hubs used for realtime communication.
    /// </summary>
    /// <param name="endpointRouteBuilder">The endpoint builder.</param>
    public static void MapRealtimeHubs(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapHub<NotificationsHub>("/Hubs/Notifications",
            o =>
            {
                o.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling |
                               HttpTransportType.ServerSentEvents;
            });
    }
}