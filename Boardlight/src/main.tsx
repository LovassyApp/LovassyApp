import { ColorSchemeProvider, MantineProvider } from "@mantine/core";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { RouterProvider, createBrowserRouter, createRoutesFromElements } from "react-router-dom";
import { Suspense, useEffect } from "react";

import { BlueboardStatusChecker } from "./core/components/blueboardStatusChecker";
import { FullScreenLoading } from "./core/components/fullScreenLoading";
import { ModalsProvider } from "@mantine/modals";
import { Notifications } from "@mantine/notifications";
import ReactDOM from "react-dom/client";
import { RealtimeNotificationsBootstrapper } from "./core/realtime/components/realtimeNotificationsBootstrapper";
import { getAppRoutes } from "./core/routing/getAppRoutes";
import { useHotkeys } from "@mantine/hooks";
import { useSettingsStore } from "./core/stores/settingsStore";

export const router = createBrowserRouter(createRoutesFromElements(getAppRoutes()));

export const queryClient = new QueryClient();

const App = () => {
    const settings = useSettingsStore();

    useHotkeys([["mod+J", () => settings.toggleColorScheme()]]);

    useEffect(() => {
        document.addEventListener("resetQueryClient", () => queryClient.clear());

        return () => document.removeEventListener("resetQueryClient", () => queryClient.clear());
    }, []);

    return (
        <ColorSchemeProvider colorScheme={settings.colorScheme} toggleColorScheme={settings.toggleColorScheme}>
            <MantineProvider
                theme={{ colorScheme: settings.colorScheme }}
                withNormalizeCSS={true}
                withGlobalStyles={true}
            >
                <ModalsProvider>
                    <QueryClientProvider client={queryClient}>
                        <Notifications limit={3} />
                        <BlueboardStatusChecker>
                            <RealtimeNotificationsBootstrapper>
                                <Suspense fallback={<FullScreenLoading />}>
                                    <RouterProvider router={router} fallbackElement={<FullScreenLoading />} />
                                </Suspense>
                            </RealtimeNotificationsBootstrapper>
                        </BlueboardStatusChecker>
                    </QueryClientProvider>
                </ModalsProvider>
            </MantineProvider>
        </ColorSchemeProvider>
    );
};

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(<App />);
