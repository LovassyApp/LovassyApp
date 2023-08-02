import { Box, Group, Paper, Stack, Text, createStyles, rem, useMantineTheme } from "@mantine/core";

import { IconQrcode } from "@tabler/icons-react";
import { ShopIndexQRCodesResponse } from "../../../api/generated/models";

const useStyles = createStyles((theme) => ({
    card: {
        cursor: "pointer",
        overflow: "hidden",
    },
}));

export const QRCodeCard = ({
    qrCode,
    openDetails,
}: {
    qrCode: ShopIndexQRCodesResponse;
    openDetails(ShopIndexQRCodesResponse): void;
}): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    return (
        <Paper withBorder={true} radius="md" p="xs" className={classes.card} onClick={() => openDetails(qrCode)}>
            <Group position="apart" maw="100%" sx={{ flexWrap: "nowrap" }}>
                <Stack justify="space-between" align="stretch" spacing={0} sx={{ flex: 1, overflow: "hidden" }}>
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {qrCode.name}
                        </Text>
                    </Box>
                    <Text size="sm" color="dimmed">
                        {qrCode.email}
                    </Text>
                </Stack>
                <IconQrcode stroke={1.5} size={48} color={theme.fn.primaryColor()} />
            </Group>
        </Paper>
    );
};
