namespace Blueboard.Core.Realtime.Interfaces;

public interface INotificationsClient
{
    Task RefreshProducts();
    Task RefreshGrades();
    Task RefreshLolos();
}