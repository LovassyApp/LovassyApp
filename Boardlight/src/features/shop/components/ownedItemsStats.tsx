import { Group, Paper, RingProgress, Stack, Text, Title, createStyles } from "@mantine/core";

import { ShopIndexOwnedItemsResponse } from "../../../api/generated/models";
import { useMemo } from "react";

const useStyles = createStyles((theme) => ({
    statsCard: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
    },
}));

export const OwnedItemsStats = ({ data }: { data: ShopIndexOwnedItemsResponse[] }): JSX.Element[] => {
    const { classes } = useStyles();

    const used = useMemo(() => data.filter((i) => i.usedAt).length, [data]);

    return [
        <Paper radius="md" p="md" className={classes.statsCard} key="1">
            <Group position="apart">
                <Stack spacing={0}>
                    <Title order={2} mb="sm">
                        Állapot
                    </Title>
                    <Text>
                        Nem felhasznált:{" "}
                        <Text component="span" weight="bold" color="green">
                            {data.length - used} db
                        </Text>
                    </Text>
                    <Text>
                        Felhasznált:{" "}
                        <Text component="span" weight="bold" color="red">
                            {used} db
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
                            value: ((data.length - used) / data.length) * 100,
                            color: "green",
                            tooltip: `Nem felhasznált - ${data.length - used} db`,
                        },
                        {
                            value: (used / data.length) * 100,
                            color: "red",
                            tooltip: `Felhasznált - ${used} db`,
                        },
                    ]}
                />
            </Group>
        </Paper>,
    ];
};
