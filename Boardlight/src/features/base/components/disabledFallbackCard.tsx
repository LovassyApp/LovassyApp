import { Box, Center, Paper, Text, createStyles } from "@mantine/core";

const useStyles = createStyles((theme) => ({
    statsCard: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
    },
}));

export const DisabledFallbackCard = (): JSX.Element => {
    const { classes } = useStyles();

    return (
        <Paper radius="md" p="md" className={classes.statsCard} h="100%">
            <Center h="100%">
                <Text color="red" align="center">
                    Ez a kártya jelenleg nem elérhető.
                </Text>
            </Center>
        </Paper>
    );
};
