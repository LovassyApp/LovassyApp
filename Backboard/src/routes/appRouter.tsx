import { Route, Routes } from "react-router-dom";
import { Suspense, lazy } from "react";

import { FullScreenLoading } from "../components/fullScreenLoading";

export const AppRouter = () => {
    const WindowLayout = lazy(() => import("./layouts/windowLayout"));

    const GradeImportPage = lazy(() => import("./pages/gradeImportPage"));
    const SecurityPage = lazy(() => import("./pages/securityPage"));
    const SettingsPage = lazy(() => import("./pages/settingsPage"));
    const StatusPage = lazy(() => import("./pages/statusPage"));

    return (
        <Suspense fallback={<FullScreenLoading />}>
            <Routes>
                <Route element={<WindowLayout />}>
                    <Route path="/" element={<GradeImportPage />} />
                    <Route path="/security" element={<SecurityPage />} />
                    <Route path="/settings" element={<SettingsPage />} />
                    <Route path="/status" element={<StatusPage />} />
                </Route>
            </Routes>
        </Suspense>
    );
};
