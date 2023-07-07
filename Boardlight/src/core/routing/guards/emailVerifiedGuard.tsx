import { Navigate, Outlet } from "react-router-dom";

import { FullScreenLoading } from "../../components/fullScreenLoading";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";

export const EmailVerifiedGuard = ({ redirect = "/auth/email-unverified" }: { redirect?: string }) => {
    const control = useGetApiAuthControl({ query: { retry: 0, enabled: false } }); // Disabled because the authenticated guard will handle the loading state

    if (control.isLoading) return <FullScreenLoading />;

    return control.isSuccess && control.data.user.emailVerifiedAt ? (
        <Outlet />
    ) : (
        <Navigate to={redirect} replace={true} />
    );
};
