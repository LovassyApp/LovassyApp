import { Suspense, lazy } from "react";

import { FullScreenLoading } from "../../components/fullScreenLoading";
import { Outlet } from "react-router-dom";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";

export const PermissionGuard = ({ permissions }: { permissions: string[] }) => {
    const control = useGetApiAuthControl({ query: { retry: 0, enabled: false } }); // Disabled because the authenticated guard will handle the loading state

    const FourOFour = lazy(() => import("../../../features/base/pages/fourOFour"));

    if (control.isLoading) return <FullScreenLoading />;

    return control.isSuccess &&
        (control.data.permissions.some((permission) => permissions.includes(permission)) ||
            control.data.isSuperUser) ? (
        <Outlet />
    ) : (
        <Suspense fallback={<FullScreenLoading />}>
            <FourOFour />
        </Suspense>
    );
};
