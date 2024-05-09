import { Divider, Group, Modal, Text } from "@mantine/core";
import { SchoolIndexGradesResponseGrade } from "../../../api/generated/models";

export const GradeDetailsModal = ({
    grade,
    opened,
    close,
}: {
    grade: SchoolIndexGradesResponseGrade;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    return (
        <Modal opened={opened} onClose={close} title="Részletek" size="lg">
            <Group position="apart" spacing={0}>
                <Text>Osztályzat:</Text>
                <Text weight="bold">
                    {grade?.textGrade} ({grade?.gradeValue})
                </Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Tantárgy:</Text>
                <Text weight="bold">{grade?.subject}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Tanár:</Text>
                <Text weight="bold">{grade?.teacher}</Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Téma:</Text>
                <Text weight="bold">{grade?.theme}</Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Csoport:</Text>
                <Text weight="bold">{grade?.group}</Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Típus:</Text>
                <Text weight="bold">{grade?.type}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Súly:</Text>
                <Text weight="bold">{grade?.weight}%</Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Értékelés ideje:</Text>
                <Text weight="bold">{new Date(grade?.evaluationDate).toLocaleString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Létrehozva a Krétában:</Text>
                <Text weight="bold">{new Date(grade?.createDate).toLocaleString("hu-HU", {})}</Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Importálva:</Text>
                <Text weight="bold">{new Date(grade?.createdAt).toLocaleString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Módosítva:</Text>
                <Text weight="bold">{new Date(grade?.updatedAt).toLocaleString("hu-HU", {})}</Text>
            </Group>
        </Modal>
    );
};