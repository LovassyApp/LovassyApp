import { createJSONStorage, persist } from "zustand/middleware";

import { create } from "zustand";
import { preferencesStorage } from "../preferencesStore";

interface SettingsState {
    blueboardUrl: string;
    setBlueboardUrl(url: string): void;
    importKey: string;
    setImportKey(key: string): void;
}

export const useSettingStore = create<SettingsState>()(
    persist(
        (set) => ({
            blueboardUrl: "",
            setBlueboardUrl: (url: string) => set({ blueboardUrl: url }),
            importKey: "",
            setImportKey: (key: string) => set({ importKey: key }),
        }), {
            name: "settings",
            storage: createJSONStorage(() => preferencesStorage)
        }
    )
);
