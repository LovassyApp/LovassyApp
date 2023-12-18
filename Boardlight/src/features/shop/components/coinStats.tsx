import { Group, Paper, RingProgress, Stack, Text, Title, createStyles } from "@mantine/core";
import {
    SchoolIndexGradesResponse,
    ShopIndexLolosResponse,
    ShopIndexOwnLolosResponse,
} from "../../../api/generated/models";

import { useMemo } from "react";

const useStyles = createStyles((theme) => ({
    statsCard: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
    },
}));

export const CoinsStats = ({
    data,
    grades = undefined,
}: {
    data: ShopIndexLolosResponse[] | ShopIndexOwnLolosResponse;
    grades?: SchoolIndexGradesResponse[] | undefined;
}): JSX.Element[] => {
    const { classes } = useStyles();

    const coins = useMemo(() => {
        if ("coins" in data) {
            return data.coins as ShopIndexLolosResponse[];
        }
        return data as ShopIndexLolosResponse[];
    }, [data]);

    const balance = useMemo(() => {
        if ("coins" in data) {
            return data.balance;
        }
        // @ts-ignore
        return data.filter((c) => !c.isSpent).length;
    }, [data]);

    const fromFive = useMemo(
        () =>
            coins?.filter((c) => c.loloType === "FromGrades" && c.reason === "Ötösökből automatikusan generálva")
                .length,
        [coins]
    );

    const fromFour = useMemo(
        () =>
            coins?.filter((c) => c.loloType === "FromGrades" && c.reason === "Négyesekből automatikusan generálva")
                .length,
        [coins]
    );

    const fromRequests = useMemo(() => coins.filter((c) => c.loloType === "FromRequest").length, [coins]);

    const fiveGrades = useMemo(() => {
        if (grades) return grades.flatMap((s) => s.grades).filter((grade) => grade.gradeValue === 5).length;
    }, [grades]);

    const fourGrades = useMemo(() => {
        if (grades) return grades.flatMap((s) => s.grades).filter((grade) => grade.gradeValue === 4).length;
    }, [grades]);

    const stats = [
        <Paper radius="md" p="md" className={classes.statsCard} key="1">
            <Group position="apart">
                <Stack spacing={0}>
                    <Title order={2} mb="sm">
                        Költekezés
                    </Title>
                    <Text>
                        Elérhető:{" "}
                        <Text component="span" weight="bold" color="cyan.5">
                            {balance} db
                        </Text>
                    </Text>
                    <Text>
                        Elköltött:{" "}
                        <Text component="span" weight="bold" color="blue.7">
                            {coins.length - balance} db
                        </Text>
                    </Text>
                    <Text>
                        Összesen:{" "}
                        <Text component="span" weight="bold">
                            {coins.length} db
                        </Text>
                    </Text>
                </Stack>
                <RingProgress
                    sections={[
                        {
                            value: (balance / coins.length) * 100,
                            color: "cyan.5",
                            tooltip: `Elérhető - ${balance} db`,
                        },
                        {
                            value: ((coins.length - balance) / coins.length) * 100,
                            color: "blue.7",
                            tooltip: `Elköltött - ${coins.length - balance} db`,
                        },
                    ]}
                />
            </Group>
        </Paper>,
        <Paper radius="md" p="md" className={classes.statsCard} key="2">
            <Group position="apart">
                <Stack spacing={0}>
                    <Title order={2} mb="sm">
                        Források
                    </Title>
                    <Text>
                        Ötösökből:{" "}
                        <Text component="span" weight="bold" color="pink">
                            {fromFive} db
                        </Text>
                    </Text>
                    <Text>
                        Négyesekből:{" "}
                        <Text component="span" weight="bold" color="grape">
                            {fromFour} db
                        </Text>
                    </Text>
                    <Text>
                        Kérvényekből:{" "}
                        <Text component="span" weight="bold" color="violet">
                            {fromRequests} db
                        </Text>
                    </Text>
                </Stack>
                <RingProgress
                    sections={[
                        {
                            value: (fromFive / coins.length) * 100,
                            color: "pink",
                            tooltip: `Ötösökből - ${fromFive} db`,
                        },
                        {
                            value: (fromFour / coins.length) * 100,
                            color: "grape",
                            tooltip: `Négyesekből - ${fromFour} db`,
                        },
                        {
                            value: (fromRequests / coins.length) * 100,
                            color: "violet",
                            tooltip: `Kérvényekből - ${fromRequests} db`,
                        },
                    ]}
                />
            </Group>
        </Paper>,
    ];

    if (grades) {
        stats.push(
            <Paper radius="md" p="md" className={classes.statsCard} key="3">
                <Group position="apart">
                    <Stack spacing={0}>
                        <Title order={2} mb="sm">
                            Új loló ötösökből
                        </Title>
                        <Text>
                            Meglévő jegyek:{" "}
                            <Text component="span" weight="bold" color="pink">
                                {fiveGrades - fromFive * 3} db
                            </Text>
                        </Text>
                        <Text>
                            Szükséges jegyek:{" "}
                            <Text component="span" weight="bold" color="grape">
                                {3 - (fiveGrades - fromFive * 3)} db
                            </Text>
                        </Text>
                    </Stack>
                    <RingProgress
                        sections={[
                            {
                                value: ((5 - (fiveGrades - fromFive * 3)) / 3) * 100,
                                color: "cyan.5",
                                tooltip: `Meglévő jegyek - ${fiveGrades - fromFive * 3} db`,
                            },
                            {
                                value: ((fiveGrades - fromFive * 3) / 3) * 100,
                                color: "blue.7",
                                tooltip: `Szükséges jegyek - ${3 - (fiveGrades - fromFive * 3)} db`,
                            },
                        ]}
                    />
                </Group>
            </Paper>
        );

        stats.push(
            <Paper radius="md" p="md" className={classes.statsCard} key="4">
                <Group position="apart">
                    <Stack spacing={0}>
                        <Title order={2} mb="sm">
                            Új loló négyeskből
                        </Title>
                        <Text>
                            Meglévő jegyek:{" "}
                            <Text component="span" weight="bold" color="pink">
                                {fourGrades - fromFour * 5} db
                            </Text>
                        </Text>
                        <Text>
                            Szükséges jegyek:{" "}
                            <Text component="span" weight="bold" color="grape">
                                {5 - (fourGrades - fromFour * 5)} db
                            </Text>
                        </Text>
                    </Stack>
                    <RingProgress
                        sections={[
                            {
                                value: ((5 - (fourGrades - fromFour * 5)) / 5) * 100,
                                color: "cyan.5",
                                tooltip: `Meglévő jegyek - ${fourGrades - fromFour * 5} db`,
                            },
                            {
                                value: ((fourGrades - fromFour * 5) / 5) * 100,
                                color: "blue.7",
                                tooltip: `Szükséges jegyek - ${5 - (fourGrades - fromFour * 5)} db`,
                            },
                        ]}
                    />
                </Group>
            </Paper>
        );
    }

    return stats;
};
