import { Center, Loader, SimpleGrid, Text, Title, createStyles, useMantineTheme } from "@mantine/core";

import { CoinCard } from "../components/coinCard";
import { CoinsStats } from "../components/coinStats";
import { useGetApiLolos } from "../../../api/generated/features/lolos/lolos";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

const CoinsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const coins = useGetApiLolos({ Sorts: "isSpent" });

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
            <Title mb="md">Összevont statisztikák</Title>
            <SimpleGrid cols={2} breakpoints={[{ maxWidth: theme.breakpoints.sm, cols: 1, spacing: "sm" }]}>
                <CoinsStats data={coins.data} />
            </SimpleGrid>
            <Title my="md">Összes érme</Title>
            <SimpleGrid
                cols={4}
                breakpoints={[
                    { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                    { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                    { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                ]}
            >
                {coins.data?.map((coin) => (
                    <CoinCard key={coin.id} coin={coin} />
                ))}
            </SimpleGrid>
        </>
    );
};

export default CoinsPage;
