using Blueboard.Core.Realtime;
using Blueboard.Core.Realtime.Hubs;
using Blueboard.Core.Realtime.Interfaces;
using Blueboard.Features.Shop.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Features.Shop.EventHandlers;

public class ProductsUpdatedEventHandler : INotificationHandler<ProductsUpdatedEvent>
{
    private readonly IHubContext<NotificationsHub, INotificationsClient> _notificationsHub;

    public ProductsUpdatedEventHandler(IHubContext<NotificationsHub, INotificationsClient> notificationsHub)
    {
        _notificationsHub = notificationsHub;
    }

    public async Task Handle(ProductsUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _notificationsHub.Clients.All.RefreshProducts();
    }
}