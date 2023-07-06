import { AuthenticatedGuard } from "../../core/components/guards/authenticatedGuard";
import { Route } from "react-router-dom";
import { lazy } from "react";

export const useBaseRoutes = () => {
    const HomePage = lazy(() => import("./pages/homePage"));

    return (
        <>
            <Route element={<AuthenticatedGuard />}>
                <Route path="/" element={<HomePage />} />
            </Route>
        </>
    );
};
