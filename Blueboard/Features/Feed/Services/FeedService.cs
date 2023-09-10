using System.Web;
using Blueboard.Features.Feed.Interfaces;
using Blueboard.Features.Feed.Models;
using Blueboard.Features.Feed.Services.Options;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace Blueboard.Features.Feed.Services;

/// <summary>
///     The singleton service responsible for storing feed items.
/// </summary>
public class FeedService
{
    private readonly IOptionsMonitor<FeedOptions> _feedOptionsMonitor;

    public FeedService(IOptionsMonitor<FeedOptions> feedOptionsMonitor)
    {
        _feedOptionsMonitor = feedOptionsMonitor;
    }

    public List<IFeedItem> FeedItems { get; } = new();

    public async Task UpdateFeed()
    {
        var websiteContent = await LoadLovassyWebsiteAsync();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(websiteContent);

        var articles = htmlDoc.DocumentNode.Descendants("article")
            .Where(node => node.GetAttributeValue("class", "").Contains("post")).ToList();

        FeedItems.Clear();

        foreach (var article in articles.Skip(1)) //The first article tag is not an actual article
        {
            var feedItem = new LovassyFeedItem
            {
                Title = HttpUtility.HtmlDecode(article.Descendants("h2").FirstOrDefault()?.Descendants("a")
                    ?.FirstOrDefault()?.InnerText ?? ""),
                Description =
                    HttpUtility.HtmlDecode(article.Descendants("div")
                        .FirstOrDefault(node => node.GetAttributeValue("class", "").Contains("post-content"))
                        ?.Descendants("p")?.FirstOrDefault()?.InnerText ?? ""),
                CreatedAt = DateTime.Parse(article
                    .Descendants("p").FirstOrDefault(node => node.GetAttributeValue("class", "").Contains("post-meta"))
                    ?.Descendants("span").FirstOrDefault()?.InnerText ?? ""),
                Link = article.Descendants("h2").FirstOrDefault()?.Descendants("a")?.FirstOrDefault()
                    ?.GetAttributeValue("href", "")
            };

            FeedItems.Add(feedItem);
        }
    }

    private async Task<string> LoadLovassyWebsiteAsync()
    {
        var client = new HttpClient();
        var response = await client.GetStringAsync(_feedOptionsMonitor.CurrentValue.LovassyWebsiteUrl);
        return response;
    }
}