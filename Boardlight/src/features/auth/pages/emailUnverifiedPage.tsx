import { Box, Button, Center, Text, Title, createStyles } from "@mantine/core";

import { usePostApiAuthResendVerifyEmail } from "../../../api/generated/features/auth/auth";

const useStyles = createStyles(() => ({
    center: {
        height: "100vh",
    },
    container: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
    },
}));

const EmailUnverifiedPage = (): JSX.Element => {
    const { classes } = useStyles();

    const resendVerifyEmail = usePostApiAuthResendVerifyEmail();

    return (
        <Center className={classes.center}>
            <Box className={classes.container} m="md">
                <Title align="center" mb="sm">
                    Email megerősítése
                </Title>
                <Text align="center">
                    Kérlek erősítsd meg az email címedet, hogy hozzá tudj férni a LovassyApp többi funkciójához!
                </Text>
                <Text align="center" mb="md">
                    Ha nem találod a megerősítésről szóló emailt, kérlek nézd meg a spam mappádat is!
                </Text>
                <Button
                    onClick={async () =>
                        await resendVerifyEmail.mutateAsync({
                            params: {
                                verifyUrl: `${window.location.origin}/auth/verify-email`,
                                verifyTokenQueryKey: "verifyToken",
                            },
                        })
                    }
                    loading={resendVerifyEmail.isLoading}
                >
                    Email újraküldése
                </Button>
            </Box>
        </Center>
    );
};

export default EmailUnverifiedPage;
