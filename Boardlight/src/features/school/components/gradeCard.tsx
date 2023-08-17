import { Box, Group, Paper, Stack, Text, rem } from "@mantine/core";

import { SchoolIndexGradesResponseGrade } from "../../../api/generated/models";

export const GradeCard = ({ grade }: { grade: SchoolIndexGradesResponseGrade }): JSX.Element => {
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
                            {grade.theme}
                        </Text>
                    </Box>
                    <Text size="sm" color="dimmed" truncate={true}>
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
