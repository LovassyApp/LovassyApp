import {
    Box,
    Center,
    Group,
    Loader,
    Paper,
    RingProgress,
    SimpleGrid,
    Stack,
    Text,
    Title,
    createStyles,
    rem,
    useMantineTheme,
} from "@mantine/core";

import { IconCoin } from "@tabler/icons-react";
import { ShopIndexOwnLolosResponseCoin } from "../../../api/generated/models";
import { useGetApiLolosOwn } from "../../../api/generated/features/lolos/lolos";
import { useMemo } from "react";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
    statsCard: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
    },
}));

const CoinCard = ({ coin }: { coin: ShopIndexOwnLolosResponseCoin }): JSX.Element => {
    const theme = useMantineTheme();

    return (
        <Paper withBorder={true} radius="md" p="xs">
            <Group position="apart" maw="100%" sx={{ flexWrap: "nowrap" }}>
                <Stack justify="space-between" align="stretch" spacing={0} sx={{ flex: 1, overflow: "hidden" }}>
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {coin.reason}
                        </Text>
                    </Box>
                    <Text size="sm" color="dimmed">
                        {new Date(coin.createdAt).toLocaleDateString("hu-HU", {})}
                    </Text>
                </Stack>
                <IconCoin
                    stroke={1.5}
                    size={rem(48)}
                    color={
                        coin.isSpent
                            ? theme.colorScheme === "dark"
                                ? theme.colors.gray[7]
                                : theme.colors.gray[5]
                            : theme.fn.primaryColor()
                    }
                />
            </Group>
        </Paper>
    );
};

const CoinsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const coins = useGetApiLolosOwn({ Sorts: "isSpent" });

    const fromFive = useMemo(
        () =>
            coins.data?.coins.filter(
                (c) => c.loloType === "FromGrades" && c.reason === "Ötösökből automatikusan generálva"
            ).length,
        [coins.data]
    );

    const fromFour = useMemo(
        () =>
            coins.data?.coins.filter(
                (c) => c.loloType === "FromGrades" && c.reason === "Négyesekből automatikusan generálva"
            ).length,
        [coins.data]
    );

    const fromRequests = useMemo(
        () => coins.data?.coins.filter((c) => c.loloType === "FromRequest").length,
        [coins.data]
    );

    if (coins.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (coins.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <Title mb="md">Statisztikák</Title>
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
                                    {coins.data.balance} db
                                </Text>
                            </Text>
                            <Text>
                                Elköltött:{" "}
                                <Text component="span" weight="bold" color="blue.7">
                                    {coins.data.coins.length - coins.data.balance} db
                                </Text>
                            </Text>
                            <Text>
                                Összesen:{" "}
                                <Text component="span" weight="bold">
                                    {coins.data.coins.length} db
                                </Text>
                            </Text>
                        </Stack>
                        <RingProgress
                            sections={[
                                {
                                    value: (coins.data.balance / coins.data.coins.length) * 100,
                                    color: "cyan.5",
                                    tooltip: `Elérhető - ${coins.data.balance} db`,
                                },
                                {
                                    value:
                                        ((coins.data.coins.length - coins.data.balance) / coins.data.coins.length) *
                                        100,
                                    color: "blue.7",
                                    tooltip: `Elköltött - ${coins.data.coins.length - coins.data.balance} db`,
                                },
                            ]}
                        />
                    </Group>
                </Paper>
                <Paper radius="md" p="md" className={classes.statsCard}>
                    <Group position="apart">
                        <Stack spacing={0}>
                            <Title order={2} mb="sm">
                                Forrás
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
                                    value: (fromFive / coins.data.coins.length) * 100,
                                    color: "pink",
                                    tooltip: `Ötösökből - ${fromFive} db`,
                                },
                                {
                                    value: (fromFour / coins.data.coins.length) * 100,
                                    color: "grape",
                                    tooltip: `Négyesekből - ${fromFour} db`,
                                },
                                {
                                    value: (fromRequests / coins.data.coins.length) * 100,
                                    color: "violet",
                                    tooltip: `Kérvényekből - ${fromRequests} db`,
                                },
                            ]}
                        />
                    </Group>
                </Paper>
            </SimpleGrid>
            <Title my="md">Érmék</Title>
            <SimpleGrid
                cols={4}
                breakpoints={[
                    { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                    { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                    { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                ]}
            >
                {coins.data?.coins.map((coin) => (
                    <CoinCard key={coin.id} coin={coin} />
                ))}
            </SimpleGrid>
        </>
    );
};

export default CoinsPage;
