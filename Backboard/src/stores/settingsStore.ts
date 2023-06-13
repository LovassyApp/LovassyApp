import { createJSONStorage, persist } from "zustand/middleware";

import { create } from "zustand";
import { preferencesStorage } from "../preferencesStore";

interface SettingsState {
    blueboardUrl: string;
    setBlueboardUrl(url: string): void;
    apiKey: string;
    setApiKey(key: string): void;
}

export const useSettingStore = create<SettingsState>()(
    persist(
        (set) => ({
            blueboardUrl: "",
            setBlueboardUrl: (url: string) => set({ blueboardUrl: url }),
            apiKey: "",
            setApiKey: (key: string) => set({ apiKey: key }),
        }), {
            name: "settings",
            storage: createJSONStorage(() => preferencesStorage)
        }
    )
);
