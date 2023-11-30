import { Group, Paper, RingProgress, Stack, Text, Title, createStyles } from "@mantine/core";

import { SchoolIndexGradesResponseGrade } from "../../../api/generated/models";

const useStyles = createStyles((theme) => ({
    statsCard: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
    },
}));

export const GradesStats = ({ grades }: { grades: SchoolIndexGradesResponseGrade[] }): JSX.Element[] => {
    const { classes } = useStyles();

    return [
        <Paper radius="md" p="md" className={classes.statsCard} key="1">
            <Title order={2} mb="md">
                Jegyek eloszlása
            </Title>
            <Group position="apart">
                <Stack spacing={0}>
                    <Text>
                        Jeles:{" "}
                        <Text component="span" color="green" weight="bold">
                            {grades.filter((grade) => grade.gradeValue == 5).length} db
                        </Text>{" "}
                        ({Math.round((grades.filter((grade) => grade.gradeValue == 5).length / grades.length) * 100)}%)
                    </Text>
                    <Text>
                        Jó:{" "}
                        <Text component="span" weight="bold" color="lime">
                            {grades.filter((grade) => grade.gradeValue == 4).length} db
                        </Text>{" "}
                        ({Math.round((grades.filter((grade) => grade.gradeValue == 4).length / grades.length) * 100)}%)
                    </Text>
                    <Text>
                        Közepes:{" "}
                        <Text component="span" weight="bold" color="yellow">
                            {grades.filter((grade) => grade.gradeValue == 3).length} db
                        </Text>{" "}
                        ({Math.round((grades.filter((grade) => grade.gradeValue == 3).length / grades.length) * 100)}%)
                    </Text>
                    <Text>
                        Elégséges:{" "}
                        <Text component="span" weight="bold" color="orange">
                            {grades.filter((grade) => grade.gradeValue == 2).length} db
                        </Text>{" "}
                        ({Math.round((grades.filter((grade) => grade.gradeValue == 2).length / grades.length) * 100)}%)
                    </Text>
                    <Text>
                        Elégtelen:{" "}
                        <Text component="span" weight="bold" color="red">
                            {grades.filter((grade) => grade.gradeValue == 1).length} db
                        </Text>{" "}
                        ({Math.round((grades.filter((grade) => grade.gradeValue == 1).length / grades.length) * 100)}%)
                    </Text>
                </Stack>
                <RingProgress
                    sections={[
                        {
                            value: (grades.filter((grade) => grade.gradeValue == 5).length / grades.length) * 100,
                            color: "green",
                            tooltip: `Ötösök - ${
                                grades.filter((grade) => grade.gradeValue == 5).length
                            } db - ${Math.round(
                                (grades.filter((grade) => grade.gradeValue == 5).length / grades.length) * 100
                            )}%`,
                        },
                        {
                            value: (grades.filter((grade) => grade.gradeValue == 4).length / grades.length) * 100,
                            color: "lime",
                            tooltip: `Négyesek - ${
                                grades.filter((grade) => grade.gradeValue == 4).length
                            } db - ${Math.round(
                                (grades.filter((grade) => grade.gradeValue == 4).length / grades.length) * 100
                            )}%`,
                        },
                        {
                            value: (grades.filter((grade) => grade.gradeValue == 3).length / grades.length) * 100,
                            color: "yellow",
                            tooltip: `Hármasok - ${
                                grades.filter((grade) => grade.gradeValue == 3).length
                            } db - ${Math.round(
                                (grades.filter((grade) => grade.gradeValue == 3).length / grades.length) * 100
                            )}%`,
                        },
                        {
                            value: (grades.filter((grade) => grade.gradeValue == 2).length / grades.length) * 100,
                            color: "orange",
                            tooltip: `Kettesek - ${
                                grades.filter((grade) => grade.gradeValue == 2).length
                            } db - ${Math.round(
                                (grades.filter((grade) => grade.gradeValue == 2).length / grades.length) * 100
                            )}%`,
                        },
                        {
                            value: (grades.filter((grade) => grade.gradeValue == 1).length / grades.length) * 100,
                            color: "red",
                            tooltip: `Egyesek - ${
                                grades.filter((grade) => grade.gradeValue == 1).length
                            } db - ${Math.round(
                                (grades.filter((grade) => grade.gradeValue == 1).length / grades.length) * 100
                            )}%`,
                        },
                    ]}
                    sx={{ alignSelf: "center" }}
                />
            </Group>
        </Paper>,
    ];
};
