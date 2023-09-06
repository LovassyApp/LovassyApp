import { Box, Center, Stack, Text, Title, createStyles, rem } from "@mantine/core";

import { IconBarrierBlock } from "@tabler/icons-react";
import { useGetApiProducts } from "../../../api/generated/features/products/products";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

const HomePage = (): JSX.Element => {
    const { classes } = useStyles();

    return (
        <Center className={classes.center}>
            <Stack spacing={0}>
                <Center>
                    <IconBarrierBlock size={rem(124)} />
                </Center>
                <Title align="center" px="sm">
                    Kezdőlap építés alatt
                </Title>
                <Text color="dimmed" align="center" px="sm">
                    Ez az oldal jelenleg még építés alatt áll... Amíg vársz nézd meg addig mi mindent tud a többi oldal!
                </Text>
            </Stack>
        </Center>
    );
};

export default HomePage;
