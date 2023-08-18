import { Box, Group, Paper, Stack, Text, createStyles, useMantineTheme } from "@mantine/core";

import { AuthIndexUserGroupsResponse } from "../../../api/generated/models";
import { IconUsersGroup } from "@tabler/icons-react";

const useStyles = createStyles(() => ({
    card: {
        cursor: "pointer",
        overflow: "hidden",
    },
}));

export const UserGroupCard = ({
    userGroup,
    openDetails,
}: {
    userGroup: AuthIndexUserGroupsResponse;
    openDetails(AuthIndexUserGroupsResponse): void;
}): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    return (
        <Paper withBorder={true} radius="md" p="xs" className={classes.card} onClick={() => openDetails(userGroup)}>
            <Group position="apart" maw="100%" sx={{ flexWrap: "nowrap" }}>
                <Stack justify="space-between" align="stretch" spacing={0} sx={{ flex: 1, overflow: "hidden" }}>
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {userGroup.name}
                        </Text>
                    </Box>
                    <Text size="sm" color="dimmed" truncate={true}>
                        {userGroup.permissions.length} db jogosultság
                    </Text>
                </Stack>
                <IconUsersGroup stroke={1.5} size={48} color={theme.fn.primaryColor()} />
            </Group>
        </Paper>
    );
};
