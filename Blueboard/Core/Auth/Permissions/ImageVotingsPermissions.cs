using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public static class ImageVotingsPermissions
{
    public class IndexImageVotings : IPermission
    {
        public string Name => "ImageVoting.IndexImageVotings";
        public string DisplayName => "Kép szavazások lekérése";
        public string Description => "Az összes kép szavazás lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexActiveImageVotings : IPermission
    {
        public string Name => "ImageVoting.IndexActiveImageVotings";
        public string DisplayName => "Aktív kép szavazások lekérése";
        public string Description => "Az aktív kép szavazás lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewImageVoting : IPermission
    {
        public string Name => "ImageVoting.ViewImageVoting";
        public string DisplayName => "Kép szavazás megtekintése";
        public string Description => "Egy adott (bármilyen) kép szavazás lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class ViewActiveImageVoting : IPermission
    {
        public string Name => "ImageVoting.ViewActiveImageVoting";
        public string DisplayName => "Aktív kép szavazás megtekintése";
        public string Description => "Egy adott aktív kép szavazás lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class CreateImageVoting : IPermission
    {
        public string Name => "ImageVoting.CreateImageVoting";
        public string DisplayName => "Kép szavazás létrehozása";
        public string Description => "Új kép szavazás létrehozása";
        public bool Dangerous => true;
    }

    public class UpdateImageVoting : IPermission
    {
        public string Name => "ImageVoting.UpdateImageVoting";
        public string DisplayName => "Kép szavazás módosítása";
        public string Description => "Egy adott kép szavazás módosítása id alapján";
        public bool Dangerous => true;
    }

    public class DeleteImageVoting : IPermission
    {
        public string Name => "ImageVoting.DeleteImageVoting";
        public string DisplayName => "Kép szavazás törlése";
        public string Description => "Egy adott kép szavazás törlése id alapján";
        public bool Dangerous => true;
    }
}