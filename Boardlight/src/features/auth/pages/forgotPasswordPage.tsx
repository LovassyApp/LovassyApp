import { Anchor, Box, Button, Center, LoadingOverlay, Text, TextInput, Title, createStyles } from "@mantine/core";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";

import { IconCheck } from "@tabler/icons-react";
import { Link } from "react-router-dom";
import { UnavailableModalContent } from "../components/unavailableModalContent";
import { modals } from "@mantine/modals";
import { notifications } from "@mantine/notifications";
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

    useEffect(() => {
        if (status.isSuccess && !status.data?.serviceStatus.resetKeyPassword) {
            modals.open({
                children: <UnavailableModalContent />,
                size: "lg",
                withCloseButton: false,
                closeOnEscape: false,
                closeOnClickOutside: false,
            });
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
                    passwordResetUrl: `${window.location.origin}/reset-password`,
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
                            description="Kérlek azt az email címed add meg, amely a fiókodhoz tartozik!"
                            mb="sm"
                            required={true}
                            {...form.getInputProps("email")}
                        />
                        <Button type="submit" fullWidth={true} mb="md">
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
    );
};

export default ForgotPasswordPage;
