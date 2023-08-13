import { Center, Divider, Group, Loader, Stack, Switch, Text, TextInput, Title } from "@mantine/core";
import { disable, enable, isEnabled } from "tauri-plugin-autostart-api";
import { useEffect, useState } from "react";

import { preferencesStore } from "../../preferencesStore";
import { useSettingStore } from "../../stores/settingsStore";

const SettingsPage = (): JSX.Element => {
    const settings = useSettingStore();

    const [loading, setLoading] = useState<boolean>(false);
    const [autostart, setAutostart] = useState<boolean>();

    useEffect(() => {
        (async () => {
            setLoading(true);
            setAutostart(await isEnabled());
            setLoading(false);
        })();
    }, []);

    const saveSettings = async () => {
        await preferencesStore.save();
    };

    return (
        <Stack spacing="xs">
            <Title order={2} size="h1">
                Beállítások
            </Title>
            {loading && (
                <Center sx={{ height: "100%" }}>
                    <Loader />
                </Center>
            )}
            {!loading && (
                <>
                    <TextInput
                        label="Blueboard URL"
                        description="A LovassyApp backend URL-je"
                        value={settings.blueboardUrl}
                        onChange={(event) => settings.setBlueboardUrl(event.currentTarget.value)}
                        onBlur={async () => await saveSettings()}
                        withAsterisk={true}
                        placeholder="https://blueboard.lovassy.hu"
                    />
                    <TextInput
                        label="Import kulcs"
                        description="A hozzáfést biztosító import kulcs, LovassyApp fejlesztőktől kérhető"
                        value={settings.importKey}
                        onChange={(event) => settings.setImportKey(event.currentTarget.value)}
                        onBlur={async () => await saveSettings()}
                        withAsterisk={true}
                    />
                    <Divider variant="dashed" />
                    <Group position="apart">
                        <Text size="sm">Automatikus indítás</Text>
                        <Switch
                            checked={autostart}
                            onChange={async (event) => {
                                event.persist(); // To prevent React reclaiming the synthetic event
                                event.currentTarget.checked ? enable() : disable();
                                setAutostart(event.currentTarget.checked);
                            }}
                        />
                    </Group>
                </>
            )}
        </Stack>
    );
};

export default SettingsPage;
