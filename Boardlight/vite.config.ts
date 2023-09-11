import { defineConfig, loadEnv } from "vite";

import { VitePWA } from "vite-plugin-pwa";
import packageJson from "./package.json";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    process.env = { ...process.env, ...loadEnv(mode, process.cwd()) };

    return {
        plugins: [
            react(),
            VitePWA({
                registerType: "autoUpdate",
                manifest: {
                    name: "LovassyApp - Boardlight",
                    short_name: "LovassyApp",
                    description: "A LovassyApp hivatalos webes frontendje.",
                    theme_color: "#1c7ed6",
                    icons: [
                        { src: "/android-chrome-192x192.png", sizes: "192x192", type: "image/png" },
                        { src: "/android-chrome-512x512.png", sizes: "512x512", type: "image/png" },
                    ],
                },
            }),
        ],
        define: {
            "import.meta.env.PACKAGE_VERSION": JSON.stringify(packageJson.version),
        },
    };
});
