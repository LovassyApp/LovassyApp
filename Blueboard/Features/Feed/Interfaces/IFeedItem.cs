namespace Blueboard.Features.Feed.Interfaces;

public class IFeedItem
{
    public string Title { get; set; }
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Link { get; set; }
}