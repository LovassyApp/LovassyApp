import { Box, Paper, Text, Title, createStyles } from "@mantine/core";

const useStyles = createStyles((theme) => ({
    statsCard: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
    },
}));

export const FeedCard = (): JSX.Element => {
    const { classes } = useStyles();

    return (
        <Paper radius="md" p="md" className={classes.statsCard} h="100%">
            <Title order={2} mb="md">
                Hírfolyam
            </Title>
            <Text color="dimmed">Úgy néz ki, hogy jelenleg üres a hírfolyamod. Nézz vissza később!</Text>
        </Paper>
    );
};
