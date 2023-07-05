import { createJSONStorage, persist } from "zustand/middleware";

import { create } from "zustand";
import { preferencesStorage } from "../preferencesStore";

interface SecurityState {
    resetKeyPassword: string;
    updateResetKeyPasswordOnImport: boolean;
    setResetKeyPassword(url: string): void;
    setUpdateResetKeyPasswordOnImport(value: boolean): void;
}

export const useSecurityStore = create<SecurityState>()(
    persist(
        (set) => ({
            resetKeyPassword: "",
            updateResetKeyPasswordOnImport: true,
            setResetKeyPassword: (url: string) => set({ resetKeyPassword: url }),
            setUpdateResetKeyPasswordOnImport: (value: boolean) => set({ updateResetKeyPasswordOnImport: value })
        }), {
            name: "security",
            storage: createJSONStorage(() => preferencesStorage)
        }
    )
);
