import { Box, Button, Center, LoadingOverlay, PasswordInput, Title, createStyles } from "@mantine/core";
import { IconCheck, IconLock } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import { useNavigate, useSearchParams } from "react-router-dom";

import { UnavailableModal } from "../components/unavailableModal";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useEffect } from "react";
import { useForm } from "@mantine/form";
import { useGetApiStatusServiceStatus } from "../../../api/generated/features/status/status";
import { usePostApiAuthResetPassword } from "../../../api/generated/features/auth/auth";

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

const ResetPasswordPage = (): JSX.Element => {
    const { classes } = useStyles();

    const [queryParams] = useSearchParams();

    const status = useGetApiStatusServiceStatus();
    const resetPassword = usePostApiAuthResetPassword();

    const navigate = useNavigate();

    const [unavaliableModalOpened, { open: openUnavailableModal }] = useDisclosure();

    useEffect(() => {
        if (status.isSuccess && !status.data?.serviceStatus.resetKeyPassword) {
            openUnavailableModal();
        }
    }, [status]);

    const form = useForm({
        initialValues: {
            newPassword: "",
            confirmNewPassword: "",
        },
        validate: {
            confirmNewPassword: (value, values) => {
                if (value !== values.newPassword) {
                    return "A két jelszó nem egyezik meg";
                }
            },
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await resetPassword.mutateAsync({
                data: values,
                params: { passwordResetToken: queryParams.get("resetToken") },
            });
            notifications.show({
                title: "Sikeres jelszóváltoztatás",
                message:
                    "A jelszavad sikeresen megváltoztatásra került. Mostantól ezzel a jelszóval tudsz bejelentkezni.",
                color: "green",
                icon: <IconCheck />,
            });
            navigate("/auth/login", { replace: true });
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
                            Új jelszó beállítása
                        </Title>
                        <form onSubmit={submit}>
                            <PasswordInput
                                label="Jelszó"
                                icon={<IconLock size={20} stroke={1.5} />}
                                mb="sm"
                                {...form.getInputProps("newPassword")}
                            />
                            <PasswordInput
                                label="Jelszó megerősítése"
                                icon={<IconLock size={20} stroke={1.5} />}
                                mb="md"
                                {...form.getInputProps("confirmNewPassword")}
                            />
                            <Button type="submit" fullWidth={true} loading={resetPassword.isLoading}>
                                Új jelszó beállítása
                            </Button>
                        </form>
                    </Box>
                </Box>
            </Center>
        </>
    );
};

export default ResetPasswordPage;
