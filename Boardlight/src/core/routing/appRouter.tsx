import { FullScreenLoading } from "../components/fullScreenLoading";
import { Routes } from "react-router-dom";
import { Suspense } from "react";
import { useAuthRoutes } from "../../features/auth/useAuthRoutes";
import { useBaseRoutes } from "../../features/base/useBaseRoutes";

export const AppRouter = () => {
    const baseRoutes = useBaseRoutes();
    const authRoutes = useAuthRoutes();

    return (
        <Suspense fallback={<FullScreenLoading />}>
            <Routes>
                {baseRoutes}
                {authRoutes}
            </Routes>
        </Suspense>
    );
};
