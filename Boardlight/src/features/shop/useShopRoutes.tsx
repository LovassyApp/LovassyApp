import { AuthenticatedGuard } from "../../core/routing/guards/authenticatedGuard";
import { EmailVerifiedGuard } from "../../core/routing/guards/emailVerifiedGuard";
import { FeatureGuard } from "../../core/routing/guards/featureGuard";
import { PermissionGuard } from "../../core/routing/guards/permissionGuard";
import { Route } from "react-router-dom";
import { lazy } from "react";

export const useShopRoutes = () => {
    const AuthenticatedLayout = lazy(() => import("../../core/routing/layouts/authenticatedLayout"));

    const CoinsPage = lazy(() => import("./pages/coinsPage"));

    return (
        <>
            <Route element={<AuthenticatedGuard />}>
                <Route element={<EmailVerifiedGuard />}>
                    <Route element={<AuthenticatedLayout />}>
                        <Route element={<FeatureGuard features={["Shop"]} />}>
                            <Route element={<PermissionGuard permissions={["Shop.IndexOwnLolos"]} />}>
                                <Route path="/shop/own-coins" element={<CoinsPage />} />
                            </Route>
                        </Route>
                    </Route>
                </Route>
            </Route>
        </>
    );
};
