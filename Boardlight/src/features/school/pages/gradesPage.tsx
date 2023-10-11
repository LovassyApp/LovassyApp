import {
    Box,
    Center,
    Loader,
    MediaQuery,
    Select,
    SimpleGrid,
    Text,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { useMemo, useState } from "react";

import { GradeCard } from "../components/gradeCard";
import { SubjectCard } from "../components/subjectCard";
import { useGetApiGrades } from "../../../api/generated/features/grades/grades";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
    subjectsContainer: {
        [theme.fn.smallerThan("xs")]: {
            display: "none",
        },
    },
    subjectSelect: {
        [theme.fn.largerThan("xs")]: {
            display: "none",
        },
    },
}));

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

    const subjectValues = useMemo(
        () =>
            grades.data?.map((subject, index) => ({
                value: index.toString(),
                label: `${subject.subject} - ${(
                    subject.grades
                        .filter((g) => g.gradeValue !== 0)
                        .reduce((acc, grade) => acc + grade.gradeValue * grade.weight, 0) /
                    subject.grades.filter((g) => g.gradeValue !== 0).reduce((acc, grade) => acc + grade.weight, 0)
                ).toPrecision(3)}`,
            })),
        [grades.data]
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
            <Box className={classes.subjectsContainer} mb="md">
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
                {subjectCards.length === 0 && (
                    <Text color="dimmed">
                        Úgy néz ki még nem lettek importálva a jegyeid... Amint importálva lesznek, itt látod majd a
                        tantárgyaid.
                    </Text>
                )}
            </Box>
            <Title mb="md">Jegyek</Title>
            {subjectCards.length === 0 && (
                <MediaQuery largerThan="xs" styles={{ display: "none" }}>
                    <Text color="dimmed" mb="sm">
                        Úgy néz ki még nem lettek importálva a jegyeid... Amint importálva lesznek, itt látod majd a
                        tantárgyaid.
                    </Text>
                </MediaQuery>
            )}
            <Select
                label="Tantárgy"
                data={subjectValues}
                value={activeSubject?.toString()}
                onChange={(value) => setActiveSubject(value ? +value : undefined)}
                className={classes.subjectSelect}
                mb="md"
            />
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
