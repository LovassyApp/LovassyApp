import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { ReactNode, useEffect, useState } from "react";
import {
    getGetApiLoloRequestsOwnQueryKey,
    getGetApiLoloRequestsQueryKey,
} from "../../../api/generated/features/lolo-requests/lolo-requests";
import { getGetApiLolosOwnQueryKey, getGetApiLolosQueryKey } from "../../../api/generated/features/lolos/lolos";
import {
    getGetApiOwnedItemsOwnQueryKey,
    getGetApiOwnedItemsQueryKey,
} from "../../../api/generated/features/owned-items/owned-items";

import { FullScreenLoading } from "../../components/fullScreenLoading";
import { getGetApiFeedItemsQueryKey } from "../../../api/generated/features/feed-items/feed-items";
import { getGetApiGradesQueryKey } from "../../../api/generated/features/grades/grades";
import { getGetApiImageVotingEntriesQueryKey } from "../../../api/generated/features/image-voting-entries/image-voting-entries";
import { getGetApiImageVotingsQueryKey } from "../../../api/generated/features/image-votings/image-votings";
import { getGetApiLoloRequestCreatedNotifiersQueryKey } from "../../../api/generated/features/lolo-request-created-notifiers/lolo-request-created-notifiers";
import { getGetApiProductsQueryKey } from "../../../api/generated/features/products/products";
import { getGetApiQRCodesQueryKey } from "../../../api/generated/features/qrcodes/qrcodes";
import { getGetApiUserGroupsQueryKey } from "../../../api/generated/features/user-groups/user-groups";
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
    const userGroupsQueryKey = getGetApiUserGroupsQueryKey();
    const ownOwnedItemsQueryKey = getGetApiOwnedItemsOwnQueryKey();
    const ownedItemsQueryKey = getGetApiOwnedItemsQueryKey();
    const feedItemsQueryKey = getGetApiFeedItemsQueryKey();
    const imageVotingsQueryKey = getGetApiImageVotingsQueryKey();
    const imageVotingEntriesQueryKey = getGetApiImageVotingEntriesQueryKey();
    const loloRequestCreatedNotifiersQueryKey = getGetApiLoloRequestCreatedNotifiersQueryKey();

    const [loading, setLoading] = useState(false);

    useEffect(() => {
        let hubConnection: HubConnection | undefined;

        if (accessToken && accessToken !== "") {
            hubConnection = new HubConnectionBuilder()
                .withUrl(`${import.meta.env.VITE_BLUEBOARD_URL ?? ""}/Hubs/Notifications`, {
                    accessTokenFactory: () => accessToken,
                })
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

                hubConnection.on("RefreshUserGroups", async () => {
                    console.log("RefreshUserGroups notification received");
                    await queryClient.invalidateQueries({ queryKey: [userGroupsQueryKey[0]] });
                });

                hubConnection.on("RefreshOwnOwnedItems", async () => {
                    console.log("RefreshOwnOwnedItems notification received");
                    await queryClient.invalidateQueries({ queryKey: [ownOwnedItemsQueryKey[0]] });
                });

                hubConnection.on("RefreshOwnedItems", async () => {
                    console.log("RefreshOwnedItems notification received");
                    await queryClient.invalidateQueries({ queryKey: [ownedItemsQueryKey[0]] });
                });

                hubConnection.on("RefreshFeedItems", async () => {
                    console.log("RefreshFeedItems notification received");
                    await queryClient.invalidateQueries({ queryKey: [feedItemsQueryKey[0]] });
                });

                hubConnection.on("RefreshImageVotings", async () => {
                    console.log("RefreshImageVotings notification received");
                    await queryClient.invalidateQueries({ queryKey: [imageVotingsQueryKey[0]] });
                });

                hubConnection.on("RefreshImageVotingEntries", async () => {
                    console.log("RefreshImageVotingEntries notification received");
                    await queryClient.invalidateQueries({ queryKey: [imageVotingEntriesQueryKey[0]] });
                });

                hubConnection.on("RefreshLoloRequestCreatedNotifiers", async () => {
                    console.log("RefreshLoloRequestCreatedNotifiers notification received");
                    await queryClient.invalidateQueries({ queryKey: [loloRequestCreatedNotifiersQueryKey[0]] });
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
