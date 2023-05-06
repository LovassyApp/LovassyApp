using MediatR;
using WebApi.Core.Backboard.Services;
using WebApi.Features.Auth.EventInterfaces;

namespace WebApi.Features.Auth.EventHandlers;

public class UpdateGradesOnSessionCreatedEvent<TSessionCreatedEvent> : INotificationHandler<TSessionCreatedEvent>
    where TSessionCreatedEvent : ISessionCreatedEvent
{
    private readonly BackboardAdapter _backboardAdapter;

    public UpdateGradesOnSessionCreatedEvent(BackboardAdapter backboardAdapter)
    {
        _backboardAdapter = backboardAdapter;
    }

    public async Task Handle(TSessionCreatedEvent notification, CancellationToken cancellationToken)
    {
        _backboardAdapter.Init(notification.User);
        await _backboardAdapter.TryUpdatingAsync();
    }
}