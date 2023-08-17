import { Box, Button, Center, Group, Text, Title, createStyles, rem } from "@mantine/core";

import { Link } from "react-router-dom";
import { useEffect } from "react";

const useStyles = createStyles((theme) => ({
    root: {
        position: "fixed",
        top: 0,
        left: 0,
        height: "100%",
        zIndex: 9000,
        width: "100%",
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[8] : theme.white,
    },

    content: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
    },

    label: {
        textAlign: "center",
        fontWeight: 900,
        fontSize: rem(220),
        lineHeight: 1,
        marginBottom: `calc(${theme.spacing.xl} * 1.5)`,
        color: theme.colorScheme === "dark" ? theme.colors.dark[4] : theme.colors.gray[2],

        [theme.fn.smallerThan("sm")]: {
            fontSize: rem(120),
        },
    },

    title: {
        fontFamily: `Greycliff CF, ${theme.fontFamily}`,
        textAlign: "center",
        fontWeight: 900,
        fontSize: rem(38),

        [theme.fn.smallerThan("sm")]: {
            fontSize: rem(32),
        },

        [theme.fn.smallerThan("xs")]: {
            fontSize: rem(24),
        },
    },

    description: {
        maxWidth: rem(600),
        margin: "auto",
        marginTop: theme.spacing.xl,
        marginBottom: `calc(${theme.spacing.xl} * 1.5)`,
    },
}));

const FourOFour = (): JSX.Element => {
    const { classes } = useStyles();

    useEffect(() => {
        const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        const scrollLeft = window.pageXOffset || document.documentElement.scrollLeft;

        window.onscroll = () => {
            window.scrollTo(scrollLeft, scrollTop);
        };

        return () => {
            window.onscroll = null;
        };
    }, []);

    return (
        <Center className={classes.root}>
            <Box className={classes.content} p="md">
                <div className={classes.label}>404</div>
                <Title className={classes.title}>Hoppá! Úgy néz ki eltévedtél...</Title>
                <Text color="dimmed" size="lg" align="center" className={classes.description}>
                    Ez csak egy 404-es hibaoldal. Nem hiszem, hogy ezt kerested... Nézd meg, hogy helyesen írtad-e be a
                    címet!
                </Text>
                <Group position="center">
                    <Button component={Link} to="/" variant="outline">
                        Vissza a kezdőlapra
                    </Button>
                </Group>
            </Box>
        </Center>
    );
};

export default FourOFour;
