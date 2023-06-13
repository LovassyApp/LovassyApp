import { StateStorage } from "zustand/middleware";
import { Store } from "tauri-plugin-store-api";

export const preferencesStore = new Store(".preferences.dat");

export const preferencesStorage: StateStorage = {
    getItem: async (name: string): Promise<string | null> => {
        return (await preferencesStore.get(name)) || null;
    },
    setItem: async (name: string, value: string): Promise<void> => {
        await preferencesStore.set(name, value);
    },
    removeItem: async (name: string): Promise<void> => {
        await preferencesStore.delete(name);
    },
};
