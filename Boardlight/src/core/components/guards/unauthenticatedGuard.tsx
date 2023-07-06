import { Navigate, Outlet } from "react-router-dom";

import { useAuthStore } from "../../stores/authStore";

export const UnauthenticatedGuard = ({ redirect = "/" }: { redirect?: string }) => {
    const accessToken = useAuthStore((state) => state.accessToken);

    return accessToken ? <Navigate to={redirect} replace={true} /> : <Outlet />;
};
