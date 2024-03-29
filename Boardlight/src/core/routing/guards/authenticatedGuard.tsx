import { Navigate, Outlet } from "react-router-dom";

import { FullScreenLoading } from "../../components/fullScreenLoading";
import { useAuthStore } from "../../stores/authStore";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";

export const AuthenticatedGuard = ({ redirect = "/auth/login" }: { redirect?: string }) => {
    const accessToken = useAuthStore((state) => state.accessToken);
    const control = useGetApiAuthControl({ query: { retry: 0, enabled: !!accessToken } });

    if (control.isInitialLoading) return <FullScreenLoading />;

    return accessToken && control.isSuccess ? <Outlet /> : <Navigate to={redirect} replace={true} />;
};
