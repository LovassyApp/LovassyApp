import { createJSONStorage, persist } from "zustand/middleware";

import { create } from "zustand";
import { preferencesStorage } from "../preferencesStore";

interface SecurityState {
    resetKeyPassword: string;
    setResetKeyPassword(url: string): void;
}

export const useSecurityStore = create<SecurityState>()(
    persist(
        (set) => ({
            resetKeyPassword: "",
            setResetKeyPassword: (url: string) => set({ resetKeyPassword: url }),
        }), {
            name: "security",
            storage: createJSONStorage(() => preferencesStorage)
        }
    )
);
