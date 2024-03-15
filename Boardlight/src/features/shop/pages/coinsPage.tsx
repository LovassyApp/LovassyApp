import {
    Center,
    Divider,
    Group,
    Loader,
    Modal,
    SimpleGrid,
    Text,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { useMemo, useState } from "react";

import { CoinCard } from "../components/coinCard";
import { CoinsStats } from "../components/coinStats";
import { ShopIndexLolosResponse } from "../../../api/generated/models";
import { useDisclosure } from "@mantine/hooks";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useGetApiLolos } from "../../../api/generated/features/lolos/lolos";
import { useGetApiUsersId } from "../../../api/generated/features/users/users";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

const DetailsModal = ({
    coin,
    opened,
    close,
}: {
    coin: ShopIndexLolosResponse;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const userQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Users.ViewUser") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const user = useGetApiUsersId(coin?.userId, { query: { enabled: userQueryEnabled && !!coin } });

    if (user.isInitialLoading)
        return (
            <Modal opened={opened} onClose={close} title="Részletek" size="lg">
                <Center>
                    <Loader />
                </Center>
            </Modal>
        );

    return (
        <Modal opened={opened} onClose={close} title="Részletek" size="lg">
            <Group position="apart" spacing={0}>
                <Text>Indoklás:</Text>
                <Text weight="bold">{coin?.reason}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Elköltve:</Text>
                <Text weight="bold">{coin?.isSpent ? "Igen" : "Nem"}</Text>
            </Group>
            {user.data && (
                <>
                    <Divider my="sm" />
                    <Group position="apart" spacing={0}>
                        <Text>Felhasználó neve:</Text>
                        <Text weight="bold">{user.data?.name}</Text>
                    </Group>
                    <Group position="apart" spacing={0}>
                        <Text>Felhasználó valódi neve:</Text>
                        <Text weight="bold">{user.data?.realName ?? "Ismeretlen"}</Text>
                    </Group>
                    <Group position="apart" spacing={0}>
                        <Text>Felhasználó osztálya:</Text>
                        <Text weight="bold">{user.data?.class ?? "Ismeretlen"}</Text>
                    </Group>
                </>
            )}
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Létrehozva:</Text>
                <Text weight="bold">{new Date(coin?.createdAt).toLocaleString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Módosítva:</Text>
                <Text weight="bold">{new Date(coin?.updatedAt).toLocaleString("hu-HU", {})}</Text>
            </Group>
        </Modal>
    );
};

const CoinsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const coins = useGetApiLolos({ Sorts: "isSpent" });

    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalCoin, setDetailsModalCoin] = useState<ShopIndexLolosResponse>();

    if (coins.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (coins.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <DetailsModal coin={detailsModalCoin} opened={detailsModalOpened} close={closeDetailsModal} />
            <Title mb="md">Összevont statisztikák</Title>
            <SimpleGrid cols={2} breakpoints={[{ maxWidth: theme.breakpoints.sm, cols: 1, spacing: "sm" }]}>
                <CoinsStats data={coins.data} />
            </SimpleGrid>
            <Title my="md">Összes érme</Title>
            <SimpleGrid
                cols={4}
                breakpoints={[
                    { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                    { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                    { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                ]}
            >
                {coins.data?.map((coin) => (
                    <CoinCard
                        key={coin.id}
                        coin={coin}
                        openDetails={() => {
                            setDetailsModalCoin(coin);
                            openDetailsModal();
                        }}
                    />
                ))}
            </SimpleGrid>
            {coins.data.length === 0 && (
                <Text color="dimmed">
                    Úgy néz ki még senkinek nincsenek érméi... Amint valaki szerez egy loló érmét, itt látod majd őket.
                </Text>
            )}
        </>
    );
};

export default CoinsPage;
