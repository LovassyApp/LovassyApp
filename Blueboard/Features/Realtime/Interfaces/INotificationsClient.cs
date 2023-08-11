namespace Blueboard.Features.Realtime.Interfaces;

public interface INotificationsClient
{
    Task RefreshProductsAsync();
}