import { Navigate, Outlet } from "react-router-dom";

import { useAuthStore } from "../../stores/authStore";

export const AuthenticatedGuard = ({ redirect = "/auth/login" }: { redirect?: string }) => {
    const accessToken = useAuthStore((state) => state.accessToken);

    return accessToken ? <Outlet /> : <Navigate to={redirect} replace={true} />;
};
