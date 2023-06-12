import { Button, Stack, TextInput, Title } from "@mantine/core";

import { IconDeviceFloppy } from "@tabler/icons-react";

const SettingsPage = (): JSX.Element => {
    return (
        <Stack spacing="xs">
            <Title order={2} size="h1">Beállítások</Title>
            <TextInput label="Blueboard URL" description="A LovassyApp backend URL-je" withAsterisk={true} placeholder="https://blueboard.lovassy.hu" />
            <TextInput label="API kulcs" description="A hozzáfést biztosító API kulcs, LovassyApp fejlesztőktől kérhető" withAsterisk={true} />
            <Button leftIcon={<IconDeviceFloppy stroke={1.5} />} sx={{ alignSelf: "center" }} variant="default" mt="xs">Mentés</Button>
        </Stack>
    );
};

export default SettingsPage;
