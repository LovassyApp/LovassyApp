import { Button, Checkbox, Stack, Text, Title } from "@mantine/core";
import { notifications, showNotification } from "@mantine/notifications";
import { useEffect, useState } from "react";

import { IconCheck } from "@tabler/icons-react";
import { ResetKeyPasswordInput } from "../../components/resetKeyPasswordInput";
import { invoke } from "@tauri-apps/api";
import { preferencesStore } from "../../preferencesStore";
import { useSecurityStore } from "../../stores/securityStore";
import { useSettingStore } from "../../stores/settingsStore";

const SecurityPage = (): JSX.Element => {
    const security = useSecurityStore();
    const settings = useSettingStore();

    const [uploadLoading, setUploadLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (settings.blueboardUrl === "" || settings.importKey === "") {
            setError("Nincs beállítva Blueboard URL vagy import kulcs!");
        } else {
            setError(null);
        }
    }, [settings.blueboardUrl, settings.importKey]);

    const updateResetKeyPassword = async () => {
        setUploadLoading(true);
        try {
            await invoke("upload_reset_key_password", {
                blueboardUrl: settings.blueboardUrl,
                resetKeyPassword: security.resetKeyPassword,
                importKey: settings.importKey,
            });
            notifications.show({
                id: "reset-key-password-uploaded",
                withCloseButton: true,
                autoClose: 3000,
                title: "Sikeres feltöltés",
                message: "A visszaállítási jelszó feltöltése sikeresen megtörtént!",
                icon: <IconCheck />,
                color: "green",
            });
        } catch (error) {
            if (error === "401") setError("Hibás import kulcs!");
            else setError("Nem sikerült feltölteni a visszaállítási jelszót!");
        }
        setUploadLoading(false);
    };

    return (
        <Stack spacing="xs">
            <Title order={2} size="h1">
                Biztonság
            </Title>
            <ResetKeyPasswordInput />
            <Checkbox
                label="Visszaállítási jelszó automatikus feltöltése importáláskor"
                checked={security.updateResetKeyPasswordOnImport}
                onChange={async (event) => {
                    security.setUpdateResetKeyPasswordOnImport(event.currentTarget.checked);
                    await preferencesStore.save();
                }}
            />
            <Button
                loading={uploadLoading}
                disabled={settings.blueboardUrl === "" || security.resetKeyPassword === "" || settings.importKey === ""}
                variant="default"
                sx={{ alignSelf: "center" }}
                mt="xs"
                onClick={async () => await updateResetKeyPassword()}
            >
                Feltöltés most
            </Button>
            {error && (
                <Text color="red" size="sm" sx={{ alignSelf: "center" }}>
                    {error}
                </Text>
            )}
        </Stack>
    );
};

export default SecurityPage;
