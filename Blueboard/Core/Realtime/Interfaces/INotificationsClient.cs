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
    Task RefreshUserGroups();
    Task RefreshOwnOwnedItems();
    Task RefreshOwnedItems();
    Task RefreshFeedItems();
    Task RefreshImageVotings();
    Task RefreshImageVotingEntries();
    Task RefreshImageVotingChoices();
    Task RefreshImageVotingEntryIncrements();
    Task RefreshLoloRequestCreatedNotifiers();
}