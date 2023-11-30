import {
    Box,
    Center,
    Group,
    Loader,
    MediaQuery,
    Modal,
    Select,
    SimpleGrid,
    Text,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { useMemo, useState } from "react";
import { useDisclosure } from "@mantine/hooks";

import { GradeCard } from "../components/gradeCard";
import { GradesStats } from "../components/gradesStats";
import { SubjectCard } from "../components/subjectCard";
import { useGetApiGrades } from "../../../api/generated/features/grades/grades";
import { SchoolIndexGradesResponseGrade } from "../../../api/generated/models";

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

const DetailsModal = ({
    grade,
    opened,
    close
}: {
    grade: SchoolIndexGradesResponseGrade;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    return (
        <Modal opened={opened} onClose={close} title="Részletek" size="lg">
            <Group position="apart" spacing={0}>
                <Text weight="bold">Osztályzat:</Text>
                <Text>{grade?.textGrade} ({grade?.gradeValue})</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text weight="bold">Súly:</Text>
                <Text>{grade?.weight}%</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text weight="bold">Típus:</Text>
                <Text>{grade?.type}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text weight="bold">Téma:</Text>
                <Text>{grade?.theme}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text weight="bold">Tanár:</Text>
                <Text>{grade?.teacher}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text weight="bold">Tantárgy:</Text>
                <Text>{grade?.subject}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text weight="bold">Csoport:</Text>
                <Text>{grade?.group}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text weight="bold">Jegytípus:</Text>
                <Text>{grade?.gradeType}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text weight="bold">Értékelés ideje:</Text>
                <Text>{new Date(grade?.evaluationDate).toLocaleString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text weight="bold">Létrehozva:</Text>
                <Text>{new Date(grade?.createDate).toLocaleString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text weight="bold">Importálva:</Text>
                <Text>{new Date(grade?.createdAt).toLocaleString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text weight="bold">Módosítva:</Text>
                <Text>{new Date(grade?.updatedAt).toLocaleString("hu-HU", {})}</Text>
            </Group>
        </Modal>
    );
};

const GradesPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const grades = useGetApiGrades();

    const [activeSubject, setActiveSubject] = useState<number | undefined>(undefined);
    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalGrade, setDetailsModalGrade] = useState<SchoolIndexGradesResponseGrade>();

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
            grades.data?.map((subject, index) => {
                const average =
                    subject.grades
                        .filter((g) => g.gradeValue !== 0)
                        .reduce((acc, grade) => acc + grade.gradeValue * grade.weight, 0) /
                    subject.grades.filter((g) => g.gradeValue !== 0).reduce((acc, grade) => acc + grade.weight, 0);

                return {
                    value: index.toString(),
                    label: isNaN(average) ? `${subject.subject}` : `${subject.subject} - ${average.toFixed(2)}`,
                };
            }),
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
            <DetailsModal
                grade={detailsModalGrade}
                opened={detailsModalOpened}
                close={closeDetailsModal}
            />
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
                <>
                    <SimpleGrid
                        cols={2}
                        breakpoints={[{ maxWidth: theme.breakpoints.sm, cols: 1, spacing: "sm" }]}
                        mb="md"
                    >
                        <GradesStats grades={grades.data[activeSubject].grades} />
                    </SimpleGrid>
                    <SimpleGrid
                        cols={4}
                        breakpoints={[
                            { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                            { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                            { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                        ]}
                    >
                        {grades.data[activeSubject].grades.map((grade) => (
                            <GradeCard key={grade.id} grade={grade} openDetails={() => {
                                setDetailsModalGrade(grade);
                                openDetailsModal();
                            }
                            }/>
                        ))}
                    </SimpleGrid>
                </>
            )}
        </>
    );
};

export default GradesPage;
