import { ColorSchemeProvider, MantineProvider } from "@mantine/core";

import { ModalsProvider } from "@mantine/modals";
import { Notifications } from "@mantine/notifications";
import ReactDOM from "react-dom/client";
import { RouterProvider } from "@tanstack/router";
import { appRouter } from "./core/routing/appRouter";
import { useHotkeys } from "@mantine/hooks";
import { useSettingsStore } from "./core/stores/settingsStore";

const App = () => {
    const settings = useSettingsStore();

    useHotkeys([["mod+J", () => settings.toggleColorScheme()]]);

    return (
        <ColorSchemeProvider colorScheme={settings.colorScheme} toggleColorScheme={settings.toggleColorScheme}>
            <MantineProvider theme={{ colorScheme: settings.colorScheme }} withNormalizeCSS={true} withGlobalStyles={true}>
                <ModalsProvider>
                    <Notifications />
                    <RouterProvider router={appRouter} />
                </ModalsProvider>
            </MantineProvider>
        </ColorSchemeProvider>
    );
};

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
    <App />
);
