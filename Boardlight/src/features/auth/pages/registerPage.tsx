import {
    Anchor,
    Box,
    Button,
    Center,
    LoadingOverlay,
    PasswordInput,
    Text,
    TextInput,
    Title,
    createStyles,
} from "@mantine/core";
import { Icon123, IconLock, IconMail, IconUser } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";

import { Link } from "react-router-dom";
import { UnavailableModalContent } from "../components/unavailableModalContent";
import { modals } from "@mantine/modals";
import { useAuthStore } from "../../../core/stores/authStore";
import { useEffect } from "react";
import { useForm } from "@mantine/form";
import { useGetApiStatusServiceStatus } from "../../../api/generated/features/status/status";
import { usePostApiAuthLogin } from "../../../api/generated/features/auth/auth";
import { usePostApiUsers } from "../../../api/generated/features/users/users";

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

const RegisterPage = (): JSX.Element => {
    const { classes } = useStyles();

    const status = useGetApiStatusServiceStatus();
    const createUser = usePostApiUsers();
    const login = usePostApiAuthLogin();

    const setAccessToken = useAuthStore((state) => state.setAccessToken);

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
            name: "",
            omCode: "",
            password: "",
            confirmPassword: "",
        },
        validate: {
            confirmPassword: (value, values) => {
                if (value !== values.password) {
                    return "A két jelszó nem egyezik";
                }
            },
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await createUser.mutateAsync({
                data: values,
                params: {
                    verifyUrl: `${window.location.origin}/auth/verify-email`,
                    verifyTokenQueryKey: "verifyToken",
                },
            });
            const res = await login.mutateAsync({
                data: { email: values.email, password: values.password, remember: true },
            });
            setAccessToken(res.token);
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
                        Regisztráció
                    </Title>
                    <form onSubmit={submit}>
                        <TextInput
                            label="Email"
                            type="email"
                            icon={<IconMail size={20} stroke={1.5} />}
                            required={true}
                            mb="sm"
                            {...form.getInputProps("email")}
                        />
                        <TextInput
                            label="Név"
                            icon={<IconUser size={20} stroke={1.5} />}
                            required={true}
                            mb="sm"
                            {...form.getInputProps("name")}
                        />
                        <TextInput
                            label="OM Azonosító"
                            icon={<Icon123 size={20} stroke={1.5} />}
                            required={true}
                            mb="sm"
                            {...form.getInputProps("omCode")}
                        />
                        <PasswordInput
                            label="Jelszó"
                            icon={<IconLock size={20} stroke={1.5} />}
                            required={true}
                            mb="sm"
                            {...form.getInputProps("password")}
                        />
                        <PasswordInput
                            label="Jelszó megerősítése"
                            icon={<IconLock size={20} stroke={1.5} />}
                            required={true}
                            mb="md"
                            {...form.getInputProps("confirmPassword")}
                        />
                        <Button
                            type="submit"
                            fullWidth={true}
                            mb="md"
                            loading={createUser.isLoading || login.isLoading}
                        >
                            Regisztrálás
                        </Button>
                    </form>
                    <Text align="center" size="sm">
                        Már van fiókod?{" "}
                        <Anchor component={Link} to="/auth/login">
                            Lépj be itt
                        </Anchor>
                    </Text>
                </Box>
            </Box>
        </Center>
    );
};

export default RegisterPage;
