using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public static class ImageVotingsPermissions
{
    public class IndexImageVotings : IPermission
    {
        public string Name => "ImageVotings.IndexImageVotings";
        public string DisplayName => "Kép szavazások lekérése";
        public string Description => "Az összes kép szavazás lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexActiveImageVotings : IPermission
    {
        public string Name => "ImageVotings.IndexActiveImageVotings";
        public string DisplayName => "Aktív kép szavazások lekérése";
        public string Description => "Az aktív kép szavazások lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewImageVoting : IPermission
    {
        public string Name => "ImageVotings.ViewImageVoting";
        public string DisplayName => "Kép szavazás megtekintése";
        public string Description => "Egy adott (bármilyen) kép szavazás lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class ViewActiveImageVoting : IPermission
    {
        public string Name => "ImageVotings.ViewActiveImageVoting";
        public string DisplayName => "Aktív kép szavazás megtekintése";
        public string Description => "Egy adott aktív kép szavazás lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class ViewImageVotingResults : IPermission
    {
        public string Name => "ImageVotings.ViewImageVotingResults";
        public string DisplayName => "Kép szavazás eredményének megtekintése";

        public string Description =>
            "Egy adott (bármilyen) kép szavazás eredményének lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class ViewActiveImageVotingResults : IPermission
    {
        public string Name => "ImageVotings.ViewActiveImageVotingResults";
        public string DisplayName => "Aktív kép szavazás eredményének megtekintése";
        public string Description => "Egy adott aktív kép szavazás eredményének lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class CreateImageVoting : IPermission
    {
        public string Name => "ImageVotings.CreateImageVoting";
        public string DisplayName => "Kép szavazás létrehozása";
        public string Description => "Új kép szavazás létrehozása";
        public bool Dangerous => true;
    }

    public class UpdateImageVoting : IPermission
    {
        public string Name => "ImageVotings.UpdateImageVoting";
        public string DisplayName => "Kép szavazás módosítása";
        public string Description => "Egy adott kép szavazás módosítása id alapján";
        public bool Dangerous => true;
    }

    public class DeleteImageVoting : IPermission
    {
        public string Name => "ImageVotings.DeleteImageVoting";
        public string DisplayName => "Kép szavazás törlése";
        public string Description => "Egy adott kép szavazás törlése id alapján";
        public bool Dangerous => true;
    }

    public class IndexImageVotingEntries : IPermission
    {
        public string Name => "ImageVotings.IndexImageVotingEntries";
        public string DisplayName => "Kép szavazás képeinek lekérése";

        public string Description =>
            "Egy adott (bármelyik) kép szavazás összes képének (nevezésének) lekérése és listázása";

        public bool Dangerous => false;
    }

    public class IndexActiveImageVotingEntries : IPermission
    {
        public string Name => "ImageVotings.IndexActiveImageVotingEntries";
        public string DisplayName => "Aktív kép szavazás képeinek lekérése";
        public string Description => "Egy adott aktív kép szavazás összes képének (nevezésének) lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.ViewImageVotingEntry";
        public string DisplayName => "Kép szavazás képének megtekintése";

        public string Description =>
            "Egy adott (bármelyik) kép szavazás egy képének (nevezésének) lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class ViewActiveImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.ViewActiveImageVotingEntry";
        public string DisplayName => "Aktív kép szavazás képének megtekintése";

        public string Description =>
            "Egy adott aktív kép szavazás egy képének (nevezésének) lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class CreateImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.CreateImageVotingEntry";
        public string DisplayName => "Kép szavazáshoz kép létrehozása";
        public string Description => "Új kép (nevezés) léterhozása egy kép szavazáshoz";
        public bool Dangerous => true;
    }

    public class CreateActiveImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.CreateActiveImageVotingEntry";
        public string DisplayName => "Aktív kép szavazáshoz kép létrehozása";
        public string Description => "Új kép (nevezés) léterhozása egy aktív kép szavazáshoz";
        public bool Dangerous => false;
    }

    public class UpdateImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.UpdateImageVotingEntry";
        public string DisplayName => "Kép szavazás képének módosítása";
        public string Description => "Egy adott (kép szavazásban bármelyik) kép (nevezés) módosítása id alapján";
        public bool Dangerous => true;
    }

    public class UpdateOwnImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.UpdateOwnImageVotingEntry";
        public string DisplayName => "Kép szavazás saját képének módosítása";
        public string Description => "Egy adott saját (kép szavazásban) kép (nevezés) módosítása id alapján";
        public bool Dangerous => true; //Because the votes remain on update
    }

    public class DeleteImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.DeleteImageVotingEntry";
        public string DisplayName => "Kép szavazás képének törlése";
        public string Description => "Egy adott (kép szavazásban bármelyik) kép (nevezés) törlése id alapján";
        public bool Dangerous => true;
    }

    public class DeleteOwnImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.DeleteOwnImageVotingEntry";
        public string DisplayName => "Kép szavazás saját képének törlése";
        public string Description => "Egy adott saját (kép szavazásban) kép (nevezés) törlése id alapján";
        public bool Dangerous => false;
    }

    public class IndexImageVotingChoices : IPermission
    {
        public string Name => "ImageVotings.IndexImageVotingChoices";
        public string DisplayName => "Kép szavazás választásainak lekérése";
        public string Description => "Egy adott (bármelyik) kép szavazás összes választásának lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexActiveImageVotingChoices : IPermission
    {
        public string Name => "ImageVotings.IndexActiveImageVotingChoices";
        public string DisplayName => "Aktív kép szavazás választásainak lekérése";
        public string Description => "Egy adott aktív kép szavazás összes választásának lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewImageVotingChoice : IPermission
    {
        public string Name => "ImageVotings.ViewImageVotingChoice";
        public string DisplayName => "Kép szavazás választásának megtekintése";

        public string Description =>
            "Egy adott (bármelyik) kép szavazás egy választásának lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class ViewActiveImageVotingChoice : IPermission
    {
        public string Name => "ImageVotings.ViewActiveImageVotingChoice";
        public string DisplayName => "Aktív kép szavazás választásának megtekintése";

        public string Description =>
            "Egy adott aktív kép szavazás egy választásának lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class ChooseImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.ChooseImageVotingEntry";
        public string DisplayName => "Kép szavazásban kép választása";
        public string Description => "(Bármelyik) kép szavazásban egy feltöltött kép (nevezés) választása";
        public bool Dangerous => true;
    }

    public class ChooseActiveImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.ChooseActiveImageVotingEntry";
        public string DisplayName => "Aktív kép szavazásban kép választása";
        public string Description => "Aktív kép szavazásban egy feltöltött kép (nevezés) választása";
        public bool Dangerous => false;
    }

    public class UnchooseImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.UnchooseImageVotingEntry";
        public string DisplayName => "Kép szavazásban kép választásának visszavonása";
        public string Description => "(Bármelyik) kép szavazásban egy választás visszavonása";
        public bool Dangerous => true;
    }

    public class UnchooseActiveImageVotingEntry : IPermission
    {
        public string Name => "ImageVotings.UnchooseActiveImageVotingEntry";
        public string DisplayName => "Aktív kép szavazásban kép választásának visszavonása";
        public string Description => "Aktív kép szavazásban egy választás visszavonása";
        public bool Dangerous => false;
    }

    public class CreateImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVotings.CreateImageVotingEntryIncrement";
        public string DisplayName => "Kép szavazás képének inkrementálása";
        public string Description => "Egy adott (bármelyik) kép szavazás egy képének (nevezésének) inkrementálása";
        public bool Dangerous => true;
    }

    public class CreateActiveImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVotings.CreateActiveImageVotingEntryIncrement";
        public string DisplayName => "Aktív kép szavazás képének inkrementálása";
        public string Description => "Egy adott aktív kép szavazás egy képének (nevezésének) inkrementálása";
        public bool Dangerous => false;
    }

    public class DeleteImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVotings.DeleteImageVotingEntryIncrement";
        public string DisplayName => "Kép szavazás képéhez tartozó inkrementálás törlése";

        public string Description =>
            "Egy adott (bármelyik) kép szavazás egy képéhez (nevezésének) tartozó inkrementálás törlése";

        public bool Dangerous => true;
    }

    public class DeleteActiveImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVotings.DeleteActiveImageVotingEntryIncrement";
        public string DisplayName => "Aktív kép szavazás képéhez tartozó inkrementálás törlése";

        public string Description =>
            "Egy adott aktív kép szavazás egy képéhez (nevezésének) tartozó inkrementálás törlése";

        public bool Dangerous => false;
    }

    public class IndexImageVotingEntryIncrements : IPermission
    {
        public string Name => "ImageVotings.IndexImageVotingEntryIncrements";
        public string DisplayName => "Kép szavazás képének szavazatainak lekérése";

        public string Description =>
            "Egy adott (bármelyik) kép szavazás összes szavazatának lekérése és listázása";

        public bool Dangerous => false;
    }

    public class IndexActiveImageVotingEntryIncrements : IPermission
    {
        public string Name => "ImageVotings.IndexActiveImageVotingEntryIncrements";
        public string DisplayName => "Aktív kép szavazás képének szavazatainak lekérése";
        public string Description => "Egy adott aktív kép szavazás összes szavazatának lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVotings.ViewImageVotingEntryIncrement";
        public string DisplayName => "Kép szavazás képének szavazatának megtekintése";

        public string Description =>
            "Egy adott (bármelyik) kép szavazás szavazatának lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class ViewActiveImageVotingEntryIncrement : IPermission
    {
        public string Name => "ImageVotings.ViewActiveImageVotingEntryIncrement";
        public string DisplayName => "Aktív kép szavazás képének szavazatának megtekintése";

        public string Description =>
            "Egy adott aktív kép szavazás szavazatának lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class IndexImageVotingEntryImages : IPermission
    {
        public string Name => "ImageVotings.IndexImageVotingEntryImages";
        public string DisplayName => "Kép szavazás be nem küldött kép fájlajainak lekérése";

        public string Description =>
            "Egy adott képszavazás (összes) be nem küldött kép fájljainak lekérése és listázása";

        public bool Dangerous => false;
    }

    public class IndexOwnImageVotingEntryImages : IPermission
    {
        public string Name => "ImageVotings.IndexOwnImageVotingEntryImages";
        public string DisplayName => "Kép szavazás saját be nem küldött kép fájljainak lekérése";
        public string Description => "Egy adott képszavazás saját be nem küldött kép fájljainak lekérése és listázása";
        public bool Dangerous => false;
    }

    public class UploadImageVotingEntryImage : IPermission
    {
        public string Name => "ImageVotings.UploadImageVotingEntryImage";
        public string DisplayName => "Kép szavazáshoz kép fájl feltöltése";
        public string Description => "Új kép fájl feltöltése egy (bármelyik) kép szavazáshoz";
        public bool Dangerous => true;
    }

    public class UploadActiveImageVotingEntryImage : IPermission
    {
        public string Name => "ImageVotings.UploadActiveImageVotingEntryImage";
        public string DisplayName => "Aktív kép szavazáshoz kép fájl feltöltése";
        public string Description => "Új kép fájl feltöltése egy aktív kép szavazáshoz";
        public bool Dangerous => false;
    }

    public class DeleteImageVotingEntryImage : IPermission
    {
        public string Name => "ImageVotings.DeleteImageVotingEntryImage";
        public string DisplayName => "Kép szavazás be nem küldött kép fájljának törlése";
        public string Description => "Egy adott képszavazás (bármelyik) be nem küldött kép fájljának törlése";
        public bool Dangerous => true;
    }

    public class DeleteOwnImageVotingEntryImage : IPermission
    {
        public string Name => "ImageVotings.DeleteOwnImageVotingEntryImage";
        public string DisplayName => "Kép szavazás saját be nem küldött kép fájljának törlése";
        public string Description => "Egy adott képszavazás saját be nem küldött kép fájljának törlése";
        public bool Dangerous => false;
    }
}