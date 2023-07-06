import { Route } from "react-router-dom";
import { lazy } from "react";

export const useAuthRoutes = () => {
    const LoginPage = lazy(() => import("./pages/loginPage"));
    const RegisterPage = lazy(() => import("./pages/registerPage"));

    return (
        <>
            <Route path="/auth/login" element={<LoginPage />} />
            <Route path="/auth/register" element={<RegisterPage />} />
        </>
    );
};
