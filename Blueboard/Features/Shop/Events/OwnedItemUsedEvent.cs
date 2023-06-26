using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;

namespace Blueboard.Features.Shop.Events;

public class OwnedItemUsedEvent : INotification
{
    public User User { get; set; }
    public Product Product { get; set; } // IMPORTANT: Has to contain the Product as well
    public Dictionary<string, string> InputValues { get; set; }
    public string? QRCodeEmail { get; set; }
}