using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public static class StudentPartiesPermissions
{
    public class IndexStudentParties : IPermission
    {
        public string Name => "StudentParties.IndexStudentParties";
        public string DisplayName => "Diák pártok listázása";
        public string Description => "Az összes diák párt lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexApprovedStudentParties : IPermission
    {
        public string Name => "StudentParties.IndexApprovedStudentParties";
        public string DisplayName => "Elfogadott diák pártok listázása";
        public string Description => "Az összes elfogadott diák párt lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewStudentParty : IPermission
    {
        public string Name => "StudentParties.ViewStudentParty";
        public string DisplayName => "Diák párt megtekintése";
        public string Description => "Egy adott diák párt lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class ViewApprovedStudentParty : IPermission
    {
        public string Name => "StudentParties.ViewApprovedStudentParty";
        public string DisplayName => "Elfogadott diák párt megtekintése";
        public string Description => "Egy adott elfogadott diák párt lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class CreateStudentParty : IPermission
    {
        public string Name => "StudentParties.CreateStudentParty";
        public string DisplayName => "Diák párt létrehozása";
        public string Description => "Új diák párt létrehozása";
        public bool Dangerous => false;
    }

    public class UpdateStudentParty : IPermission
    {
        public string Name => "StudentParties.UpdateStudentParty";
        public string DisplayName => "Diák párt módosítása";
        public string Description => "Egy adott (bármelyik) diák párt módosítása id alapján";
        public bool Dangerous => true;
    }

    public class UpdateOwnStudentParty : IPermission
    {
        public string Name => "StudentParties.UpdateOwnStudentParty";
        public string DisplayName => "Saját diák párt módosítása";
        public string Description => "Saját diák párt módosítása id alapján";
        public bool Dangerous => false;
    }

    public class ApproveStudentParty : IPermission
    {
        public string Name => "StudentParties.ApproveStudentParty";
        public string DisplayName => "Diák párt elfogadása";
        public string Description => "Egy adott (bármelyik) diák párt elfogadása id alapján";
        public bool Dangerous => true;
    }

    public class DeleteStudentParty : IPermission
    {
        public string Name => "StudentParties.DeleteStudentParty";
        public string DisplayName => "Diák párt törlése";
        public string Description => "Egy adott (bármelyik) diák párt törlése id alapján";
        public bool Dangerous => true;
    }

    public class DeleteOwnStudentParty : IPermission
    {
        public string Name => "StudentParties.DeleteOwnStudentParty";
        public string DisplayName => "Saját diák párt törlése";
        public string Description => "Saját diák párt törlése id alapján";
        public bool Dangerous => false;
    }

    public class IndexStudentPartyCampaignPosts : IPermission
    {
        public string Name => "StudentParties.IndexStudentPartyCampaignPosts";
        public string DisplayName => "Diák pártok kampányposztjainak listázása";
        public string Description => "Az összes diák párt kampányposztjainak lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewStudentPartyCampaignPost : IPermission
    {
        public string Name => "StudentParties.ViewStudentPartyCampaignPost";
        public string DisplayName => "Diák párt kampányposzt megtekintése";
        public string Description => "Egy adott diák párt kampányposzt lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class CreateStudentPartyCampaignPost : IPermission
    {
        public string Name => "StudentParties.CreateStudentPartyCampaignPost";
        public string DisplayName => "Diák párt kampányposzt létrehozása";
        public string Description => "Új diák párt kampányposzt létrehozása";
        public bool Dangerous => false;
    }

    public class UpdateStudentPartyCampaignPost : IPermission
    {
        public string Name => "StudentParties.UpdateStudentPartyCampaignPost";
        public string DisplayName => "Diák párt kampányposzt módosítása";
        public string Description => "Egy adott (bármelyik) diák párt kampányposzt módosítása id alapján";
        public bool Dangerous => true;
    }

    public class UpdateOwnStudentPartyCampaignPost : IPermission
    {
        public string Name => "StudentParties.UpdateOwnStudentPartyCampaignPost";
        public string DisplayName => "Saját diák párt kampányposztjának módosítása";
        public string Description => "Saját diák párt kampányposztjának módosítása id alapján";
        public bool Dangerous => false;
    }

    public class DeleteStudentPartyCampaignPost : IPermission
    {
        public string Name => "StudentParties.DeleteStudentPartyCampaignPost";
        public string DisplayName => "Diák párt kampányposzt törlése";
        public string Description => "Egy adott (bármelyik) diák párt kampányposzt törlése id alapján";
        public bool Dangerous => true;
    }

    public class DeleteOwnStudentPartyCampaignPost : IPermission
    {
        public string Name => "StudentParties.DeleteOwnStudentPartyCampaignPost";
        public string DisplayName => "Saját diák párt kampányposztjának törlése";
        public string Description => "Saját diák párt kampányposztjának törlése id alapján";
        public bool Dangerous => false;
    }
}