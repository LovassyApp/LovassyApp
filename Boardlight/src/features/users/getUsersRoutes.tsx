import { AuthenticatedGuard } from "../../core/routing/guards/authenticatedGuard";
import { EmailVerifiedGuard } from "../../core/routing/guards/emailVerifiedGuard";
import { FeatureGuard } from "../../core/routing/guards/featureGuard";
import { PermissionGuard } from "../../core/routing/guards/permissionGuard";
import { Route } from "react-router-dom";
import { lazy } from "react";

export const getUsersRoutes = () => {
    const AuthenticatedLayout = lazy(() => import("../../core/routing/layouts/authenticatedLayout"));

    const UsersPage = lazy(() => import("./pages/usersPage"));

    return (
        <>
            <Route element={<AuthenticatedGuard />}>
                <Route element={<EmailVerifiedGuard />}>
                    <Route element={<AuthenticatedLayout />}>
                        <Route element={<FeatureGuard features={["Users"]} />}>
                            <Route element={<PermissionGuard permissions={["Users.IndexUsers"]} />}>
                                <Route path="/users" element={<UsersPage />} />
                            </Route>
                        </Route>
                    </Route>
                </Route>
            </Route>
        </>
    );
};
