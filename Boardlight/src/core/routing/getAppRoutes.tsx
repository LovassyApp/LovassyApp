import { getAuthRoutes } from "../../features/auth/getAuthRoutes";
import { getBaseRoutes } from "../../features/base/getBaseRoutes";
import { getImageVotingsRoutes } from "../../features/imageVotings/getImageVotingsRoutes";
import { getImportRoutes } from "../../features/import/getImportRoutes";
import { getSchoolRoutes } from "../../features/school/getSchoolRoutes";
import { getShopRoutes } from "../../features/shop/getShopRoutes";
import { getUsersRoutes } from "../../features/users/getUsersRoutes";

export const getAppRoutes = () => {
    const baseRoutes = getBaseRoutes();
    const authRoutes = getAuthRoutes();
    const schoolRoutes = getSchoolRoutes();
    const shopRoutes = getShopRoutes();
    const importRoutes = getImportRoutes();
    const usersRoutes = getUsersRoutes();
    const imageVotingRoutes = getImageVotingsRoutes();

    return (
        <>
            {baseRoutes}
            {authRoutes}
            {schoolRoutes}
            {shopRoutes}
            {importRoutes}
            {usersRoutes}
            {imageVotingRoutes}
        </>
    );
};
