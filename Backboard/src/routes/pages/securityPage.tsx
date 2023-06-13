import { Stack, Title } from "@mantine/core";

import { ResetKeyPasswordInput } from "../../components/resetKeyPasswordInput";

const SecurityPage = (): JSX.Element => {
    return (
        <Stack spacing="xs">
            <Title order={2} size="h1">Biztonság</Title>
            <ResetKeyPasswordInput />
        </Stack>
    );
};

export default SecurityPage;
