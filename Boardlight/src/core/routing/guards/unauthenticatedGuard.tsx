import { Navigate, Outlet } from "react-router-dom";

import { FullScreenLoading } from "../../components/fullScreenLoading";
import { useAuthStore } from "../../stores/authStore";
import { useEffect } from "react";
import { usePostApiAuthRefresh } from "../../../api/generated/features/auth/auth";

export const UnauthenticatedGuard = ({ redirect = "/" }: { redirect?: string }) => {
    const accessToken = useAuthStore((state) => state.accessToken);
    const setAccessToken = useAuthStore((state) => state.setAccessToken);
    const refresh = usePostApiAuthRefresh();

    useEffect(() => {
        (async () => {
            if (!accessToken) {
                try {
                    console.log("Attempting to refresh token");
                    const res = await refresh.mutateAsync({});
                    setAccessToken(res.token);
                } catch (err) {
                    console.log("Failed to refresh token");
                }
            }
        })();
    }, [accessToken]);

    if (refresh.isLoading) return <FullScreenLoading />;

    return accessToken ? <Navigate to={redirect} replace={true} /> : <Outlet />;
};
