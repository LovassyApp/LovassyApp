import { Box, Button, Center, Group, Text, Title, createStyles, rem } from "@mantine/core";
import { ReactNode, useCallback, useEffect, useRef, useState } from "react";

import { FullScreenLoading } from "./fullScreenLoading";
import { Link } from "react-router-dom";
import { StatusViewServiceStatusResponse } from "../../api/generated/models";
import { getApiStatusServiceStatus } from "../../api/generated/features/status/status";

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

export const BlueboardStatusChecker = ({ children }: { children: ReactNode }) => {
    const { classes } = useStyles();

    const statusQuery = useCallback(() => getApiStatusServiceStatus(), []);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(false);

    const interval = useRef<any>();

    const readyCallback = useCallback((res: StatusViewServiceStatusResponse) => {
        if (res.ready) {
            clearInterval(interval.current);
            setError(false);
            setLoading(false);
        }
    }, []);

    const intervalCallback = useCallback(() => {
        (async () => {
            try {
                const res = await statusQuery();
                readyCallback(res);
            } catch (err) {
                setError(true);
                setLoading(false);
            }
        })();
    }, [readyCallback, statusQuery]);

    const initialCallback = useCallback(async () => {
        try {
            setLoading(true);
            const res = await statusQuery();
            readyCallback(res);
        } catch (err) {
            setError(true);
            setLoading(false);
            interval.current = setInterval(intervalCallback, 5000);
        }
    }, [intervalCallback, readyCallback, statusQuery]);

    useEffect(() => {
        initialCallback();
    }, []);

    if (loading) return <FullScreenLoading />;

    if (error)
        return (
            <Center className={classes.root}>
                <Box className={classes.content} p="md">
                    <div className={classes.label}>503</div>
                    <Title className={classes.title}>Hoppá! Úgy néz ki a LovassyApp nem elérhető...</Title>
                    <Text color="dimmed" size="lg" align="center" className={classes.description}>
                        Semmi baj, valószínűleg csak karbantartás zajlik. Kérlek próbálkozz később! Amint újra elérhető
                        lesz az alkalmazás, ez az oldal automatikusan frissülni fog.
                    </Text>
                </Box>
            </Center>
        );

    return <>{children}</>;
};
