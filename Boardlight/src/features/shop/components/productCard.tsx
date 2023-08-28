import { Box, Group, Image, Paper, Stack, Text, createStyles, rem } from "@mantine/core";
import { IconEyeOff, IconForms, IconQrcode, IconQrcodeOff } from "@tabler/icons-react";

import { ShopIndexProductsResponse } from "../../../api/generated/models";

const useStyles = createStyles((theme) => ({
    card: {
        cursor: "pointer",
        overflow: "hidden",
    },
    qrIcon: {
        position: "absolute",
        bottom: rem(4),
        left: rem(4),
        color: theme.white,
        zIndex: 2,
    },
    eyeIcon: {
        position: "absolute",
        bottom: rem(4),
        right: rem(4),
        color: theme.white,
        zIndex: 2,
    },
    overlayContainer: {
        ["&:after"]: {
            content: '""',
            position: "absolute",
            display: "block",
            left: 0,
            top: 0,
            width: "100%",
            height: "100%",
            background:
                "rgba(0, 0, 0, 0) linear-gradient(to bottom, rgba(0, 0, 0, 0) 0px, rgba(0, 0, 0, 0.6) 100%) repeat 0 0",
            zIndex: 1,
        },
    },
}));

export const ProductCard = ({
    product,
    openDetails,
}: {
    product: ShopIndexProductsResponse;
    openDetails(): void;
}): JSX.Element => {
    const { classes } = useStyles();

    return (
        <Paper withBorder={true} radius="md" className={classes.card} onClick={() => openDetails()}>
            <Group noWrap={true} spacing={0}>
                <Box pos="relative" className={classes.overlayContainer}>
                    <Image src={product.thumbnailUrl} height={94} width={94} />
                    {product.qrCodeActivated ? (
                        <IconQrcode stroke={1.5} size={24} className={classes.qrIcon} />
                    ) : (
                        <IconQrcodeOff stroke={1.5} size={24} className={classes.qrIcon} />
                    )}
                    {!product.visible && <IconEyeOff stroke={1.5} size={24} className={classes.eyeIcon} />}
                </Box>
                <Stack justify="space-between" align="stretch" spacing={0} sx={{ flex: 1, overflow: "hidden" }} p="xs">
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {product.name}
                        </Text>
                    </Box>
                    <Box maw="100%" mb={rem(2)}>
                        <Text size="sm" color="dimmed" truncate={true}>
                            {product.description}
                        </Text>
                    </Box>
                    <Group position="apart" noWrap={true}>
                        <Text size="sm">{product.price} loló</Text>
                        <Text size="sm" color={product.quantity > 0 ? "green" : "red"}>
                            {product.quantity} db
                        </Text>
                    </Group>
                </Stack>
            </Group>
        </Paper>
    );
};
