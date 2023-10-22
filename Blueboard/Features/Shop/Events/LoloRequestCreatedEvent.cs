using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;

namespace Blueboard.Features.Shop.Events;

public class LoloRequestCreatedEvent : INotification
{
    public LoloRequest LoloRequest { get; set; }
    public string LoloRequestsUrl { get; set; }
}