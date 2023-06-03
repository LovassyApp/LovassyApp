using WebApi.Core.Auth.Interfaces;

namespace WebApi.Core.Auth.Permissions;

public class ShopPermissions
{
    public class IndexOwnLolos : IPermission
    {
        public string Name { get; } = "Shop.IndexOwnLolos";
        public string DisplayName { get; } = "Saját lolók lekérése";
        public string Description { get; } = "A saját loló mennyiség és loló érmék lekérése és listázása";
    }

    public class IndexLolos : IPermission
    {
        public string Name { get; } = "Shop.IndexLolos";
        public string DisplayName { get; } = "Lolók lekérése";
        public string Description { get; } = "Az összes loló érme lekérése és listázása";
    }

    public class IndexLoloRequests : IPermission
    {
        public string Name { get; } = "Shop.IndexLoloRequests";
        public string DisplayName { get; } = "Loló kérvények lekérése";
        public string Description { get; } = "Az összes loló kérvény lekérése és listázása";
    }

    public class ViewLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.ViewLoloRequest";
        public string DisplayName { get; } = "Loló kérvény megtekintése";
        public string Description { get; } = "Egy adott loló kérvény lekérése és megtekintése id alapján";
    }

    public class CreateLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.CreateLoloRequest";
        public string DisplayName { get; } = "Loló kérvény létrehozása";
        public string Description { get; } = "Új saját loló kérvény létrehozása";
    }

    public class OverruleLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.OverruleLoloRequest";
        public string DisplayName { get; } = "Loló kérvény felülbírálása";
        public string Description { get; } = "Egy adott loló kérvény felülbírálása id alapján";
    }

    public class UpdateOwnLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.UpdateOwnLoloRequest";
        public string DisplayName { get; } = "Loló kérvény módosítása";
        public string Description { get; } = "Egy adott saját loló kérvény módosítása id alapján";
    }

    public class UpdateLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.UpdateLoloRequest";
        public string DisplayName { get; } = "Loló kérvény módosítása";
        public string Description { get; } = "Egy adott loló kérvény módosítása id alapján";
    }

    public class DeleteOwnLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.DeleteOwnLoloRequest";
        public string DisplayName { get; } = "Loló kérvény törlése";
        public string Description { get; } = "Egy adott saját loló kérvény törlése id alapján";
    }

    public class DeleteLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.DeleteLoloRequest";
        public string DisplayName { get; } = "Loló kérvény törlése";
        public string Description { get; } = "Egy adott loló kérvény törlése id alapján";
    }

    public class IndexQRCodes : IPermission
    {
        public string Name { get; } = "Shop.IndexQRCodes";
        public string DisplayName { get; } = "QR kódok lekérése";
        public string Description { get; } = "Az összes QR kód lekérése és listázása";
    }

    public class CreateQRCode : IPermission
    {
        public string Name { get; } = "Shop.CreateQRCode";
        public string DisplayName { get; } = "QR kód létrehozása";
        public string Description { get; } = "Új QR kód létrehozása";
    }
}