import { Center, Stack, Title, createStyles } from "@mantine/core";

import { ColorSchemeToggle } from "../../components/colorSchemeToggle";

const useStyles = createStyles((theme) => ({}));

const GradeImportPage = (): JSX.Element => {
    return (
        <Center sx={{ height: "100%" }}>
            <Stack align="center">
                <Title>Index Page</Title>
                <ColorSchemeToggle />
            </Stack>
        </Center>
    );
};

export default GradeImportPage;
