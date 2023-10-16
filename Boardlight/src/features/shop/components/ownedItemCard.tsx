import { Box, Group, Image, Paper, Stack, Text, createStyles, rem } from "@mantine/core";
import { IconForms, IconQrcode, IconQrcodeOff } from "@tabler/icons-react";

import { ShopIndexOwnOwnedItemsResponse } from "../../../api/generated/models";

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
    inputsIcon: {
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

export const OwnedItemCard = ({
    ownedItem,
    openDetails,
}: {
    ownedItem: ShopIndexOwnOwnedItemsResponse;
    openDetails(): void;
}): JSX.Element => {
    const { classes } = useStyles();

    return (
        <Paper withBorder={true} radius="md" className={classes.card} onClick={() => openDetails()}>
            <Group noWrap={true} spacing={0}>
                <Box pos="relative" className={classes.overlayContainer}>
                    <Image src={ownedItem.product.thumbnailUrl} height={94} width={94} />
                    {ownedItem.product.qrCodeActivated ? (
                        <IconQrcode stroke={1.5} size={24} className={classes.qrIcon} />
                    ) : (
                        <IconQrcodeOff stroke={1.5} size={24} className={classes.qrIcon} />
                    )}
                    {ownedItem.product.inputs.length > 0 && (
                        <IconForms stroke={1.5} size={24} className={classes.inputsIcon} />
                    )}
                </Box>
                <Stack justify="space-between" align="stretch" spacing={0} sx={{ flex: 1, overflow: "hidden" }} p="xs">
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {ownedItem.product.name}
                        </Text>
                    </Box>
                    <Box maw="100%" mb={rem(2)}>
                        <Text size="sm" color="dimmed" truncate={true}>
                            {ownedItem.product.description}
                        </Text>
                    </Box>
                    <Box maw="100%">
                        <Text size="sm" color={ownedItem.usedAt ? "red" : "green"} truncate={true}>
                            {ownedItem.usedAt
                                ? `Felhasználva: ${new Date(ownedItem.usedAt).toLocaleDateString("hu-HU", {})}`
                                : "Felhasználható"}
                        </Text>
                    </Box>
                </Stack>
            </Group>
        </Paper>
    );
};
