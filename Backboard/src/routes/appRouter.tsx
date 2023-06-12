import { Route, Routes } from "react-router-dom";
import { Suspense, lazy } from "react";

import { FullScreenLoading } from "../components/fullScreenLoading";

export const AppRouter = () => {
    const WindowLayout = lazy(() => import("./layouts/windowLayout"));

    const GradeImportPage = lazy(() => import("./pages/gradeImportPage"));
    const ResetKeyPasswordPage = lazy(() => import("./pages/resetKeyPasswordPage"));
    const SettingsPage = lazy(() => import("./pages/settingsPage"));

    return (
        <Suspense fallback={<FullScreenLoading />}>
            <Routes>
                <Route element={<WindowLayout />}>
                    <Route path="/" element={<GradeImportPage />} />
                    <Route path="/resetKeyPassword" element={<ResetKeyPasswordPage />} />
                    <Route path="/settings" element={<SettingsPage />} />
                </Route>
            </Routes>
        </Suspense>
    );
};
