import axios, { AxiosRequestConfig } from "axios";

export const blueboardURL = import.meta.env.VITE_API_URL.VITE_BLUEBOARD_URL;

const axiosConfig: AxiosRequestConfig = {
    baseURL: blueboardURL,
};
// TODO: Add interceptors for auth

export const axiosClient = axios.create(axiosConfig);
