import { Box, Text, createStyles } from "@mantine/core";

import { useGetApiProducts } from "../../../api/generated/features/products/products";

const useStyles = createStyles((theme) => ({}));

const HomePage = (): JSX.Element => {
    const { classes } = useStyles();
    const produts = useGetApiProducts();

    return <Text>Test</Text>;
};

export default HomePage;
