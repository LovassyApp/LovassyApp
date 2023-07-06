import { PersistOptions, createJSONStorage, persist } from "zustand/middleware";
import { StateCreator, create } from "zustand";

export interface AuthState {
    accessToken?: string;
    setAccessToken(accessToken?: string): void;
}

type SettingsStorePersist = (
    config: StateCreator<AuthState>,
    options: PersistOptions<AuthState>
) => StateCreator<AuthState>;

export const useAuthStore = create<AuthState>(
    (persist as unknown as SettingsStorePersist)(
        (set, get) => ({
            accessToken: undefined,
            setAccessToken: (accessToken?: string) => set({ accessToken }),
        }),
        {
            name: "auth-storage",
            storage: createJSONStorage(() => sessionStorage),
        }
    )
);
