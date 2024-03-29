import { Anchor, Box, Button, Center, LoadingOverlay, Text, TextInput, Title, createStyles } from "@mantine/core";
import { IconCheck, IconMail } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";

import { Link } from "react-router-dom";
import { UnavailableModal } from "../components/unavailableModal";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useEffect } from "react";
import { useForm } from "@mantine/form";
import { useGetApiStatusServiceStatus } from "../../../api/generated/features/status/status";
import { usePostApiAuthSendPasswordReset } from "../../../api/generated/features/auth/auth";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100vh",
    },
    content: {
        width: "25vw",

        [theme.fn.smallerThan("lg")]: {
            width: "40vw",
        },

        [theme.fn.smallerThan("md")]: {
            width: "45vw",
        },

        [theme.fn.smallerThan("sm")]: {
            width: "65vw",
        },

        [theme.fn.smallerThan("xs")]: {
            width: "75vw",
        },
    },
}));

const ForgotPasswordPage = (): JSX.Element => {
    const { classes } = useStyles();

    const status = useGetApiStatusServiceStatus();
    const sendPasswordReset = usePostApiAuthSendPasswordReset();

    const [unavaliableModalOpened, { open: openUnavailableModal }] = useDisclosure();

    useEffect(() => {
        if (status.isSuccess && !status.data?.serviceStatus.resetKeyPassword) {
            openUnavailableModal();
        }
    }, [status]);

    const form = useForm({
        initialValues: {
            email: "",
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await sendPasswordReset.mutateAsync({
                data: values,
                params: {
                    passwordResetUrl: `${window.location.origin}/auth/reset-password`,
                    passwordResetTokenQueryKey: "resetToken",
                },
            });
            notifications.show({
                title: "Email elküldve",
                message:
                    "Az email sikeresen el lett küldve. Ne felejtsd el megnézni a spam mappádat is, ha nem találod!",
                color: "green",
                icon: <IconCheck />,
            });
        } catch (err) {
            if (err instanceof ValidationError) handleValidationErrors(err, form);
        }
    });

    return (
        <>
            <UnavailableModal opened={unavaliableModalOpened} />
            <Center className={classes.center}>
                <Box pos="relative">
                    <LoadingOverlay radius="md" visible={status.isLoading} />
                    <Box className={classes.content} m="md">
                        <Title align="center" mb="sm">
                            Jelszó visszaállítása
                        </Title>
                        <form onSubmit={submit}>
                            <TextInput
                                label="Email"
                                type="email"
                                description="Kérlek azt az email címed add meg, amely a fiókodhoz tartozik!"
                                icon={<IconMail size={20} stroke={1.5} />}
                                mb="sm"
                                required={true}
                                {...form.getInputProps("email")}
                            />
                            <Button type="submit" fullWidth={true} mb="md" loading={sendPasswordReset.isLoading}>
                                Email küldése
                            </Button>
                        </form>
                        <Text align="center" size="sm">
                            <Anchor component={Link} to="/auth/login">
                                Vissza a bejelentkezéshez
                            </Anchor>
                        </Text>
                    </Box>
                </Box>
            </Center>
        </>
    );
};

export default ForgotPasswordPage;
