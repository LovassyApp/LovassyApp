namespace Helpers.Email.Views.Emails.OwnedItemUsedNotification;

public class OwnedItemUsedNotificationViewModel
{
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string UserDisplayName { get; set; }
    public string? UserClass { get; set; }
    public Dictionary<string, string> InputValues { get; set; }
}