import { Stack, Title, createStyles } from "@mantine/core";

import { ResetKeyPasswordInput } from "../../components/resetKeyPasswordInput";

const useStyles = createStyles((theme) => ({}));

const ResetKeyPasswordPage = (): JSX.Element => {
    const { classes } = useStyles();

    return (
        <Stack spacing="xs">
            <Title order={2} size="h1">Biztonság</Title>
            <ResetKeyPasswordInput />
        </Stack>
    );
};

export default ResetKeyPasswordPage;
