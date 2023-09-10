using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public static class FeedPermissions
{
    public class IndexFeedItems : IPermission
    {
        public string Name => "Feed.IndexFeedItems";
        public string DisplayName => "Hírfolyam híreinek listázása";
        public string Description => "Az aktuális hírfolyam összes hírének lekérése";
        public bool Dangerous => false;
    }
}