import { Box, Group, Paper, Stack, Text, rem, useMantineTheme } from "@mantine/core";

import { IconCoin } from "@tabler/icons-react";
import { ShopIndexOwnLolosResponseCoin } from "../../../api/generated/models";

export const CoinCard = ({ coin }: { coin: ShopIndexOwnLolosResponseCoin }): JSX.Element => {
    const theme = useMantineTheme();

    return (
        <Paper withBorder={true} radius="md" p="xs">
            <Group position="apart" maw="100%" sx={{ flexWrap: "nowrap" }}>
                <Stack justify="space-between" align="stretch" spacing={0} sx={{ flex: 1, overflow: "hidden" }}>
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {coin.reason}
                        </Text>
                    </Box>
                    <Text size="sm" color="dimmed">
                        {new Date(coin.createdAt).toLocaleDateString("hu-HU", {})}
                    </Text>
                </Stack>
                <IconCoin
                    stroke={1.5}
                    size={48}
                    color={
                        coin.isSpent
                            ? theme.colorScheme === "dark"
                                ? theme.colors.gray[7]
                                : theme.colors.gray[5]
                            : theme.fn.primaryColor()
                    }
                />
            </Group>
        </Paper>
    );
};
