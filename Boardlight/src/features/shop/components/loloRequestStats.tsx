import { Box, Group, Paper, RingProgress, Stack, Text, Title, createStyles } from "@mantine/core";

import { ShopIndexLoloRequestsResponse } from "../../../api/generated/models";
import { useMemo } from "react";

const useStyles = createStyles((theme) => ({
    statsCard: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
    },
}));

export const LoloRequestStats = ({ data }: { data: ShopIndexLoloRequestsResponse[] }): JSX.Element[] => {
    const { classes } = useStyles();

    const accepted = useMemo(() => data.filter((c) => c.acceptedAt).length, [data]);

    const denied = useMemo(() => data.filter((c) => c.deniedAt).length, [data]);

    const pending = useMemo(() => data.filter((c) => !c.acceptedAt && !c.deniedAt).length, [data]);

    return [
        <Paper radius="md" p="md" className={classes.statsCard} key="1">
            <Group position="apart">
                <Stack spacing={0}>
                    <Title order={2} mb="sm">
                        Állapot
                    </Title>
                    <Text>
                        Elfogadott:{" "}
                        <Text component="span" weight="bold" color="green">
                            {accepted} db
                        </Text>
                    </Text>
                    <Text>
                        Elutasított:{" "}
                        <Text component="span" weight="bold" color="red">
                            {denied} db
                        </Text>
                    </Text>
                    <Text>
                        Függőben:{" "}
                        <Text component="span" weight="bold" color="yellow">
                            {pending} db
                        </Text>
                    </Text>
                    <Text>
                        Összesen:{" "}
                        <Text component="span" weight="bold">
                            {data.length} db
                        </Text>
                    </Text>
                </Stack>
                <RingProgress
                    sections={[
                        {
                            value: (accepted / data.length) * 100,
                            color: "green",
                            tooltip: `Elfogadott - ${accepted} db`,
                        },
                        {
                            value: (denied / data.length) * 100,
                            color: "red",
                            tooltip: `Elutasított - ${denied} db`,
                        },
                        {
                            value: (pending / data.length) * 100,
                            color: "yellow",
                            tooltip: `Függőben - ${pending} db`,
                        },
                    ]}
                />
            </Group>
        </Paper>,
    ];
};
