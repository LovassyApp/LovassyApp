import { ActionIcon, ColorScheme } from "@mantine/core";
import { IconDeviceDesktop, IconMoonStars, IconSun } from "@tabler/icons-react";

import { useLocalStorage } from "@mantine/hooks";

export const ColorSchemeToggle = (): JSX.Element => {
    const [savedColorScheme, setSavedColorScheme] = useLocalStorage<ColorScheme | "system">({
        key: "mantine-color-scheme",
        defaultValue: "system",
        getInitialValueInEffect: false,
    });

    return (<ActionIcon
        variant="light"
        onClick={() => setSavedColorScheme(savedColorScheme === "dark" ? "light" : (savedColorScheme === "light" ? "system" : "dark"))}
        sx={(theme) => ({
            backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[0],
            color: theme.colorScheme === "dark" ? theme.white : theme.black,
        })}>
        {savedColorScheme === "dark" ? <IconMoonStars stroke={1.5} size={18} /> : (savedColorScheme === "light" ? <IconSun stroke={1.5} size={18} /> : <IconDeviceDesktop stroke={1.5} size={18} />)}
    </ActionIcon>);
};
