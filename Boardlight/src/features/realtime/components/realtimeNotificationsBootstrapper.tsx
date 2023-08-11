import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { ReactNode, useEffect } from "react";

import { getGetApiProductsQueryKey } from "../../../api/generated/features/products/products";
import { useAuthStore } from "../../../core/stores/authStore";
import { useQueryClient } from "@tanstack/react-query";

export const RealtimeNotificationsBootstrapper = ({ children }: { children: ReactNode }): JSX.Element => {
    const accessToken = useAuthStore((state) => state.accessToken);

    const queryClient = useQueryClient();

    const productsQueryKey = getGetApiProductsQueryKey();

    useEffect(() => {
        let hubConnection: HubConnection | undefined;

        if (accessToken && accessToken !== "") {
            hubConnection = new HubConnectionBuilder()
                .withUrl("/blueboard/Hubs/Notifications", { accessTokenFactory: () => accessToken })
                .configureLogging(LogLevel.Information)
                .build();
        }

        (async () => {
            if (hubConnection) {
                hubConnection.on("RefreshProducts", () => {
                    console.log("RefreshProducts notification received");
                    queryClient.invalidateQueries({ queryKey: [productsQueryKey[0]] });
                });

                try {
                    await hubConnection.start();
                    console.log("Connected to realtime notifications");
                } catch (err) {
                    console.error(err);
                }
            }

            return () => {
                if (hubConnection) {
                    hubConnection.stop();
                    console.log("Disconnected from realtime notifications");
                }
            };
        })();
    }, [accessToken]);

    return <>{children}</>;
};
