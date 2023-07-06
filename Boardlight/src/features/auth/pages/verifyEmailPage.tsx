import { Box, Center, MediaQuery, Stack, Text, Title, createStyles, useMantineTheme } from "@mantine/core";
import { IconCircleCheckFilled, IconCircleX, IconCircleXFilled } from "@tabler/icons-react";

import { FullScreenLoading } from "../../../core/components/fullScreenLoading";
import { useEffect } from "react";
import { usePostApiAuthVerifyEmail } from "../../../api/generated/features/auth/auth";
import { useSearchParams } from "react-router-dom";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100vh",
    },
    errorIcon: {
        color: theme.colors.red[6],
    },
    successIcon: {
        color: theme.colors.green[6],
    },
}));

const VerifyEmailPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const [queryParams] = useSearchParams();
    const verifyEmail = usePostApiAuthVerifyEmail();

    useEffect(() => {
        (async () => {
            await verifyEmail.mutateAsync({ params: { verifyToken: queryParams.get("verifyToken") } });
        })();
    }, []);

    if (verifyEmail.isSuccess)
        return (
            <Center className={classes.center}>
                <Stack align="center" m="md">
                    <IconCircleCheckFilled size={124} className={classes.successIcon} />
                    <Title align="center">Siker!</Title>
                    <Text align="center">
                        Sikeresen megerősítetted az email címedet! Most már nyugodtan bezárhatod ezt az oldalt.
                    </Text>
                </Stack>
            </Center>
        );

    if (verifyEmail.isError)
        return (
            <Center className={classes.center}>
                <Stack align="center" m="md">
                    <IconCircleXFilled size={124} className={classes.errorIcon} />
                    <Title align="center">Hiba!</Title>
                    <Text align="center">
                        Hiba történt... Ez vagy azért van, mert már lejárt a megerősítési link (ha újraküldöd az emailt,
                        akkor új linket kapsz),{" "}
                        <MediaQuery smallerThan="md" styles={{ display: "none" }}>
                            <br />
                        </MediaQuery>{" "}
                        vagy mert már megerősítetted az email címed. Utóbbi esetben nyugodtan bezárhatod ezt az oldalt.
                    </Text>
                </Stack>
            </Center>
        );

    return <FullScreenLoading />;
};

export default VerifyEmailPage;
