namespace Blueboard.Core.Realtime.Interfaces;

public interface INotificationsClient
{
    Task RefreshProducts();
    Task RefreshGrades();
    Task RefreshOwnLolos();
    Task RefreshLolos();
    Task RefreshQRCodes();
    Task RefreshOwnLoloRequests();
    Task RefreshLoloRequests();
}