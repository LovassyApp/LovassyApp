import { ReactNode } from "react";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";

export const PermissionsConditional = ({
    permissions,
    fallback,
    children,
    fetchControl = false,
}: {
    permissions?: string[];
    fallback?: ReactNode;
    children: ReactNode;
    fetchControl?: boolean;
}): JSX.Element => {
    const control = useGetApiAuthControl({ query: { retry: 0, enabled: fetchControl } });

    if (control.isLoading) return undefined; // Shouldn't happen in theory

    if (!permissions) return <>{children}</>;

    if (
        control.isSuccess &&
        (control.data.permissions.some((permission) => permissions.includes(permission)) || control.data.isSuperUser)
    )
        return <>{children}</>;

    if (fallback) return <>{fallback}</>;

    return undefined;
};
