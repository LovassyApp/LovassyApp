import axios, { AxiosRequestConfig } from "axios";

import { IconX } from "@tabler/icons-react";
import { authApiClient } from "./apiClients";
import { notifications } from "@mantine/notifications";
import { useAuthStore } from "../core/stores/authStore";

export const blueboardURL = import.meta.env.VITE_BLUEBOARD_URL;

const axiosConfig: AxiosRequestConfig = {
    baseURL: blueboardURL,
};
// TODO: Add interceptors for auth

export const axiosClient = axios.create(axiosConfig);

axiosClient.interceptors.response.use(
    (response) => response,
    async (error) => {
        if (error.response.status === 401) {
            console.log("Attempting to refresh token...");

            try {
                const res = await authApiClient.apiAuthRefreshPost();
                useAuthStore.getState().setAccessToken(res.data.token);
            } catch (err) {
                console.log("Error refreshing token: ", err);
                useAuthStore.getState().setAccessToken(undefined); // Weird circular dependency thingy but it works
                notifications.show({
                    title: "Lejárt a munkamenet",
                    message: "Úgy néz ki, hogy lejárt a munkamenet. Kérlek, jelentkezz be újra!",
                    color: "red",
                    icon: <IconX />,
                    autoClose: 5000,
                    withCloseButton: true,
                });
                throw error;
            }
        }
        throw error;
    }
);
