import { Navigate, Outlet } from "react-router-dom";
import { Suspense, lazy } from "react";

import { FullScreenLoading } from "../../components/fullScreenLoading";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";

export const FeatureGuard = ({ features }: { features: string[] }) => {
    const control = useGetApiAuthControl({ query: { retry: 0, enabled: false } }); // Disabled because the authenticated guard will handle the loading state

    const FourOFour = lazy(() => import("../../../features/base/pages/fourOFour"));

    if (control.isLoading) return <FullScreenLoading />;

    return control.isSuccess && control.data.features.some((feature) => features.includes(feature)) ? (
        <Outlet />
    ) : (
        <Suspense fallback={<FullScreenLoading />}>
            <FourOFour />
        </Suspense>
    );
};
