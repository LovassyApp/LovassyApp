import { PersistOptions, createJSONStorage, persist } from "zustand/middleware";
import { StateCreator, create } from "zustand";

import { ColorScheme } from "@mantine/core";

export interface SettingsState {
    colorScheme: ColorScheme;
    toggleColorScheme(): void;
}

type SettingsStorePersist = (
    config: StateCreator<SettingsState>,
    options: PersistOptions<SettingsState>
) => StateCreator<SettingsState>;

export const useSettingsStore = create<SettingsState>(
    (persist as unknown as SettingsStorePersist)(
        (set, get) => ({
            colorScheme: "light",
            toggleColorScheme: () =>
                set((state) => ({ colorScheme: state.colorScheme === "light" ? "dark" : "light" })),
        }),
        {
            name: "settings-storage",
            storage: createJSONStorage(() => localStorage),
        }
    )
);
