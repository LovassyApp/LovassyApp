using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public class ShopPermissions
{
    public class IndexOwnLolos : IPermission
    {
        public string Name => "Shop.IndexOwnLolos";
        public string DisplayName => "Saját lolók lekérése";
        public string Description => "A saját loló mennyiség és loló érmék lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexLolos : IPermission
    {
        public string Name => "Shop.IndexLolos";
        public string DisplayName => "Lolók lekérése";
        public string Description => "Az összes loló érme lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexOwnLoloRequests : IPermission
    {
        public string Name => "Shop.IndexOwnLoloRequests";
        public string DisplayName => "Saját loló kérvények lekérése";
        public string Description => "A saját loló kérvények lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexLoloRequests : IPermission
    {
        public string Name => "Shop.IndexLoloRequests";
        public string DisplayName => "Loló kérvények lekérése";
        public string Description => "Az összes loló kérvény lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewLoloRequest : IPermission
    {
        public string Name => "Shop.ViewLoloRequest";
        public string DisplayName => "Loló kérvény megtekintése";
        public string Description => "Egy adott loló kérvény lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class CreateLoloRequest : IPermission
    {
        public string Name => "Shop.CreateLoloRequest";
        public string DisplayName => "Loló kérvény létrehozása";
        public string Description => "Új saját loló kérvény létrehozása";
        public bool Dangerous => false;
    }

    public class OverruleLoloRequest : IPermission
    {
        public string Name => "Shop.OverruleLoloRequest";
        public string DisplayName => "Loló kérvény felülbírálása";
        public string Description => "Egy adott loló kérvény felülbírálása id alapján";
        public bool Dangerous => true;
    }

    public class UpdateOwnLoloRequest : IPermission
    {
        public string Name => "Shop.UpdateOwnLoloRequest";
        public string DisplayName => "Saját loló kérvény módosítása";
        public string Description => "Egy adott saját loló kérvény módosítása id alapján";
        public bool Dangerous => false;
    }

    public class UpdateLoloRequest : IPermission
    {
        public string Name => "Shop.UpdateLoloRequest";
        public string DisplayName => "Loló kérvény módosítása";
        public string Description => "Egy adott loló kérvény módosítása id alapján";
        public bool Dangerous => true;
    }

    public class DeleteOwnLoloRequest : IPermission
    {
        public string Name => "Shop.DeleteOwnLoloRequest";
        public string DisplayName => "Saját loló kérvény törlése";
        public string Description => "Egy adott saját loló kérvény törlése id alapján";
        public bool Dangerous => false;
    }

    public class DeleteLoloRequest : IPermission
    {
        public string Name => "Shop.DeleteLoloRequest";
        public string DisplayName => "Loló kérvény törlése";
        public string Description => "Egy adott loló kérvény törlése id alapján";
        public bool Dangerous => true;
    }

    public class IndexQRCodes : IPermission
    {
        public string Name => "Shop.IndexQRCodes";
        public string DisplayName => "QR kódok lekérése";
        public string Description => "Az összes QR kód lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewQRCode : IPermission
    {
        public string Name => "Shop.ViewQRCode";
        public string DisplayName => "QR kód megtekintése";
        public string Description => "Egy adott QR kód lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class CreateQRCode : IPermission
    {
        public string Name => "Shop.CreateQRCode";
        public string DisplayName => "QR kód létrehozása";
        public string Description => "Új QR kód létrehozása";
        public bool Dangerous => false;
    }

    public class UpdateQRCode : IPermission
    {
        public string Name => "Shop.UpdateQRCode";
        public string DisplayName => "QR kód módosítása";
        public string Description => "Egy adott QR kód módosítása id alapján";
        public bool Dangerous => true;
    }

    public class DeleteQRCode : IPermission
    {
        public string Name => "Shop.DeleteQRCode";
        public string DisplayName => "QR kód törlése";
        public string Description => "Egy adott QR kód törlése id alapján";
        public bool Dangerous => true;
    }

    public class IndexProducts : IPermission
    {
        public string Name => "Shop.IndexProducts";
        public string DisplayName => "Termékek lekérése";
        public string Description => "Az összes termék lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexStoreProducts : IPermission
    {
        public string Name => "Shop.IndexStoreProducts";
        public string DisplayName => "Látható termékek lekérése";
        public string Description => "Az összes látható termék lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewProduct : IPermission
    {
        public string Name => "Shop.ViewProduct";
        public string DisplayName => "Termék megtekintése";
        public string Description => "Egy adott termék lekérése és megtekintése id alapján (bármelyik termék)";
        public bool Dangerous => false;
    }

    public class ViewStoreProduct : IPermission
    {
        public string Name => "Shop.ViewStoreProduct";
        public string DisplayName => "Látható termék megtekintése";

        public string Description =>
            "Egy adott látható termék lekérése és megtekintése id alapján (nem bármelyik termék)";

        public bool Dangerous => false;
    }

    public class CreateProduct : IPermission
    {
        public string Name => "Shop.CreateProduct";
        public string DisplayName => "Termék létrehozása";
        public string Description => "Új termék létrehozása";
        public bool Dangerous => true;
    }

    public class UpdateProduct : IPermission
    {
        public string Name => "Shop.UpdateProduct";
        public string DisplayName => "Termék módosítása";
        public string Description => "Egy adott termék módosítása id alapján";
        public bool Dangerous => true;
    }

    public class DeleteProduct : IPermission
    {
        public string Name => "Shop.DeleteProduct";
        public string DisplayName => "Termék törlése";
        public string Description => "Egy adott termék törlése id alapján";
        public bool Dangerous => true;
    }

    public class BuyProduct : IPermission
    {
        public string Name => "Shop.BuyProduct";
        public string DisplayName => "Termék vásárlása";
        public string Description => "Egy adott (csak látható) termék vásárlása id alapján";
        public bool Dangerous => false;
    }

    public class IndexOwnedItems : IPermission
    {
        public string Name => "Shop.IndexOwnedItems";
        public string DisplayName => "Birtokolt termékek lekérése";
        public string Description => "Az összes (bárki által) birtokolt termék lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexOwnOwnedItems : IPermission
    {
        public string Name => "Shop.IndexOwnOwnedItems";
        public string DisplayName => "Saját birtokolt termékek lekérése";
        public string Description => "Az összes saját birtokolt termék lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewOwnedItem : IPermission
    {
        public string Name => "Shop.ViewOwnedItem";
        public string DisplayName => "Birtokolt termék megtekintése";

        public string Description => "Egy adott (bárki által) birtokolt termék lekérése és megtekintése id alapján";

        public bool Dangerous => false;
    }

    public class ViewOwnOwnedItem : IPermission
    {
        public string Name => "Shop.ViewOwnOwnedItem";
        public string DisplayName => "Saját birtokolt termék megtekintése";
        public string Description => "Egy adott saját birtokolt termék lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class CreateOwnedItem : IPermission
    {
        public string Name => "Shop.CreateOwnedItem";
        public string DisplayName => "Birtokolt termék létrehozása";
        public string Description => "Új birtokolt termék létrehozása (bárki részére)";
        public bool Dangerous => true;
    }

    public class UpdateOwnedItem : IPermission
    {
        public string Name => "Shop.UpdateOwnedItem";
        public string DisplayName => "Birtokolt termék módosítása";
        public string Description => "Egy adott (bárki által) birtokolt termék módosítása id alapján";
        public bool Dangerous => true;
    }

    public class DeleteOwnedItem : IPermission
    {
        public string Name => "Shop.DeleteOwnedItem";
        public string DisplayName => "Birtokolt termék törlése";
        public string Description => "Egy adott (bárki által) birtokolt termék törlése id alapján";
        public bool Dangerous => true;
    }

    public class DeleteOwnOwnedItem : IPermission
    {
        public string Name => "Shop.DeleteOwnOwnedItem";
        public string DisplayName => "Saját birtokolt termék törlése";
        public string Description => "Egy adott saját birtokolt termék törlése id alapján";
        public bool Dangerous => false;
    }

    public class UseOwnOwnedItem : IPermission
    {
        public string Name => "Shop.UseOwnOwnedItem";
        public string DisplayName => "Saját birtokolt termék felhasználása";
        public string Description => "Egy adott saját birtokolt termék felhasználása id alapján";
        public bool Dangerous => false;
    }
}