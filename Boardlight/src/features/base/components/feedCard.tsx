import { ActionIcon, Box, Card, Center, Group, Loader, Paper, Stack, Text, Title, createStyles } from "@mantine/core";

import { IconExternalLink } from "@tabler/icons-react";
import { useGetApiFeedItems } from "../../../api/generated/features/feed-items/feed-items";

const useStyles = createStyles((theme) => ({
    statsCard: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
    },
}));

export const FeedCard = (): JSX.Element => {
    const { classes } = useStyles();

    const feedItems = useGetApiFeedItems({ Page: 1, PageSize: 5 });

    if (feedItems.isLoading) {
        return (
            <Paper radius="md" p="md" className={classes.statsCard} h="100%">
                <Center h="100%">
                    <Loader />
                </Center>
            </Paper>
        );
    }

    if (feedItems.isError) {
        return (
            <Paper radius="md" p="md" className={classes.statsCard} h="100%">
                <Center h="100%">
                    <Text color="red" align="center">
                        Hiba történt az adatok lekérésekor.
                    </Text>
                </Center>
            </Paper>
        );
    }

    return (
        <Paper radius="md" p="md" className={classes.statsCard} h="100%">
            <Title order={2} mb="md">
                Hírfolyam
            </Title>
            <Stack spacing="xs">
                {feedItems.data?.map((item, index) => (
                    <Paper key={index} withBorder={true} p="xs" radius="md">
                        <Group position="apart" maw="100%" sx={{ flexWrap: "nowrap" }}>
                            <Stack spacing={0} sx={{ flex: 1, overflow: "hidden" }}>
                                <Text size="lg" weight={500} truncate={true}>
                                    {item.title}
                                </Text>
                                <Text truncate={true}>{item.description}</Text>
                                <Text size="sm" color="dimmed" truncate={true}>
                                    {new Date(item.createdAt).toLocaleDateString("hu-HU", {})}
                                </Text>
                            </Stack>
                            <ActionIcon
                                variant="transparent"
                                size={48}
                                component="a"
                                href={item.link ?? "#"}
                                target="_blank"
                            >
                                <IconExternalLink stroke={1.5} />
                            </ActionIcon>
                        </Group>
                    </Paper>
                ))}
            </Stack>
        </Paper>
    );
};
