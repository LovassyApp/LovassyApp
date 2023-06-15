using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

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

    public class ViewQRCode : IPermission
    {
        public string Name { get; } = "Shop.ViewQRCode";
        public string DisplayName { get; } = "QR kód megtekintése";
        public string Description { get; } = "Egy adott QR kód lekérése és megtekintése id alapján";
    }

    public class CreateQRCode : IPermission
    {
        public string Name { get; } = "Shop.CreateQRCode";
        public string DisplayName { get; } = "QR kód létrehozása";
        public string Description { get; } = "Új QR kód létrehozása";
    }

    public class UpdateQRCode : IPermission
    {
        public string Name { get; } = "Shop.UpdateQRCode";
        public string DisplayName { get; } = "QR kód módosítása";
        public string Description { get; } = "Egy adott QR kód módosítása id alapján";
    }

    public class DeleteQRCode : IPermission
    {
        public string Name { get; } = "Shop.DeleteQRCode";
        public string DisplayName { get; } = "QR kód törlése";
        public string Description { get; } = "Egy adott QR kód törlése id alapján";
    }

    public class IndexProducts : IPermission
    {
        public string Name { get; } = "Shop.IndexProducts";
        public string DisplayName { get; } = "Termékek lekérése";
        public string Description { get; } = "Az összes termék lekérése és listázása";
    }

    public class IndexStoreProducts : IPermission
    {
        public string Name { get; } = "Shop.IndexStoreProducts";
        public string DisplayName { get; } = "Látható termékek lekérése";
        public string Description { get; } = "Az összes látható termék lekérése és listázása";
    }

    public class ViewProduct : IPermission
    {
        public string Name { get; } = "Shop.ViewProduct";
        public string DisplayName { get; } = "Termék megtekintése";
        public string Description { get; } = "Egy adott termék lekérése és megtekintése id alapján (bármelyik termék)";
    }

    public class ViewStoreProduct : IPermission
    {
        public string Name { get; } = "Shop.ViewStoreProduct";
        public string DisplayName { get; } = "Látható termék megtekintése";

        public string Description { get; } =
            "Egy adott látható termék lekérése és megtekintése id alapján (nem bármelyik termék)";
    }

    public class CreateProduct : IPermission
    {
        public string Name { get; } = "Shop.CreateProduct";
        public string DisplayName { get; } = "Termék létrehozása";
        public string Description { get; } = "Új termék létrehozása";
    }

    public class UpdateProduct : IPermission
    {
        public string Name { get; } = "Shop.UpdateProduct";
        public string DisplayName { get; } = "Termék módosítása";
        public string Description { get; } = "Egy adott termék módosítása id alapján";
    }

    public class DeleteProduct : IPermission
    {
        public string Name { get; } = "Shop.DeleteProduct";
        public string DisplayName { get; } = "Termék törlése";
        public string Description { get; } = "Egy adott termék törlése id alapján";
    }

    public class BuyProduct : IPermission
    {
        public string Name { get; } = "Shop.BuyProduct";
        public string DisplayName { get; } = "Termék vásárlása";
        public string Description { get; } = "Egy adott (csak látható) termék vásárlása id alapján";
    }
}