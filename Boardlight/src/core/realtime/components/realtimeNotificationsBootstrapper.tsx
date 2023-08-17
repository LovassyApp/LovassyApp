import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { ReactNode, useEffect, useState } from "react";

import { FullScreenLoading } from "../../components/fullScreenLoading";
import { getGetApiGradesQueryKey } from "../../../api/generated/features/grades/grades";
import { getGetApiLolosOwnQueryKey } from "../../../api/generated/features/lolos/lolos";
import { getGetApiProductsQueryKey } from "../../../api/generated/features/products/products";
import { useAuthStore } from "../../stores/authStore";
import { useQueryClient } from "@tanstack/react-query";

export const RealtimeNotificationsBootstrapper = ({ children }: { children: ReactNode }): JSX.Element => {
    const accessToken = useAuthStore((state) => state.accessToken);

    const queryClient = useQueryClient();

    const productsQueryKey = getGetApiProductsQueryKey();
    const gradesQueryKey = getGetApiGradesQueryKey();
    const lolosQueryKey = getGetApiLolosOwnQueryKey();

    const [loading, setLoading] = useState(false);

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

                hubConnection.on("RefreshGrades", () => {
                    console.log("RefreshGrades notification received");
                    queryClient.invalidateQueries({ queryKey: [gradesQueryKey[0]] });
                });

                hubConnection.on("RefreshLolos", () => {
                    console.log("RefreshLolos notification received");
                    queryClient.invalidateQueries({ queryKey: [lolosQueryKey[0]] });
                });

                try {
                    setLoading(true);
                    await hubConnection.start();
                    console.log("Connected to realtime notifications");
                } catch (err) {
                    console.error(err);
                } finally {
                    setLoading(false);
                }
            }
        })();

        return () => {
            if (hubConnection) {
                hubConnection.stop();
                console.log("Disconnected from realtime notifications");
            }
        };
    }, [accessToken]);

    if (loading) return <FullScreenLoading />;

    return <>{children}</>;
};
