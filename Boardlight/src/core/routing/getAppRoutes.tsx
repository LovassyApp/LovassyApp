import { getAuthRoutes } from "../../features/auth/getAuthRoutes";
import { getBaseRoutes } from "../../features/base/getBaseRoutes";
import { getImportRoutes } from "../../features/import/getImportRoutes";
import { getSchoolRoutes } from "../../features/school/getSchoolRoutes";
import { getShopRoutes } from "../../features/shop/getShopRoutes";

export const getAppRoutes = () => {
    const baseRoutes = getBaseRoutes();
    const authRoutes = getAuthRoutes();
    const schoolRoutes = getSchoolRoutes();
    const shopRoutes = getShopRoutes();
    const importRoutes = getImportRoutes();

    return (
        <>
            {baseRoutes}
            {authRoutes}
            {schoolRoutes}
            {shopRoutes}
            {importRoutes}
        </>
    );
};
