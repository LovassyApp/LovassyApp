import { Box, Button, Card, Group, Image, Text, createStyles, rem, useMantineTheme } from "@mantine/core";
import { IconQrcode, IconQrcodeOff } from "@tabler/icons-react";

import { ShopIndexProductsResponse } from "../../../api/generated/models";

const useStyles = createStyles((theme) => ({
    section: {
        padding: theme.spacing.md,
        borderTop: `${rem(1)} solid ${theme.colorScheme === "dark" ? theme.colors.dark[4] : theme.colors.gray[3]}`,
    },
}));

export const StoreProductCard = ({
    storeProduct,
    openDetails,
}: {
    storeProduct: ShopIndexProductsResponse;
    openDetails(ShopIndexProductsResponse): void;
}): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    return (
        <Card radius="md" withBorder>
            <Card.Section>
                <Image src={storeProduct.thumbnailUrl} alt="Tesla Model S" height={200} />
            </Card.Section>
            <Card.Section className={classes.section}>
                <Text size="lg" truncate weight={500}>
                    {storeProduct.name}
                </Text>
                <Text size="sm" color="dimmed" lineClamp={2}>
                    {storeProduct.description}
                </Text>
            </Card.Section>
            <Card.Section className={classes.section} mt={0}>
                <Group spacing="xs" sx={{ overflow: "hidden" }}>
                    {storeProduct.qrCodeActivated ? (
                        <IconQrcode stroke={1.5} size={20} />
                    ) : (
                        <IconQrcodeOff stroke={1.5} size={20} />
                    )}
                    <Text size="sm" truncate>
                        {storeProduct.qrCodeActivated ? "QR kód aktivált" : "Nem QR kód aktivált"}
                    </Text>
                </Group>
            </Card.Section>
            <Card.Section className={classes.section}>
                <Group position="apart" sx={{ flexWrap: "nowrap" }}>
                    <Box maw="100%" sx={{ overflow: "hidden" }}>
                        <Text size="lg" weight={500} truncate={true}>
                            {storeProduct.price} lóló
                        </Text>
                    </Box>
                    <Text size="sm" color="dimmed">
                        {storeProduct.quantity} db
                    </Text>
                </Group>
                <Button fullWidth color="blue" mt="sm" onClick={() => openDetails(storeProduct)}>
                    {"Részletek"}
                </Button>
            </Card.Section>
        </Card>
    );
};
