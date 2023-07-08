import { AuthenticatedGuard } from "../../core/routing/guards/authenticatedGuard";
import { EmailVerifiedGuard } from "../../core/routing/guards/emailVerifiedGuard";
import { Route } from "react-router-dom";
import { lazy } from "react";

export const useSchoolRoutes = () => {
    const AuthenticatedLayout = lazy(() => import("../../core/routing/layouts/authenticatedLayout"));

    const GradesPage = lazy(() => import("./pages/gradesPage"));

    return (
        <>
            <Route element={<AuthenticatedGuard />}>
                <Route element={<EmailVerifiedGuard />}>
                    <Route element={<AuthenticatedLayout />}>
                        <Route path="/school/grades" element={<GradesPage />} />
                    </Route>
                </Route>
            </Route>
        </>
    );
};
