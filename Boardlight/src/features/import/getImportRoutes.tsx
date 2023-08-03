import { AuthenticatedGuard } from "../../core/routing/guards/authenticatedGuard";
import AuthenticatedLayout from "../../core/routing/layouts/authenticatedLayout";
import { EmailVerifiedGuard } from "../../core/routing/guards/emailVerifiedGuard";
import { FeatureGuard } from "../../core/routing/guards/featureGuard";
import { PermissionGuard } from "../../core/routing/guards/permissionGuard";
import { Route } from "react-router-dom";
import { lazy } from "react";

export const getImportRoutes = () => {
    const ImportKeysPage = lazy(() => import("./pages/importKeysPage"));

    return (
        <>
            <Route element={<AuthenticatedGuard />}>
                <Route element={<EmailVerifiedGuard />}>
                    <Route element={<AuthenticatedLayout />}>
                        <Route element={<FeatureGuard features={["Import"]} />}>
                            <Route element={<PermissionGuard permissions={["Import.IndexImportKeys"]} />}>
                                <Route path="/import/import-keys" element={<ImportKeysPage />} />
                            </Route>
                        </Route>
                    </Route>
                </Route>
            </Route>
        </>
    );
};
