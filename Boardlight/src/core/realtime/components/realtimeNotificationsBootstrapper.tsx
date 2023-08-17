import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { ReactNode, useEffect, useState } from "react";
import {
    getGetApiLoloRequestsOwnQueryKey,
    getGetApiLoloRequestsQueryKey,
} from "../../../api/generated/features/lolo-requests/lolo-requests";
import { getGetApiLolosOwnQueryKey, getGetApiLolosQueryKey } from "../../../api/generated/features/lolos/lolos";

import { FullScreenLoading } from "../../components/fullScreenLoading";
import { getGetApiGradesQueryKey } from "../../../api/generated/features/grades/grades";
import { getGetApiProductsQueryKey } from "../../../api/generated/features/products/products";
import { getGetApiQRCodesQueryKey } from "../../../api/generated/features/qrcodes/qrcodes";
import { useAuthStore } from "../../stores/authStore";
import { useQueryClient } from "@tanstack/react-query";

export const RealtimeNotificationsBootstrapper = ({ children }: { children: ReactNode }): JSX.Element => {
    const accessToken = useAuthStore((state) => state.accessToken);

    const queryClient = useQueryClient();

    const productsQueryKey = getGetApiProductsQueryKey();
    const gradesQueryKey = getGetApiGradesQueryKey();
    const ownLolosQueryKey = getGetApiLolosOwnQueryKey();
    const lolosQueryKey = getGetApiLolosQueryKey();
    const qrcodesQueryKey = getGetApiQRCodesQueryKey();
    const ownLoloRequestsQueryKey = getGetApiLoloRequestsOwnQueryKey();
    const loloRequestsQueryKey = getGetApiLoloRequestsQueryKey();

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
                hubConnection.on("RefreshProducts", async () => {
                    console.log("RefreshProducts notification received");
                    await queryClient.invalidateQueries({ queryKey: [productsQueryKey[0]] });
                });

                hubConnection.on("RefreshGrades", async () => {
                    console.log("RefreshGrades notification received");
                    await queryClient.invalidateQueries({ queryKey: [gradesQueryKey[0]] });
                });

                hubConnection.on("RefreshOwnLolos", async () => {
                    console.log("RefreshOwnLolos notification received");
                    await queryClient.invalidateQueries({ queryKey: [ownLolosQueryKey[0]] });
                });

                hubConnection.on("RefreshLolos", async () => {
                    console.log("RefreshLolos notification received");
                    await queryClient.invalidateQueries({ queryKey: [lolosQueryKey[0]] });
                });

                hubConnection.on("RefreshQRCodes", async () => {
                    console.log("RefreshQRCodes notification received");
                    await queryClient.invalidateQueries({ queryKey: [qrcodesQueryKey[0]] });
                });

                hubConnection.on("RefreshOwnLoloRequests", async () => {
                    console.log("RefreshOwnLoloRequests notification received");
                    await queryClient.invalidateQueries({ queryKey: [ownLoloRequestsQueryKey[0]] });
                });

                hubConnection.on("RefreshLoloRequests", async () => {
                    console.log("RefreshLoloRequests notification received");
                    await queryClient.invalidateQueries({ queryKey: [loloRequestsQueryKey[0]] });
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
