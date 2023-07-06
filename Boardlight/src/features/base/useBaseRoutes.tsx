import { AuthenticatedGuard } from "../../core/components/guards/authenticatedGuard";
import { EmailVerifiedGuard } from "../../core/components/guards/emailVerifiedGuard";
import { Route } from "react-router-dom";
import { lazy } from "react";

export const useBaseRoutes = () => {
    const HomePage = lazy(() => import("./pages/homePage"));

    return (
        <>
            <Route element={<AuthenticatedGuard />}>
                <Route element={<EmailVerifiedGuard />}>
                    <Route path="/" element={<HomePage />} />
                </Route>
            </Route>
        </>
    );
};
