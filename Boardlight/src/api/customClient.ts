import { getPostApiAuthRefreshMutationOptions } from "./generated/features/auth/auth";
import { handleApiErrors } from "../helpers/apiHelpers";
import { useAuthStore } from "../core/stores/authStore";

const callAPI = async (url: string, init: RequestInit) => {
    const response = await fetch(url, init);

    if (!response.ok) {
        let res;
        try {
            res = { status: response.status, data: await response.json() };
        } catch (error) {
            res = { status: response.status, data: {} };
        }
        throw res;
    }

    try {
        if (response.headers.get("Content-Type")?.includes("application/json")) {
            return await response.json();
        }
        const a = document.createElement("a");
        const file = window.URL.createObjectURL(await response.blob());
        a.href = file;
        const header = response.headers.get("Content-Disposition");
        const parts = header.split(";");
        const filename = parts[1].split("=")[1];
        a.download = filename;
        a.click();
    } catch (error) {
        return;
    }

    return response;
};

export const useCustomClient = async <T>({
    url,
    method,
    params,
    headers,
    data,
    signal,
}: {
    url: string;
    method: "get" | "post" | "put" | "delete" | "patch";
    params?: any;
    headers?: any;
    data?: unknown;
    signal?: AbortSignal;
}): Promise<T> => {
    const baseUrl = "/blueboard";

    let headersLocal;
    if (data instanceof FormData) {
        headersLocal = { Accept: "application/json" };
    } else {
        headersLocal = { ...headers, "Content-Type": "application/json", Accept: "application/json" };
    }

    if (useAuthStore.getState().accessToken) {
        headersLocal.Authorization = `Bearer ${useAuthStore.getState().accessToken}`;
    }

    try {
        return await callAPI(`${baseUrl}${url}?${new URLSearchParams(params)}`, {
            method: method.toUpperCase(),
            headers: headersLocal,
            ...(data ? { body: data instanceof FormData ? data : JSON.stringify(data), signal } : { signal }),
        });
    } catch (error) {
        if (error.status === 401) {
            try {
                console.log("Attempting to refresh token...");

                // eslint-disable-next-line react-hooks/rules-of-hooks
                const refreshOptions = getPostApiAuthRefreshMutationOptions();
                const res = await refreshOptions.mutationFn({});

                useAuthStore.getState().setAccessToken(res.token);
                headersLocal.Authorization = `Bearer ${res.token}`;

                console.log("Successfully refreshed token");

                return await callAPI(`${baseUrl}${url}?${new URLSearchParams(params)}`, {
                    method: method.toUpperCase(),
                    headers: headersLocal,
                    ...(data ? { body: data instanceof FormData ? data : JSON.stringify(data), signal } : { signal }),
                });
            } catch (error) {
                console.log("Failed to refresh token");

                useAuthStore.getState().setAccessToken(undefined);
                document.dispatchEvent(new Event("resetQueryClient")); // You may ask why not just call queryClient.clear() here... Well, the thing is that even though that works perfectly, I can't for the love of god get it to generate the client if we do that. So we have this weird ass solution instead, and we listen to this event in main.tsx.
            }
        }
        handleApiErrors(error);
        throw error;
    }
};

export default useCustomClient;

export type ErrorType<ErrorData> = ErrorData;

export type BodyType<BodyData> = BodyData & { headers?: any };
