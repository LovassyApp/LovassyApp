import { Navigate, Outlet, useNavigate, useSearch } from "@tanstack/router";
import { ReactNode, useEffect } from "react";

import { useAuthStore } from "../../stores/authStore";

export const AuthenticatedGuard = ({ redirect = "/auth/login", children }: { redirect?: string, children: ReactNode }) => {
    const accessToken = useAuthStore((state) => state.accessToken);

    if (accessToken)
        return children;

    return <Navigate to={redirect} />;
};
