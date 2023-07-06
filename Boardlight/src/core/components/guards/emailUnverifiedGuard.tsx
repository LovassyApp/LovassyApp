import { Navigate, Outlet } from "react-router-dom";

import { FullScreenLoading } from "../fullScreenLoading";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";

export const EmailUnverifiedGuard = ({ redirect = "/" }: { redirect?: string }) => {
    const control = useGetApiAuthControl({ query: { retry: 0 } });

    if (control.isLoading) return <FullScreenLoading />;

    return control.isSuccess && !control.data.user.emailVerifiedAt ? (
        <Outlet />
    ) : (
        <Navigate to={redirect} replace={true} />
    );
};
