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
import { useMemo, useState } from "react";

import { SchoolIndexGradesResponseGrade } from "../../../api/generated/models";
import { useGetApiGrades } from "../../../api/generated/features/grades/grades";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
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

const SubjectCard = ({
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

const GradeCard = ({ grade }: { grade: SchoolIndexGradesResponseGrade }): JSX.Element => {
    const color =
        grade.gradeValue === 5
            ? "green"
            : grade.gradeValue === 4
            ? "lime"
            : grade.gradeValue === 3
            ? "yellow"
            : grade.gradeValue === 2
            ? "orange"
            : grade.gradeValue === 1
            ? "red"
            : "blue";

    return (
        <Paper withBorder={true} radius="md" p="sm">
            <Group position="apart" align="center" maw="100%" sx={{ flexWrap: "nowrap" }}>
                <Stack justify="space-between" align="stretch" spacing={0} sx={{ flex: 1, overflow: "hidden" }}>
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {grade.name}
                        </Text>
                    </Box>
                    <Text size="sm" color="dimmed">
                        {grade.teacher}
                    </Text>
                    <Text size="sm" color="dimmed">
                        {new Date(grade.evaluationDate).toLocaleDateString("hu-HU", {})} - {grade.weight}%
                    </Text>
                </Stack>
                <Text size={rem(60)} sx={{ lineHeight: rem(60) }} color={color} align="center" weight={500} pr="md">
                    {grade.gradeValue !== 0 ? grade.gradeValue : "-"}
                </Text>
            </Group>
        </Paper>
    );
};

const GradesPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const grades = useGetApiGrades();

    const [activeSubject, setActiveSubject] = useState<number | undefined>(undefined);

    const subjectCards = useMemo(
        () =>
            grades.data?.map((subject, index) => (
                <SubjectCard
                    key={subject.subject}
                    subject={subject.subject}
                    grades={subject.grades}
                    index={index}
                    active={activeSubject}
                    setActive={setActiveSubject}
                />
            )),
        [activeSubject, grades.data]
    );

    if (grades.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (grades.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <Title mb="md">Tantárgyak</Title>
            <SimpleGrid
                cols={4}
                breakpoints={[
                    { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                    { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                    { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                ]}
            >
                {subjectCards}
            </SimpleGrid>
            <Title my="md">Jegyek</Title>
            {activeSubject === undefined ? (
                <Text color="dimmed">Válassz ki egy tantárgyat, hogy láthasd a jegyeidet!</Text>
            ) : (
                <SimpleGrid
                    cols={4}
                    breakpoints={[
                        { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                        { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                        { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                    ]}
                >
                    {grades.data[activeSubject].grades.map((grade) => (
                        <GradeCard key={grade.id} grade={grade} />
                    ))}
                </SimpleGrid>
            )}
        </>
    );
};

export default GradesPage;
