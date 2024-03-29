import "dayjs/locale/hu";

import { ColorSchemeProvider, MantineProvider } from "@mantine/core";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { RouterProvider, createBrowserRouter, createRoutesFromElements } from "react-router-dom";
import { Suspense, useEffect } from "react";

import { BlueboardStatusChecker } from "./core/components/blueboardStatusChecker";
import { DatesProvider } from "@mantine/dates";
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
        // "Boardlight" figlet
        console.log(
            " ____                      _ _ _       _     _   \r\n| __ )  ___   __ _ _ __ __| | (_) __ _| |__ | |_ \r\n|  _ \\ / _ \\ / _` | '__/ _` | | |/ _` | '_ \\| __|\r\n| |_) | (_) | (_| | | | (_| | | | (_| | | | | |_ \r\n|____/ \\___/ \\__,_|_|  \\__,_|_|_|\\__, |_| |_|\\__|\r\n                                 |___/           "
        );

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
                <DatesProvider settings={{ locale: "hu" }}>
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
                </DatesProvider>
            </MantineProvider>
        </ColorSchemeProvider>
    );
};

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(<App />);
