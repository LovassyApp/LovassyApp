import { ColorScheme, ColorSchemeProvider, MantineProvider } from "@mantine/core";
import { useEffect, useState } from "react";

import { AppRouter } from "./routes/appRouter";
import { MemoryRouter } from "react-router-dom";
import { ModalsProvider } from "@mantine/modals";
import { Notifications } from "@mantine/notifications";
import ReactDOM from "react-dom/client";
import { UnlistenFn } from "@tauri-apps/api/event";
import { appWindow } from "@tauri-apps/api/window";
import { useLocalStorage } from "@mantine/hooks";

const App = () => {
    const [savedColorScheme, setSavedColorScheme] = useLocalStorage<ColorScheme | "system">({
        key: "mantine-color-scheme",
        defaultValue: "system",
        getInitialValueInEffect: false,
    });

    const [colorScheme, setColorScheme] = useState<ColorScheme>("light");

    const toggleColorScheme = () => {
        setSavedColorScheme(colorScheme === "dark" ? "light" : "dark");
    };

    useEffect(() => {
        let unlisten: UnlistenFn | undefined;

        (async () => {
            if (savedColorScheme === "system") {
                setColorScheme(await appWindow.theme() as ColorScheme);

                unlisten = await appWindow.onThemeChanged(({ payload: theme }) => {
                    setColorScheme(theme as ColorScheme);
                });
            } else {
                setColorScheme(savedColorScheme);
            }
        })();

        return () => {
            if (unlisten) {
                unlisten();
            }
        };
    }, [savedColorScheme]);

    return (
        <ColorSchemeProvider colorScheme={colorScheme} toggleColorScheme={toggleColorScheme}>
            <MantineProvider theme={{
                colorScheme,
            }}
            withGlobalStyles={true}
            withNormalizeCSS={true}>
                <ModalsProvider>
                    <Notifications limit={3} />
                    <AppRouter />
                </ModalsProvider>
            </MantineProvider>
        </ColorSchemeProvider>
    );
};

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
    <MemoryRouter>
        <App />
    </MemoryRouter>
);
