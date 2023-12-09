import { Center, Loader, Paper, Progress, Stack, Text, Title, createStyles } from "@mantine/core";

import { useGetApiLolosOwn } from "../../../api/generated/features/lolos/lolos";
import { useGetApiGrades } from "../../../api/generated/features/grades/grades";
import { useMemo } from "react";

const useStyles = createStyles((theme) => ({
    statsCard: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
    },
}));

export const LoloStatsCard = (): JSX.Element => {
    const { classes } = useStyles();

    const coins = useGetApiLolosOwn();
    const grades = useGetApiGrades();

    const fromGrades = useMemo(() => coins.data?.coins.filter((c) => c.loloType === "FromGrades").length, [coins.data]);

    const fromRequests = useMemo(
        () => coins.data?.coins.filter((c) => c.loloType === "FromRequest").length,
        [coins.data]
    );

    const gradeFive = useMemo(() => grades.data?.flatMap((s) => s.grades).filter((grade) => grade.gradeValue == 5).length, [grades.data]);
    const gradeFour = useMemo(() => grades.data?.flatMap((s) => s.grades).filter((grade) => grade.gradeValue == 4).length, [grades.data]);
    const fromGradeFive = useMemo(() => coins.data?.coins.filter((c) => c.loloType === "FromGrades" && c.reason === "Ötösökből automatikusan generálva").length, [coins.data]);
    const fromGradeFour = useMemo(() => coins.data?.coins.filter((c) => c.loloType === "FromGrades" && c.reason === "Négyesekből automatikusan generálva").length, [coins.data]);

    if (coins.isLoading) {
        return (
            <Paper radius="md" p="md" className={classes.statsCard} h="100%">
                <Center h="100%">
                    <Loader />
                </Center>
            </Paper>
        );
    }

    if (coins.isError) {
        return (
            <Paper radius="md" p="md" className={classes.statsCard} h="100%">
                <Center h="100%">
                    <Text color="red" align="center">
                        Hiba történt az adatok lekérésekor.
                    </Text>
                </Center>
            </Paper>
        );
    }

    return (
        <Paper radius="md" p="md" className={classes.statsCard} h="100%">
            <Stack spacing={0}>
                <Title order={2} mb="md">
                    Loló érmék
                </Title>
                <Text>
                    Elérhető:{" "}
                    <Text component="span" weight="bold" color="green">
                        {coins.data?.balance} db
                    </Text>
                </Text>
                <Text>
                    Elköltött:{" "}
                    <Text component="span" weight="bold" color="red">
                        {coins.data.coins.length - coins.data?.balance} db
                    </Text>
                </Text>
                <Progress
                    size="xl"
                    sections={[
                        { value: (coins.data?.balance / coins.data.coins.length) * 100, color: "green.6" },
                        {
                            value: ((coins.data.coins.length - coins.data?.balance) / coins.data.coins.length) * 100,
                            color: "red.6",
                        },
                    ]}
                    my="sm"
                />
                <Text>
                    Jegyekből generálva:{" "}
                    <Text component="span" weight="bold" color="pink">
                        {fromGrades} db
                    </Text>
                    {" "}(Még <Text weight="bold" component="span">{3 - (gradeFive - (fromGradeFive * 3))} ötös</Text> vagy <Text component="span" weight="bold">{5 - (gradeFour - (fromGradeFour * 5))} négyes</Text> kell egy újabb lólóhoz.)
                </Text>
                <Text>
                    Kérvényekből generálva:{" "}
                    <Text component="span" weight="bold" color="violet">
                        {fromRequests} db
                    </Text>
                </Text>
                <Progress
                    size="xl"
                    sections={[
                        { value: (fromGrades / coins.data.coins.length) * 100, color: "pink.6" },
                        { value: (fromRequests / coins.data.coins.length) * 100, color: "violet.5" },
                    ]}
                    my="sm"
                />
                <Text>
                    Összesen:{" "}
                    <Text component="span" weight="bold">
                        {coins.data.coins.length} db
                    </Text>
                </Text>
            </Stack>
        </Paper>
    );
};
