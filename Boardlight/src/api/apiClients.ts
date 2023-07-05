import {
    AuthApi,
    GradesApi,
    ImportApi,
    ImportKeysApi,
    LoloRequestsApi,
    LolosApi,
    OwnedItemsApi,
    PermissionsApi,
    ProductsApi,
    QRCodesApi,
    StatusApi,
    UserGroupsApi,
    UsersApi,
} from "./generated";
import { axiosClient, blueboardURL } from "./axiosClient";

export const authApiClient = new AuthApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const gradesApiClient = new GradesApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const importApiClient = new ImportApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const importKeysApiClient = new ImportKeysApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const loloRequestsApiClient = new LoloRequestsApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const lolosApiClient = new LolosApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const ownedItemsApiClient = new OwnedItemsApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const permissionsApiClient = new PermissionsApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const productsApiClient = new ProductsApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const qrCodesApiClient = new QRCodesApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const statusApiClient = new StatusApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const userGroupsApiClient = new UserGroupsApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);

export const usersApiClient = new UsersApi(
    {
        basePath: blueboardURL,
        isJsonMime: () => false,
    },
    undefined,
    axiosClient
);
