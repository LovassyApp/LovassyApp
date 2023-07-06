import { Box, Button, Center, Checkbox, Group, PasswordInput, TextInput, Title, createStyles } from "@mantine/core";
import { IconLock, IconMail } from "@tabler/icons-react";

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
        } catch (err) {
            console.log(err);
        }
    });

    return (
        <Center className={classes.center}>
            <Box className={classes.content}>
                <Title align="center">Bejelentkezés</Title>
                <form onSubmit={submit}>
                    <TextInput
                        label="Email"
                        variant="filled"
                        icon={<IconMail size={20} />}
                        required={true}
                        mb="sm"
                        {...form.getInputProps("email")}
                    />
                    <PasswordInput
                        label="Jelszó"
                        variant="filled"
                        icon={<IconLock size={20} />}
                        required={true}
                        mb="lg"
                        {...form.getInputProps("password")}
                    />
                    <Group position="apart" align="flex-start">
                        <Checkbox
                            label="Nefelejts pipa"
                            {...form.getInputProps("remember", { type: "checkbox" })}
                        />
                        <Button type="submit">Bejelentkezés</Button>
                    </Group>
                </form>
            </Box>
        </Center>
    );
};

export default LoginPage;
