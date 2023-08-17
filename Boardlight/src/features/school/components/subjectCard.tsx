import { Box, Group, Paper, RingProgress, Stack, Text, createStyles, rem } from "@mantine/core";

import { SchoolIndexGradesResponseGrade } from "../../../api/generated/models";

const useStyles = createStyles((theme) => ({
    card: {
        cursor: "pointer",
        overflow: "hidden",
        transition: "box-shadow 100ms ease",
    },
    cardActive: {
        boxShadow: `inset 0 0 0 ${rem(1)} ${theme.colors.blue[6]}`,
        borderColor: `${theme.colors.blue[6]} !important`,
    },
}));

export const SubjectCard = ({
    subject,
    grades,
    active,
    index,
    setActive,
}: {
    subject: string;
    grades: SchoolIndexGradesResponseGrade[];
    active: number;
    index: number;
    setActive(index?: number): void;
}): JSX.Element => {
    const { classes, cx } = useStyles();

    const average =
        grades.reduce((acc, grade) => acc + grade.gradeValue * grade.weight, 0) /
        grades.reduce((acc, grade) => acc + grade.weight, 0);

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

    return (
        <Paper
            withBorder={true}
            radius="md"
            className={cx(classes.card, { [classes.cardActive]: active === index })}
            onClick={() => setActive(active === index ? undefined : index)}
            p="xs"
        >
            <Group position="apart" maw="100%" sx={{ flexWrap: "nowrap" }}>
                <Stack justify="space-between" align="stretch" spacing="xs" sx={{ flex: 1, overflow: "hidden" }}>
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {subject}
                        </Text>
                    </Box>
                    <Text size="sm" color="dimmed">
                        {grades.length} jegy
                    </Text>
                </Stack>
                <RingProgress
                    size={60}
                    thickness={5}
                    sections={[{ value: (average / 5) * 100, color: color }]}
                    roundCaps={true}
                    label={
                        <Text size="sm" align="center" weight={500}>
                            {average.toFixed(2)}
                        </Text>
                    }
                />
            </Group>
        </Paper>
    );
};
