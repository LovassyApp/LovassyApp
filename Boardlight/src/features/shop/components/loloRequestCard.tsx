import { Box, Group, Paper, Stack, Text, createStyles, useMantineTheme } from "@mantine/core";
import { IconCircleCheck, IconCircleX, IconHelpCircle } from "@tabler/icons-react";

import { ShopIndexLoloRequestsResponse } from "../../../api/generated/models";

const useStyles = createStyles(() => ({
    card: {
        cursor: "pointer",
        overflow: "hidden",
    },
}));

export const LoloRequestCard = ({
    loloRequest,
    openDetails,
}: {
    loloRequest: ShopIndexLoloRequestsResponse;
    openDetails(): void;
}): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

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
                        size={48}
                        color={theme.colors.red[theme.colorScheme === "dark" ? 6 : 7]}
                    />
                )}
                {loloRequest.acceptedAt && (
                    <IconCircleCheck
                        stroke={1.5}
                        size={48}
                        color={theme.colors.green[theme.colorScheme === "dark" ? 6 : 7]}
                    />
                )}
                {!loloRequest.acceptedAt && !loloRequest.deniedAt && (
                    <IconHelpCircle
                        stroke={1.5}
                        size={48}
                        color={theme.colors.yellow[theme.colorScheme === "dark" ? 6 : 7]}
                    />
                )}
            </Group>
        </Paper>
    );
};
