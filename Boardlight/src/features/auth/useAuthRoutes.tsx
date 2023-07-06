import { AuthenticatedGuard } from "../../core/components/guards/authenticatedGuard";
import { EmailUnverifiedGuard } from "../../core/components/guards/emailUnverifiedGuard";
import { Route } from "react-router-dom";
import { TippsLayout } from "../../core/routing/layouts/tippsLayout";
import { UnauthenticatedGuard } from "../../core/components/guards/unauthenticatedGuard";
import { lazy } from "react";

export const useAuthRoutes = () => {
    const LoginPage = lazy(() => import("./pages/loginPage"));
    const RegisterPage = lazy(() => import("./pages/registerPage"));
    const ForgotPasswordPage = lazy(() => import("./pages/forgotPasswordPage"));
    const ResetPasswordPage = lazy(() => import("./pages/resetPasswordPage"));
    const EmailUnverifiedPage = lazy(() => import("./pages/emailUnverifiedPage"));
    const VerifyEmailPage = lazy(() => import("./pages/verifyEmailPage"));

    return (
        <>
            <Route element={<UnauthenticatedGuard />}>
                <Route element={<TippsLayout />}>
                    <Route path="/auth/login" element={<LoginPage />} />
                </Route>
                <Route path="/auth/register" element={<RegisterPage />} />
                <Route path="/auth/forgot-password" element={<ForgotPasswordPage />} />
                <Route path="/auth/reset-password" element={<ResetPasswordPage />} />
            </Route>
            <Route element={<AuthenticatedGuard />}>
                <Route element={<EmailUnverifiedGuard />}>
                    <Route path="/auth/email-unverified" element={<EmailUnverifiedPage />} />
                </Route>
            </Route>
            <Route path="/auth/verify-email" element={<VerifyEmailPage />} />
        </>
    );
};
