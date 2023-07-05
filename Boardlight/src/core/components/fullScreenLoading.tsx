import { Box, Center, Loader, createStyles } from "@mantine/core";

const useStyles = createStyles((theme) => ({
    container: {
        position: "fixed",
        top: 0,
        left: 0,
        height: "100%",
        zIndex: 1000,
        width: "100%",
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[8] : theme.white,
    },
    content: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
    },
}));

export const FullScreenLoading = (): JSX.Element => {
    const { classes } = useStyles();

    return (
        <Center className={classes.container}>
            <Box className={classes.content}>
                <Loader variant="bars" size="xl" mb={20} />
            </Box>
        </Center>
    );
};
