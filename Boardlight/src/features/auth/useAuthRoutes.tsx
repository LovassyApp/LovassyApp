import { Route } from "react-router-dom";
import { TippsLayout } from "../../core/routing/layouts/tippsLayout";
import { UnauthenticatedGuard } from "../../core/components/guards/unauthenticatedGuard";
import { lazy } from "react";

export const useAuthRoutes = () => {
    const LoginPage = lazy(() => import("./pages/loginPage"));
    const RegisterPage = lazy(() => import("./pages/registerPage"));

    return (
        <>
            <Route element={<UnauthenticatedGuard />}>
                <Route element={<TippsLayout />}>
                    <Route path="/auth/login" element={<LoginPage />} />
                    <Route path="/auth/register" element={<RegisterPage />} />
                </Route>
            </Route>
        </>
    );
};
