import { AuthenticatedGuard } from "../../core/routing/guards/authenticatedGuard";
import { EmailVerifiedGuard } from "../../core/routing/guards/emailVerifiedGuard";
import { Route } from "react-router-dom";
import { lazy } from "react";

export const useBaseRoutes = () => {
    const AuthenticatedLayout = lazy(() => import("../../core/routing/layouts/authenticatedLayout"));

    const HomePage = lazy(() => import("./pages/homePage"));

    return (
        <>
            <Route element={<AuthenticatedGuard />}>
                <Route element={<EmailVerifiedGuard />}>
                    <Route element={<AuthenticatedLayout />}>
                        <Route path="/" element={<HomePage />} />
                    </Route>
                </Route>
            </Route>
        </>
    );
};
