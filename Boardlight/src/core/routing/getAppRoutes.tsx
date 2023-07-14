import { getAuthRoutes } from "../../features/auth/getAuthRoutes";
import { getBaseRoutes } from "../../features/base/getBaseRoutes";
import { getSchoolRoutes } from "../../features/school/getSchoolRoutes";
import { getShopRoutes } from "../../features/shop/getShopRoutes";

export const getAppRoutes = () => {
    const baseRoutes = getBaseRoutes();
    const authRoutes = getAuthRoutes();
    const schoolRoutes = getSchoolRoutes();
    const shopRoutes = getShopRoutes();

    return (
        <>
            {baseRoutes}
            {authRoutes}
            {schoolRoutes}
            {shopRoutes}
        </>
    );
};
