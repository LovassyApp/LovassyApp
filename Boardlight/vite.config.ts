import { defineConfig, loadEnv } from "vite";

import packageJson from "./package.json";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    process.env = { ...process.env, ...loadEnv(mode, process.cwd()) };

    return {
        plugins: [react()],
        define: {
            "import.meta.env.PACKAGE_VERSION": JSON.stringify(packageJson.version),
        },
        server: {
            proxy: {
                "/blueboard": {
                    target: process.env.VITE_BLUEBOARD_URL,
                    changeOrigin: true,
                    rewrite: (path) => path.replace(/^\/blueboard/, ""),
                    ws: true,
                },
            },
        },
    };
});
