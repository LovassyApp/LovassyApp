import { Badge, Box, Divider, Group, Paper, Stack, Text, createStyles, rem, useMantineTheme } from "@mantine/core";
import {
    IconAlphabetLatin,
    IconCircleCheck,
    IconCircleX,
    IconCoin,
    IconHelpCircle,
    IconNote,
    IconQuestionMark,
} from "@tabler/icons-react";

import { ShopIndexLoloRequestsResponse } from "../../../api/generated/models";
import { modals } from "@mantine/modals";

const useStyles = createStyles((theme) => ({
    card: {
        cursor: "pointer",
        overflow: "hidden",
    },
}));

const DetailsModalContents = ({ loloRequest }: { loloRequest: ShopIndexLoloRequestsResponse }): JSX.Element => {
    return (
        <>
            <Group position="apart" spacing={0}>
                <Text>Cím:</Text>
                <Text weight="bold">{loloRequest.title}</Text>
            </Group>
            <Text>Törzsszöveg:</Text>
            <Text weight="bold">{loloRequest.body}</Text>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Állapot:</Text>
                {loloRequest.acceptedAt && (
                    <Text weight="bold" color="green">
                        Elfogadva
                    </Text>
                )}
                {loloRequest.deniedAt && (
                    <Text weight="bold" color="red">
                        Elutasítva
                    </Text>
                )}
                {!loloRequest.acceptedAt && !loloRequest.deniedAt && (
                    <Text weight="bold" color="yellow">
                        Függőben
                    </Text>
                )}
            </Group>
            {loloRequest.acceptedAt && (
                <Group position="apart" spacing={0}>
                    <Text>Elfogadás dátuma:</Text>
                    <Text weight="bold">{new Date(loloRequest.acceptedAt).toLocaleDateString("hu-HU", {})}</Text>
                </Group>
            )}
            {loloRequest.deniedAt && (
                <Group position="apart" spacing={0}>
                    <Text>Elutasítás dátuma:</Text>
                    <Text weight="bold">{new Date(loloRequest.deniedAt).toLocaleDateString("hu-HU", {})}</Text>
                </Group>
            )}
            <Group position="apart" spacing={0}>
                <Text>Létrehozás dátuma:</Text>
                <Text weight="bold">{new Date(loloRequest.createdAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
        </>
    );
};

export const LoloRequestCard = ({ loloRequest }: { loloRequest: ShopIndexLoloRequestsResponse }): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const openDetails = (): void => {
        modals.open({
            title: "Részletek",
            children: <DetailsModalContents loloRequest={loloRequest} />,
            size: "lg",
        });
    };

    return (
        <Paper withBorder={true} radius="md" p="xs" className={classes.card} onClick={() => openDetails()}>
            <Group position="apart" maw="100%" sx={{ flexWrap: "nowrap" }}>
                <Stack justify="space-between" align="stretch" spacing={0} sx={{ flex: 1, overflow: "hidden" }}>
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {loloRequest.title}
                        </Text>
                    </Box>
                    <Box maw="100%">
                        <Text size="sm" color="dimmed" truncate={true}>
                            {loloRequest.body}
                        </Text>
                    </Box>
                </Stack>
                {loloRequest.deniedAt && (
                    <IconCircleX
                        stroke={1.5}
                        size={rem(48)}
                        color={theme.colors.red[theme.colorScheme === "dark" ? 6 : 7]}
                    />
                )}
                {loloRequest.acceptedAt && (
                    <IconCircleCheck
                        stroke={1.5}
                        size={rem(48)}
                        color={theme.colors.green[theme.colorScheme === "dark" ? 6 : 7]}
                    />
                )}
                {!loloRequest.acceptedAt && !loloRequest.deniedAt && (
                    <IconHelpCircle
                        stroke={1.5}
                        size={rem(48)}
                        color={theme.colors.yellow[theme.colorScheme === "dark" ? 6 : 7]}
                    />
                )}
            </Group>
        </Paper>
    );
};
