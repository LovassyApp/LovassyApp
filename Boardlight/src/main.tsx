import { ColorSchemeProvider, MantineProvider } from "@mantine/core";

import { AppRouter } from "./core/routing/appRouter";
import { BrowserRouter } from "react-router-dom";
import { ModalsProvider } from "@mantine/modals";
import { Notifications } from "@mantine/notifications";
import ReactDOM from "react-dom/client";
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
                    <AppRouter />
                </ModalsProvider>
            </MantineProvider>
        </ColorSchemeProvider>
    );
};

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
    <BrowserRouter>
        <App />
    </BrowserRouter>,
);
