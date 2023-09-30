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

    public class IndexImageVotingEntries : IPermission
    {
        public string Name => "ImageVoting.IndexImageVotingEntries";
        public string DisplayName => "Kép szavazás képeinek lekérése";
        public string Description => "Egy adott (bármelyik) kép szavazás összes képének lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexActiveImageVotingEntries : IPermission
    {
        public string Name => "ImageVoting.IndexActiveImageVotingEntries";
        public string DisplayName => "Aktív kép szavazás képeinek lekérése";
        public string Description => "Egy adott aktív kép szavazás összes képének lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewImageVotingEntry : IPermission
    {
        public string Name => "ImageVoting.ViewImageVotingEntry";
        public string DisplayName => "Kép szavazás képének megtekintése";

        public string Description =>
            "Egy adott (bármelyik) kép szavazás egy képének lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class ViewActiveImageVotingEntry : IPermission
    {
        public string Name => "ImageVoting.ViewActiveImageVotingEntry";
        public string DisplayName => "Aktív kép szavazás képének megtekintése";
        public string Description => "Egy adott aktív kép szavazás egy képének lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class CreateImageVotingEntry : IPermission
    {
        public string Name => "ImageVoting.CreateImageVotingEntry";
        public string DisplayName => "Kép szavazáshoz kép létrehozása";
        public string Description => "Új kép léterhozása egy kép szavazáshoz";
        public bool Dangerous => true;
    }

    public class CreateActiveImageVotingEntry : IPermission
    {
        public string Name => "ImageVoting.CreateActiveImageVotingEntry";
        public string DisplayName => "Aktív kép szavazáshoz kép létrehozása";
        public string Description => "Új kép léterhozása egy aktív kép szavazáshoz";
        public bool Dangerous => false;
    }

    public class UpdateImageVotingEntry : IPermission
    {
        public string Name => "ImageVoting.UpdateImageVotingEntry";
        public string DisplayName => "Kép szavazás képének módosítása";
        public string Description => "Egy adott (bármelyik) kép (kép szavazásban) módosítása id alapján";
        public bool Dangerous => true;
    }

    public class UpdateOwnImageVotingEntry : IPermission
    {
        public string Name => "ImageVoting.UpdateOwnImageVotingEntry";
        public string DisplayName => "Kép szavazás saját képének módosítása";
        public string Description => "Egy adott saját kép (kép szavazásban) módosítása id alapján";
        public bool Dangerous => false;
    }

    public class DeleteImageVotingEntry : IPermission
    {
        public string Name => "ImageVoting.DeleteImageVotingEntry";
        public string DisplayName => "Kép szavazás képének törlése";
        public string Description => "Egy adott (bármelyik) kép (kép szavazásban) törlése id alapján";
        public bool Dangerous => true;
    }

    public class DeleteOwnImageVotingEntry : IPermission
    {
        public string Name => "ImageVoting.DeleteOwnImageVotingEntry";
        public string DisplayName => "Kép szavazás saját képének törlése";
        public string Description => "Egy adott saját kép (kép szavazásban) törlése id alapján";
        public bool Dangerous => false;
    }

    public class IndexImageVotingChoices : IPermission
    {
        public string Name => "ImageVoting.IndexImageVotingChoices";
        public string DisplayName => "Kép szavazás választásainak lekérése";
        public string Description => "Egy adott (bármelyik) kép szavazás összes válastászának lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexActiveImageVotingChoices : IPermission
    {
        public string Name => "ImageVoting.IndexActiveImageVotingChoices";
        public string DisplayName => "Aktív kép szavazás választásainak lekérése";
        public string Description => "Egy adott aktív kép szavazás összes választásának lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewImageVotingChoice : IPermission
    {
        public string Name => "ImageVoting.ViewImageVotingChoice";
        public string DisplayName => "Kép szavazás választásának megtekintése";

        public string Description =>
            "Egy adott (bármelyik) kép szavazás egy választásának lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class ViewActiveImageVotingChoice : IPermission
    {
        public string Name => "ImageVoting.ViewActiveImageVotingChoice";
        public string DisplayName => "Aktív kép szavazás választásának megtekintése";

        public string Description =>
            "Egy adott aktív kép szavazás egy választásának lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class CreateImageVotingChoice : IPermission
    {
        public string Name => "ImageVoting.CreateImageVotingChoice";
        public string DisplayName => "Kép szavazáshoz választás létrehozása";
        public string Description => "Új választás léterhozása egy kép szavazáshoz";
        public bool Dangerous => true;
    }

    public class CreateActiveImageVotingChoice : IPermission
    {
        public string Name => "ImageVoting.CreateActiveImageVotingChoice";
        public string DisplayName => "Aktív kép szavazáshoz választás létrehozása";
        public string Description => "Új választás léterhozása egy aktív kép szavazáshoz";
        public bool Dangerous => false;
    }

    public class UpdateImageVotingChoice : IPermission
    {
        public string Name => "ImageVoting.UpdateImageVotingChoice";
        public string DisplayName => "Kép szavazás választásának módosítása";
        public string Description => "Egy adott (bármelyik) választás (kép szavazásban) módosítása id alapján";
        public bool Dangerous => true;
    }

    public class UpdateOwnImageVotingChoice : IPermission
    {
        public string Name => "ImageVoting.UpdateOwnImageVotingChoice";
        public string DisplayName => "Kép szavazás saját választásának módosítása";
        public string Description => "Egy adott saját választás (kép szavazásban) módosítása id alapján";
        public bool Dangerous => false;
    }

    public class DeleteImageVotingChoice : IPermission
    {
        public string Name => "ImageVoting.DeleteImageVotingChoice";
        public string DisplayName => "Kép szavazás választásának törlése";
        public string Description => "Egy adott (bármelyik) választás (kép szavazásban) törlése id alapján";
        public bool Dangerous => true;
    }

    public class DeleteOwnImageVotingChoice : IPermission
    {
        public string Name => "ImageVoting.DeleteOwnImageVotingChoice";
        public string DisplayName => "Kép szavazás saját választásának törlése";
        public string Description => "Egy adott saját választás (kép szavazásban) törlése id alapján";
        public bool Dangerous => false;
    }

    public class IndexImageVotingEntryIncrements : IPermission
    {
        public string Name => "ImageVoting.IndexImageVotingEntryIncrements";
        public string DisplayName => "Kép szavazás képének szavazatainak lekérése";

        public string Description =>
            "Egy adott (bármelyik) kép (kép szavazásban) összes szavazatának lekérése és listázása";

        public bool Dangerous => false;
    }

    public class IndexActiveImageVotingEntryIncrements : IPermission
    {
        public string Name => "ImageVoting.IndexActiveImageVotingEntryIncrements";
        public string DisplayName => "Aktív kép szavazás képének szavazatainak lekérése";
        public string Description => "Egy adott aktív kép (kép szavazásban) összes szavazatának lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVoting.ViewImageVotingEntryIncrement";
        public string DisplayName => "Kép szavazás képének szavazatának megtekintése";

        public string Description =>
            "Egy adott (bármelyik) kép (kép szavazásban) szavazatának lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class ViewActiveImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVoting.ViewActiveImageVotingEntryIncrement";
        public string DisplayName => "Aktív kép szavazás képének szavazatának megtekintése";

        public string Description =>
            "Egy adott aktív kép (kép szavazásban) szavazatának lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class CreateImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVoting.CreateImageVotingEntryIncrement";
        public string DisplayName => "Kép szavazás képéhez szavazat létrehozása";
        public string Description => "Új szavazat léterhozása egy kép szavazás képéhez";
        public bool Dangerous => true;
    }

    public class CreateActiveImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVoting.CreateActiveImageVotingEntryIncrement";
        public string DisplayName => "Aktív kép szavazás képéhez szavazat létrehozása";
        public string Description => "Új szavazat léterhozása egy aktív kép szavazás képéhez";
        public bool Dangerous => false;
    }

    // No update permission for increments, because we don't want to allow anyone to change the increment amount to an arbitrary value

    public class DeleteImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVoting.DeleteImageVotingEntryIncrement";
        public string DisplayName => "Kép szavazás képének szavazatának törlése";
        public string Description => "Egy adott (bármelyik) szavazat (kép szavazásban) törlése id alapján";
        public bool Dangerous => true;
    }

    public class DeleteOwnImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVoting.DeleteOwnImageVotingEntryIncrement";
        public string DisplayName => "Kép szavazás saját képének szavazatának törlése";
        public string Description => "Egy adott saját szavazat (kép szavazásban) törlése id alapján";
        public bool Dangerous => false;
    }
}