import { Center, Loader, Paper, RingProgress, Stack, Text, Title, createStyles } from "@mantine/core";

import { useGetApiGrades } from "../../../api/generated/features/grades/grades";
import { useMemo } from "react";

const useStyles = createStyles((theme) => ({
    statsCard: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
    },
}));

export const GradesAverageCard = (): JSX.Element => {
    const { classes } = useStyles();

    const grades = useGetApiGrades();

    const average = useMemo(() => {
        if (grades.data) {
            return (
                grades.data.flatMap((s) => s.grades).reduce((acc, grade) => acc + grade.gradeValue * grade.weight, 0) /
                grades.data.flatMap((s) => s.grades).reduce((acc, grade) => acc + grade.weight, 0)
            );
        }
        return 0;
    }, [grades.data]);

    const color =
        average >= 4.5
            ? "green"
            : average >= 3.5
            ? "lime"
            : average >= 2.5
            ? "yellow"
            : average >= 1.5
            ? "orange"
            : "red";

    if (grades.isLoading) {
        return (
            <Paper radius="md" p="md" className={classes.statsCard} h="100%">
                <Center h="100%">
                    <Loader />
                </Center>
            </Paper>
        );
    }

    if (grades.isError) {
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
            <Center h="100%">
                <Stack spacing="sm">
                    <RingProgress
                        sections={[
                            {
                                value: (average / 5) * 100,
                                color: color,
                            },
                        ]}
                        roundCaps={true}
                        label={
                            <Text size="sm" align="center" weight={500}>
                                {average.toFixed(2)}
                            </Text>
                        }
                        sx={{ alignSelf: "center" }}
                    />
                    <Title order={2} align="center">
                        Jegyek átlaga
                    </Title>
                    <Text align="center" color="dimmed">
                        {grades.data?.flatMap((s) => s.grades).length} db jegy
                    </Text>
                </Stack>
            </Center>
        </Paper>
    );
};
