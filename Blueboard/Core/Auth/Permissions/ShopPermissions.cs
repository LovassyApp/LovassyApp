using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public class ShopPermissions
{
    public class IndexOwnLolos : IPermission
    {
        public string Name { get; } = "Shop.IndexOwnLolos";
        public string DisplayName { get; } = "Saját lolók lekérése";
        public string Description { get; } = "A saját loló mennyiség és loló érmék lekérése és listázása";
        public bool Dangerous { get; } = false;
    }

    public class IndexLolos : IPermission
    {
        public string Name { get; } = "Shop.IndexLolos";
        public string DisplayName { get; } = "Lolók lekérése";
        public string Description { get; } = "Az összes loló érme lekérése és listázása";
        public bool Dangerous { get; } = false;
    }

    public class IndexOwnLoloRequests : IPermission
    {
        public string Name { get; } = "Shop.IndexOwnLoloRequests";
        public string DisplayName { get; } = "Saját loló kérvények lekérése";
        public string Description { get; } = "A saját loló kérvények lekérése és listázása";
        public bool Dangerous { get; } = false;
    }

    public class IndexLoloRequests : IPermission
    {
        public string Name { get; } = "Shop.IndexLoloRequests";
        public string DisplayName { get; } = "Loló kérvények lekérése";
        public string Description { get; } = "Az összes loló kérvény lekérése és listázása";
        public bool Dangerous { get; } = false;
    }

    public class ViewLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.ViewLoloRequest";
        public string DisplayName { get; } = "Loló kérvény megtekintése";
        public string Description { get; } = "Egy adott loló kérvény lekérése és megtekintése id alapján";
        public bool Dangerous { get; } = false;
    }

    public class CreateLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.CreateLoloRequest";
        public string DisplayName { get; } = "Loló kérvény létrehozása";
        public string Description { get; } = "Új saját loló kérvény létrehozása";
        public bool Dangerous { get; } = false;
    }

    public class OverruleLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.OverruleLoloRequest";
        public string DisplayName { get; } = "Loló kérvény felülbírálása";
        public string Description { get; } = "Egy adott loló kérvény felülbírálása id alapján";
        public bool Dangerous { get; } = true;
    }

    public class UpdateOwnLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.UpdateOwnLoloRequest";
        public string DisplayName { get; } = "Loló kérvény módosítása";
        public string Description { get; } = "Egy adott saját loló kérvény módosítása id alapján";
        public bool Dangerous { get; } = false;
    }

    public class UpdateLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.UpdateLoloRequest";
        public string DisplayName { get; } = "Loló kérvény módosítása";
        public string Description { get; } = "Egy adott loló kérvény módosítása id alapján";
        public bool Dangerous { get; } = true;
    }

    public class DeleteOwnLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.DeleteOwnLoloRequest";
        public string DisplayName { get; } = "Loló kérvény törlése";
        public string Description { get; } = "Egy adott saját loló kérvény törlése id alapján";
        public bool Dangerous { get; } = false;
    }

    public class DeleteLoloRequest : IPermission
    {
        public string Name { get; } = "Shop.DeleteLoloRequest";
        public string DisplayName { get; } = "Loló kérvény törlése";
        public string Description { get; } = "Egy adott loló kérvény törlése id alapján";
        public bool Dangerous { get; } = true;
    }

    public class IndexQRCodes : IPermission
    {
        public string Name { get; } = "Shop.IndexQRCodes";
        public string DisplayName { get; } = "QR kódok lekérése";
        public string Description { get; } = "Az összes QR kód lekérése és listázása";
        public bool Dangerous { get; } = false;
    }

    public class ViewQRCode : IPermission
    {
        public string Name { get; } = "Shop.ViewQRCode";
        public string DisplayName { get; } = "QR kód megtekintése";
        public string Description { get; } = "Egy adott QR kód lekérése és megtekintése id alapján";
        public bool Dangerous { get; } = false;
    }

    public class CreateQRCode : IPermission
    {
        public string Name { get; } = "Shop.CreateQRCode";
        public string DisplayName { get; } = "QR kód létrehozása";
        public string Description { get; } = "Új QR kód létrehozása";
        public bool Dangerous { get; } = false;
    }

    public class UpdateQRCode : IPermission
    {
        public string Name { get; } = "Shop.UpdateQRCode";
        public string DisplayName { get; } = "QR kód módosítása";
        public string Description { get; } = "Egy adott QR kód módosítása id alapján";
        public bool Dangerous { get; } = true;
    }

    public class DeleteQRCode : IPermission
    {
        public string Name { get; } = "Shop.DeleteQRCode";
        public string DisplayName { get; } = "QR kód törlése";
        public string Description { get; } = "Egy adott QR kód törlése id alapján";
        public bool Dangerous { get; } = true;
    }

    public class IndexProducts : IPermission
    {
        public string Name { get; } = "Shop.IndexProducts";
        public string DisplayName { get; } = "Termékek lekérése";
        public string Description { get; } = "Az összes termék lekérése és listázása";
        public bool Dangerous { get; } = false;
    }

    public class IndexStoreProducts : IPermission
    {
        public string Name { get; } = "Shop.IndexStoreProducts";
        public string DisplayName { get; } = "Látható termékek lekérése";
        public string Description { get; } = "Az összes látható termék lekérése és listázása";
        public bool Dangerous { get; } = false;
    }

    public class ViewProduct : IPermission
    {
        public string Name { get; } = "Shop.ViewProduct";
        public string DisplayName { get; } = "Termék megtekintése";
        public string Description { get; } = "Egy adott termék lekérése és megtekintése id alapján (bármelyik termék)";
        public bool Dangerous { get; } = false;
    }

    public class ViewStoreProduct : IPermission
    {
        public string Name { get; } = "Shop.ViewStoreProduct";
        public string DisplayName { get; } = "Látható termék megtekintése";

        public string Description { get; } =
            "Egy adott látható termék lekérése és megtekintése id alapján (nem bármelyik termék)";

        public bool Dangerous { get; } = false;
    }

    public class CreateProduct : IPermission
    {
        public string Name { get; } = "Shop.CreateProduct";
        public string DisplayName { get; } = "Termék létrehozása";
        public string Description { get; } = "Új termék létrehozása";
        public bool Dangerous { get; } = true;
    }

    public class UpdateProduct : IPermission
    {
        public string Name { get; } = "Shop.UpdateProduct";
        public string DisplayName { get; } = "Termék módosítása";
        public string Description { get; } = "Egy adott termék módosítása id alapján";
        public bool Dangerous { get; } = true;
    }

    public class DeleteProduct : IPermission
    {
        public string Name { get; } = "Shop.DeleteProduct";
        public string DisplayName { get; } = "Termék törlése";
        public string Description { get; } = "Egy adott termék törlése id alapján";
        public bool Dangerous { get; } = true;
    }

    public class BuyProduct : IPermission
    {
        public string Name { get; } = "Shop.BuyProduct";
        public string DisplayName { get; } = "Termék vásárlása";
        public string Description { get; } = "Egy adott (csak látható) termék vásárlása id alapján";
        public bool Dangerous { get; } = false;
    }

    public class IndexOwnedItems : IPermission
    {
        public string Name { get; } = "Shop.IndexOwnedItems";
        public string DisplayName { get; } = "Birtokolt termékek lekérése";
        public string Description { get; } = "Az összes (bárki által) birtokolt termék lekérése és listázása";
        public bool Dangerous { get; } = false;
    }

    public class IndexOwnOwnedItems : IPermission
    {
        public string Name { get; } = "Shop.IndexOwnOwnedItems";
        public string DisplayName { get; } = "Saját birtokolt termékek lekérése";
        public string Description { get; } = "Az összes saját birtokolt termék lekérése és listázása";
        public bool Dangerous { get; } = false;
    }

    public class ViewOwnedItem : IPermission
    {
        public string Name { get; } = "Shop.ViewOwnedItem";
        public string DisplayName { get; } = "Birtokolt termék megtekintése";

        public string Description { get; } =
            "Egy adott (bárki által) birtokolt termék lekérése és megtekintése id alapján";

        public bool Dangerous { get; } = false;
    }

    public class ViewOwnOwnedItem : IPermission
    {
        public string Name { get; } = "Shop.ViewOwnOwnedItem";
        public string DisplayName { get; } = "Saját birtokolt termék megtekintése";
        public string Description { get; } = "Egy adott saját birtokolt termék lekérése és megtekintése id alapján";
        public bool Dangerous { get; } = false;
    }

    public class CreateOwnedItem : IPermission
    {
        public string Name { get; } = "Shop.CreateOwnedItem";
        public string DisplayName { get; } = "Birtokolt termék létrehozása";
        public string Description { get; } = "Új birtokolt termék létrehozása (bárki részére)";
        public bool Dangerous { get; } = true;
    }

    public class UpdateOwnedItem : IPermission
    {
        public string Name { get; } = "Shop.UpdateOwnedItem";
        public string DisplayName { get; } = "Birtokolt termék módosítása";
        public string Description { get; } = "Egy adott (bárki által) birtokolt termék módosítása id alapján";
        public bool Dangerous { get; } = true;
    }

    public class DeleteOwnedItem : IPermission
    {
        public string Name { get; } = "Shop.DeleteOwnedItem";
        public string DisplayName { get; } = "Birtokolt termék törlése";
        public string Description { get; } = "Egy adott (bárki által) birtokolt termék törlése id alapján";
        public bool Dangerous { get; } = true;
    }

    public class DeleteOwnOwnedItem : IPermission
    {
        public string Name { get; } = "Shop.DeleteOwnOwnedItem";
        public string DisplayName { get; } = "Saját birtokolt termék törlése";
        public string Description { get; } = "Egy adott saját birtokolt termék törlése id alapján";
        public bool Dangerous { get; } = false;
    }

    public class UseOwnOwnedItem : IPermission
    {
        public string Name { get; } = "Shop.UseOwnOwnedItem";
        public string DisplayName { get; } = "Saját birtokolt termék felhasználása";
        public string Description { get; } = "Egy adott saját birtokolt termék felhasználása id alapján";
        public bool Dangerous { get; } = false;
    }
}