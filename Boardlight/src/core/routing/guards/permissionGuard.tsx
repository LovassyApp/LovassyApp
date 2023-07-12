import { Navigate, Outlet } from "react-router-dom";

import { FullScreenLoading } from "../../components/fullScreenLoading";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";

export const PermissionGuard = ({ permissions, redirect = "/" }: { permissions: string[]; redirect?: string }) => {
    const control = useGetApiAuthControl({ query: { retry: 0, enabled: false } }); // Disabled because the authenticated guard will handle the loading state

    if (control.isLoading) return <FullScreenLoading />;

    return control.isSuccess && control.data.permissions.some((permission) => permissions.includes(permission)) ? (
        <Outlet />
    ) : (
        <Navigate to={redirect} replace={true} />
    );
};
