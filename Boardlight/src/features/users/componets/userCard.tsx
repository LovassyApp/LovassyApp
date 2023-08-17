import { Box, Group, Paper, Stack, Text, createStyles, useMantineTheme } from "@mantine/core";

import { IconUser } from "@tabler/icons-react";
import { UsersIndexUsersResponse } from "../../../api/generated/models";

const useStyles = createStyles(() => ({
    card: {
        cursor: "pointer",
        overflow: "hidden",
    },
}));

export const UserCard = ({
    user,
    openDetails,
}: {
    user: UsersIndexUsersResponse;
    openDetails(UsersIndexUsersResponse): void;
}): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    return (
        <Paper withBorder={true} radius="md" p="xs" className={classes.card} onClick={() => openDetails(user)}>
            <Group position="apart" maw="100%" sx={{ flexWrap: "nowrap" }}>
                <Stack justify="space-between" align="stretch" spacing={0} sx={{ flex: 1, overflow: "hidden" }}>
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {user.name}{" "}
                            {user.realName && (
                                <Text component="span" color="dimmed">
                                    ({user.realName} - {user.class})
                                </Text>
                            )}
                        </Text>
                    </Box>
                    <Text size="sm" color="dimmed" truncate={true}>
                        {user.email}
                    </Text>
                </Stack>
                <IconUser
                    stroke={1.5}
                    size={48}
                    color={
                        user.emailVerifiedAt
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
