import { IconX } from "@tabler/icons-react";
import { UseFormReturnType } from "@mantine/form";
import { notifications } from "@mantine/notifications";

export class ValidationError {
    public constructor(public readonly message: string, public readonly errors: { [key: string]: string[] }) {}
}

export class NotFoundError {
    public constructor(public readonly message: string) {}
}

export const handleApiErrors = (error: any) => {
    switch (error.status) {
        case 400:
            throw new ValidationError(error.data.detail ?? "Hibás kérvény.", error.data.errors ?? []);
        case 401:
            notifications.show({
                title: "Hiba (401)",
                color: "red",
                icon: <IconX />,
                message: "Lejárt a munkameneted. Kérlek lépj be újra.",
            });
            break;
        case 403:
            notifications.show({
                title: "Hiba (403)",
                color: "red",
                icon: <IconX />,
                message: "Nincs jogosultságod a kérés végrehajtásához.",
            });
            break;
        case 404:
            throw new NotFoundError(error.message);
        case 429:
            notifications.show({
                title: "Hiba (429)",
                color: "red",
                icon: <IconX />,
                message: "Túl sok kérés. Kérlek próbáld újra később.",
            });
            break;
        case 500:
            notifications.show({
                title: "Hiba (500)",
                color: "red",
                icon: <IconX />,
                message: "Szerverhiba. Kérlek próbáld újra később.",
            });
            break;
        case 503:
            notifications.show({
                title: "Hiba (503)",
                color: "red",
                icon: <IconX />,
                message: "Funkció nem elérhető. Kérlek próbáld újra később.",
            });
            break;
        default:
            if (error instanceof DOMException) break;
            console.error(error);
            notifications.show({
                title: "Hiba",
                color: "red",
                icon: <IconX />,
                message: "Egy ismeretlen hiba történt.",
            });
            break;
    }
};

export const handleValidationErrors = (error: ValidationError, form: UseFormReturnType<any>) => {
    if (Object.keys(error.errors).length === 0) {
        notifications.show({
            title: "Hiba (400)",
            color: "red",
            icon: <IconX />,
            message: error.message,
        });
        return;
    }

    for (const validationError in error.errors) {
        let message = "";
        for (const err of error.errors[validationError]) {
            message += `${err}\n`;
        }
        form.setFieldError(validationError, message);
    }
};
