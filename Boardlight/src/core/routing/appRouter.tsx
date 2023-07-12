import { FullScreenLoading } from "../components/fullScreenLoading";
import { Routes } from "react-router-dom";
import { Suspense } from "react";
import { useAuthRoutes } from "../../features/auth/useAuthRoutes";
import { useBaseRoutes } from "../../features/base/useBaseRoutes";
import { useSchoolRoutes } from "../../features/school/useSchoolRoutes";
import { useShopRoutes } from "../../features/shop/useShopRoutes";

export const AppRouter = () => {
    const baseRoutes = useBaseRoutes();
    const authRoutes = useAuthRoutes();
    const schoolRoutes = useSchoolRoutes();
    const shopRoutes = useShopRoutes();

    return (
        <Suspense fallback={<FullScreenLoading />}>
            <Routes>
                {baseRoutes}
                {authRoutes}
                {schoolRoutes}
                {shopRoutes}
            </Routes>
        </Suspense>
    );
};
