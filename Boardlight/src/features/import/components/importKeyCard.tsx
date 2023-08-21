import { Box, Group, Paper, Stack, Text, createStyles, useMantineTheme } from "@mantine/core";

import { IconCircleKey } from "@tabler/icons-react";
import { ImportIndexImportKeysResponse } from "../../../api/generated/models";

const useStyles = createStyles(() => ({
    card: {
        cursor: "pointer",
        overflow: "hidden",
    },
}));

export const ImportKeyCard = ({
    importKey,
    openDetails,
}: {
    importKey: ImportIndexImportKeysResponse;
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
                            {importKey.name}
                        </Text>
                    </Box>
                    <Text size="sm" color="dimmed">
                        Módosítva: {new Date(importKey.updatedAt).toLocaleDateString("hu-HU", {})}
                    </Text>
                </Stack>
                <IconCircleKey
                    stroke={1.5}
                    size={48}
                    color={
                        importKey.enabled
                            ? theme.fn.primaryColor()
                            : theme.colorScheme === "dark"
                            ? theme.colors.gray[7]
                            : theme.colors.gray[5]
                    }
                />
            </Group>
        </Paper>
    );
};
