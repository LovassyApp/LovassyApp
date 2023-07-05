import { FullScreenLoading } from "../components/fullScreenLoading";
import { Routes } from "react-router-dom";
import { Suspense } from "react";
import { useAuthRoutes } from "../../features/auth/useAuthRoutes";

export const AppRouter = () => {
    const authRoutes = useAuthRoutes();

    return (
        <Suspense fallback={<FullScreenLoading />}>
            <Routes>
                {authRoutes}
            </Routes>
        </Suspense>
    );
};
