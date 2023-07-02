import { Body, getClient } from "@tauri-apps/api/http";
import { Box, Button, Center, Divider, Group, Loader, Stack, Text, Title, createStyles } from "@mantine/core";
import { useEffect, useState } from "react";

import { invoke } from "@tauri-apps/api/tauri";
import { useSettingStore } from "../../stores/settingsStore";

const useStyles = createStyles((theme) => ({
    container: {
        height: "100%",
    }
}));

const StatusPage = (): JSX.Element => {
    const { classes } = useStyles();

    const blueboardUrl = useSettingStore((state) => state.blueboardUrl);
    const [loading, setLoading] = useState<boolean>(true);
    const [data, setData] = useState<any>(null);
    const [error, setError] = useState<string | null>(null);

    const fetchData = async (url: string) => {
        // TODO: use a generated client!!!
        try {
            const response = await invoke("status", { blueboardUrl }) as any;
            setError(null);
            setData(response);
        } catch (error) {
            setError("Nem sikerült lekérni az adatokat");
        }
    };

    useEffect(() => {
        setLoading(true);
        (async () => {
            if (blueboardUrl === "") {
                setError("Nincs beállítva Blueboard URL!");
                setLoading(false);
                return;
            }
            await fetchData(`${blueboardUrl}/Api/Status/ServiceStatus`);

            setLoading(false);
        })();
    }, [blueboardUrl]);

    return (
        <Stack spacing="xs" className={classes.container}>
            <Title order={2} size="h1">Állapot</Title>
            {loading && (
                <Center sx={{ height: "100%" }}>
                    <Loader />
                </Center>
            )}
            {!loading && error !== null && (
                <Center sx={{ height: "100%" }}>
                    <Text color="red" size="sm">{error}</Text>
                </Center>
            )}
            {!loading && error === null && data !== null && (
                <>
                    <Group position="apart">
                        <Text size="sm">Blueboard készen áll:</Text>
                        <Text size="sm"
                            weight="bold"
                            color={data.ready ? "green" : "red"}>
                            {data.ready ? "Igen" : "Nem"}
                        </Text>
                    </Group>
                    <Divider />
                    <Group position="apart">
                        <Text size="sm">Adatbázis:</Text>
                        <Text size="sm"
                            weight="bold"
                            color={data.serviceStatus.database ? "green" : "red"}>
                            {data.serviceStatus.database ? "Fut" : "Nem fut"}
                        </Text>
                    </Group>
                    <Group position="apart">
                        <Text size="sm">Valósidejű funkciók:</Text>
                        <Text size="sm"
                            weight="bold"
                            color={data.serviceStatus.realtime ? "green" : "red"}>
                            {data.serviceStatus.realtime ? "Fut" : "Nem fut"}
                        </Text>
                    </Group>
                    <Group position="apart">
                        <Text size="sm">Visszaállítási jelszó:</Text>
                        <Text size="sm"
                            weight="bold"
                            color={data.serviceStatus.resetKeyPassword ? "green" : "yellow"}>
                            {data.serviceStatus.resetKeyPassword ? "Beállítva" : "Nincs beállítva"}
                        </Text>
                    </Group>
                    <Button variant="default" onClick={async () => {
                        setLoading(true);
                        if (blueboardUrl === "") {
                            setError("Nincs beállítva Blueboard URL!");
                            setLoading(false);
                            return;
                        }
                        await fetchData(`${blueboardUrl}/Api/Status/ServiceStatus`);
                        setLoading(false);
                    }} sx={{ alignSelf: "center" }}>
                        Frissítés
                    </Button>
                </>
            )}

        </Stack>
    );
};

export default StatusPage;
