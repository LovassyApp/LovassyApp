import { AuthenticatedGuard } from "../../core/routing/guards/authenticatedGuard";
import { EmailVerifiedGuard } from "../../core/routing/guards/emailVerifiedGuard";
import { FeatureGuard } from "../../core/routing/guards/featureGuard";
import { PermissionGuard } from "../../core/routing/guards/permissionGuard";
import { Route } from "react-router-dom";
import { lazy } from "react";

export const getShopRoutes = () => {
    const AuthenticatedLayout = lazy(() => import("../../core/routing/layouts/authenticatedLayout"));

    const OwnCoinsPage = lazy(() => import("./pages/ownCoinsPage"));
    const CoinsPage = lazy(() => import("./pages/coinsPage"));
    const OwnLoloRequestsPage = lazy(() => import("./pages/ownLoloRequestsPage"));
    const LoloRequestsPage = lazy(() => import("./pages/loloRequestsPage"));
    const QRCodesPage = lazy(() => import("./pages/qrCodesPage"));
    const ShopPage = lazy(() => import("./pages/shopPage"));

    return (
        <>
            <Route element={<AuthenticatedGuard />}>
                <Route element={<EmailVerifiedGuard />}>
                    <Route element={<AuthenticatedLayout />}>
                        <Route element={<FeatureGuard features={["Shop"]} />}>
                            <Route element={<PermissionGuard permissions={["Shop.IndexOwnLolos"]} />}>
                                <Route path="/shop/own-coins" element={<OwnCoinsPage />} />
                            </Route>
                            <Route element={<PermissionGuard permissions={["Shop.IndexLolos"]} />}>
                                <Route path="/shop/coins" element={<CoinsPage />} />
                            </Route>
                            <Route element={<PermissionGuard permissions={["Shop.IndexOwnLoloRequests"]} />}>
                                <Route path="/shop/own-lolo-requests" element={<OwnLoloRequestsPage />} />
                            </Route>
                            <Route element={<PermissionGuard permissions={["Shop.IndexLoloRequests"]} />}>
                                <Route path="/shop/lolo-requests" element={<LoloRequestsPage />} />
                            </Route>
                            <Route element={<PermissionGuard permissions={["Shop.IndexQRCodes"]} />}>
                                <Route path="/shop/qr-codes" element={<QRCodesPage />} />
                            </Route>
                            <Route element={<PermissionGuard permissions={["Shop.IndexStoreProducts"]} />}>
                                <Route path="/shop" element={<ShopPage />} />
                            </Route>
                        </Route>
                    </Route>
                </Route>
            </Route>
        </>
    );
};
