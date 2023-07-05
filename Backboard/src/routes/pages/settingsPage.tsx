import { Stack, TextInput, Title } from "@mantine/core";

import { preferencesStore } from "../../preferencesStore";
import { useSettingStore } from "../../stores/settingsStore";

const SettingsPage = (): JSX.Element => {
    const settings = useSettingStore();

    const saveSettings = async () => {
        await preferencesStore.save();
    };

    return (
        <Stack spacing="xs">
            <Title order={2} size="h1">Beállítások</Title>
            <TextInput label="Blueboard URL"
                description="A LovassyApp backend URL-je" value={settings.blueboardUrl}
                onChange={(event) => settings.setBlueboardUrl(event.currentTarget.value)}
                onBlur={async () => await saveSettings()} withAsterisk={true}
                placeholder="https://blueboard.lovassy.hu" />
            <TextInput label="Import kulcs"
                description="A hozzáfést biztosító import kulcs, LovassyApp fejlesztőktől kérhető"
                value={settings.importKey} onChange={(event) => settings.setImportKey(event.currentTarget.value)}
                onBlur={async () => await saveSettings()}
                withAsterisk={true} />
        </Stack>
    );
};

export default SettingsPage;
