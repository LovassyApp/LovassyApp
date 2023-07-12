import {
    Group,
    Paper,
    RingProgress,
    SimpleGrid,
    Stack,
    Text,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { ShopIndexLolosResponse, ShopIndexOwnLolosResponse } from "../../../api/generated/models";
import { useEffect, useMemo, useState } from "react";

const useStyles = createStyles((theme) => ({
    statsCard: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
    },
}));

export const CoinsStats = ({ data }: { data: ShopIndexLolosResponse[] | ShopIndexOwnLolosResponse }): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const coins = useMemo(() => {
        if ("coins" in data) {
            return data.coins as ShopIndexLolosResponse[];
        }
        return data as ShopIndexLolosResponse[];
    }, [data]);

    const balance = useMemo(() => {
        if ("coins" in data) {
            return data.balance;
        }
        // @ts-ignore
        return data.filter((c) => !c.isSpent).length;
    }, [data]);

    const fromFive = useMemo(
        () =>
            coins?.filter((c) => c.loloType === "FromGrades" && c.reason === "Ötösökből automatikusan generálva")
                .length,
        [coins]
    );

    const fromFour = useMemo(
        () =>
            coins?.filter((c) => c.loloType === "FromGrades" && c.reason === "Négyesekből automatikusan generálva")
                .length,
        [coins]
    );

    const fromRequests = useMemo(() => coins.filter((c) => c.loloType === "FromRequest").length, [coins]);

    return (
        <SimpleGrid cols={2} breakpoints={[{ maxWidth: theme.breakpoints.sm, cols: 1, spacing: "sm" }]}>
            <Paper radius="md" p="md" className={classes.statsCard}>
                <Group position="apart">
                    <Stack spacing={0}>
                        <Title order={2} mb="sm">
                            Költekezés
                        </Title>
                        <Text>
                            Elérhető:{" "}
                            <Text component="span" weight="bold" color="cyan.5">
                                {balance} db
                            </Text>
                        </Text>
                        <Text>
                            Elköltött:{" "}
                            <Text component="span" weight="bold" color="blue.7">
                                {coins.length - balance} db
                            </Text>
                        </Text>
                        <Text>
                            Összesen:{" "}
                            <Text component="span" weight="bold">
                                {coins.length} db
                            </Text>
                        </Text>
                    </Stack>
                    <RingProgress
                        sections={[
                            {
                                value: (balance / coins.length) * 100,
                                color: "cyan.5",
                                tooltip: `Elérhető - ${balance} db`,
                            },
                            {
                                value: ((coins.length - balance) / coins.length) * 100,
                                color: "blue.7",
                                tooltip: `Elköltött - ${coins.length - balance} db`,
                            },
                        ]}
                    />
                </Group>
            </Paper>
            <Paper radius="md" p="md" className={classes.statsCard}>
                <Group position="apart">
                    <Stack spacing={0}>
                        <Title order={2} mb="sm">
                            Források
                        </Title>
                        <Text>
                            Ötösökből:{" "}
                            <Text component="span" weight="bold" color="pink">
                                {fromFive} db
                            </Text>
                        </Text>
                        <Text>
                            Négyesekből:{" "}
                            <Text component="span" weight="bold" color="grape">
                                {fromFour} db
                            </Text>
                        </Text>
                        <Text>
                            Kérvényekből:{" "}
                            <Text component="span" weight="bold" color="violet">
                                {fromRequests} db
                            </Text>
                        </Text>
                    </Stack>
                    <RingProgress
                        sections={[
                            {
                                value: (fromFive / coins.length) * 100,
                                color: "pink",
                                tooltip: `Ötösökből - ${fromFive} db`,
                            },
                            {
                                value: (fromFour / coins.length) * 100,
                                color: "grape",
                                tooltip: `Négyesekből - ${fromFour} db`,
                            },
                            {
                                value: (fromRequests / coins.length) * 100,
                                color: "violet",
                                tooltip: `Kérvényekből - ${fromRequests} db`,
                            },
                        ]}
                    />
                </Group>
            </Paper>
        </SimpleGrid>
    );
};
