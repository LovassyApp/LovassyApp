import { AuthenticatedGuard } from "../../core/routing/guards/authenticatedGuard";
import { EmailVerifiedGuard } from "../../core/routing/guards/emailVerifiedGuard";
import { Route } from "react-router-dom";
import { lazy } from "react";

export const getBaseRoutes = () => {
    const AuthenticatedLayout = lazy(() => import("../../core/routing/layouts/authenticatedLayout"));

    const HomePage = lazy(() => import("./pages/homePage"));
    const FourOFour = lazy(() => import("./pages/fourOFour"));

    return (
        <>
            <Route element={<AuthenticatedGuard />}>
                <Route element={<EmailVerifiedGuard />}>
                    <Route element={<AuthenticatedLayout />}>
                        <Route path="/" element={<HomePage />} />
                    </Route>
                </Route>
            </Route>
            <Route path="*" element={<FourOFour />} />
        </>
    );
};
