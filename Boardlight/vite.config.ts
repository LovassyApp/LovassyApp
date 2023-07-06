import { defineConfig, loadEnv } from "vite";

import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    process.env = { ...process.env, ...loadEnv(mode, process.cwd()) };

    return {
        plugins: [react()],
        server: {
            proxy: {
                "/blueboard": {
                    target: process.env.VITE_BLUEBOARD_URL,
                    changeOrigin: true,
                    rewrite: (path) => path.replace(/^\/blueboard/, ""),
                },
            },
        },
    };
});
