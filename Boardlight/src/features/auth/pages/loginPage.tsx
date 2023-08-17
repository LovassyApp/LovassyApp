import {
    Anchor,
    Box,
    Button,
    Center,
    Checkbox,
    Group,
    PasswordInput,
    Text,
    TextInput,
    Title,
    createStyles,
} from "@mantine/core";
import { IconLock, IconMail } from "@tabler/icons-react";
import { Link, useNavigate } from "react-router-dom";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";

import { useAuthStore } from "../../../core/stores/authStore";
import { useForm } from "@mantine/form";
import { usePostApiAuthLogin } from "../../../api/generated/features/auth/auth";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
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

const LoginPage = (): JSX.Element => {
    const { classes } = useStyles();

    const setAccessToken = useAuthStore((state) => state.setAccessToken);

    const login = usePostApiAuthLogin();

    const navigate = useNavigate();

    const form = useForm({
        initialValues: {
            email: "",
            password: "",
            remember: false,
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            const res = await login.mutateAsync({ data: values });
            setAccessToken(res.token);
            navigate("/");
        } catch (err) {
            if (err instanceof ValidationError) handleValidationErrors(err, form);
        }
    });

    return (
        <Center className={classes.center}>
            <Box className={classes.content}>
                <Title align="center" mb="sm">
                    Bejelentkezés
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
                    <PasswordInput
                        label="Jelszó"
                        icon={<IconLock size={20} stroke={1.5} />}
                        required={true}
                        mb="lg"
                        {...form.getInputProps("password")}
                    />
                    <Group position="apart" align="flex-start" mb="md">
                        <Checkbox label="Nefelejts pipa" {...form.getInputProps("remember", { type: "checkbox" })} />
                        <Button type="submit">Bejelentkezés</Button>
                    </Group>
                </form>
                <Text align="center" size="sm">
                    Elfelejtetted a jelszavad?{" "}
                    <Anchor component={Link} to="/auth/forgot-password">
                        Állítsd vissza itt
                    </Anchor>
                </Text>
                <Text align="center" size="sm">
                    Még nincs fiókod?{" "}
                    <Anchor component={Link} to="/auth/register">
                        Regisztrálj itt
                    </Anchor>
                </Text>
            </Box>
        </Center>
    );
};

export default LoginPage;
