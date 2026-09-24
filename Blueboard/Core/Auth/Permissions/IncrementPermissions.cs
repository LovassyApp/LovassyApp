using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public static class IncrementPermissions
{
    public class IncrementYear : IPermission
    {
        public string Name => "Increment.IncrementYear";
        public string DisplayName => "Év növelése";
        public string Description => "Az összes felhasználó összes jegyének, lolójának, és birtokolt kimentésének törlése";
        public bool Dangerous => true;
    }
}