import { Center, Loader, SimpleGrid, Text, Title, createStyles, useMantineTheme } from "@mantine/core";

import { CoinCard } from "../components/coinCard";
import { CoinsStats } from "../components/coinStats";
import { useGetApiGrades } from "../../../api/generated/features/grades/grades";
import { useGetApiLolosOwn } from "../../../api/generated/features/lolos/lolos";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

const OwnCoinsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const coins = useGetApiLolosOwn({ Sorts: "isSpent" });
    const grades = useGetApiGrades();

    if (coins.isLoading || grades.isLoading)
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
                <CoinsStats data={coins.data} grades={grades.isError ? undefined : grades.data} />
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
                {coins.data?.coins.map((coin) => <CoinCard key={coin.id} coin={coin} />)}
            </SimpleGrid>
            {coins.data.coins.length === 0 && (
                <Text color="dimmed">
                    Úgy néz ki még nincs egy lolód sem... Amint lesz, itt látod majd a loló érméidet.
                </Text>
            )}
        </>
    );
};

export default OwnCoinsPage;
