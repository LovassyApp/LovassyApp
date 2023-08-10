import {
    ActionIcon,
    Badge,
    Box,
    Button,
    Center,
    Drawer,
    Group,
    Loader,
    SimpleGrid,
    Switch,
    Text,
    TextInput,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconFilter, IconSearch, IconX } from "@tabler/icons-react";
import { useMemo, useState } from "react";

import { ShopIndexProductsResponse } from "../../../api/generated/models";
import { StoreProductCard } from "../components/storeProductCard";
import { useDisclosure } from "@mantine/hooks";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useGetApiLolosOwn } from "../../../api/generated/features/lolos/lolos";
import { useGetApiProducts } from "../../../api/generated/features/products/products";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

interface StoreParams {
    Search?: string;
    Filters?: string;
}

const FiltersDrawer = ({
    opened,
    close,
    params,
    setParams,
    balance,
}: {
    opened: boolean;
    close(): void;
    params: StoreParams;
    setParams: (arg0: StoreParams) => void;
    balance?: number;
}): JSX.Element => {
    const [search, setSearch] = useState<string>(params.Search ?? "");
    const [buyable, setBuyable] = useState<boolean>(params.Filters?.includes("Quantity>0") ?? false);

    const doSetParams = () => {
        const filters = [];

        if (buyable) {
            filters.push("Quantity>0");
            if (balance) filters.push(`Price<${balance}`);
        }

        setParams({ Search: search, Filters: filters.join(",") });
        close();
    };

    return (
        <Drawer opened={opened} onClose={close} title="Szűrés" position="right">
            <TextInput
                label="Keresés"
                value={search}
                icon={<IconSearch size={20} stroke={1.5} />}
                rightSection={
                    search !== "" && (
                        <ActionIcon variant="transparent" color="dark" onClick={() => setSearch("")}>
                            <IconX size={20} stroke={1.5} />
                        </ActionIcon>
                    )
                }
                onChange={(event) => setSearch(event.currentTarget.value)}
            />
            <Group position="apart" align="center" mt="sm">
                <Text size="sm">Csak megvásárolható</Text>
                <Switch checked={buyable} onChange={() => setBuyable(!buyable)} />
            </Group>
            <Button onClick={() => doSetParams()} fullWidth variant="outline" mt="md">
                Gyerünk!
            </Button>
        </Drawer>
    );
};

const ShopPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const [params, setParams] = useState<StoreParams>({});

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const userQueryEnabled = useMemo(
        () => control.data?.permissions?.includes("Shop.IndexOwnLolos") ?? false,
        [control]
    );

    const lolos = useGetApiLolosOwn({}, { query: { enabled: userQueryEnabled } });

    const storeProducts = useGetApiProducts({
        Search: params.Search ?? "",
        Filters: params.Filters
            ? params.Filters + (control.data?.permissions?.includes("Shop.IndexProducts") ? ",Visible==true" : "")
            : control.data?.permissions?.includes("Shop.IndexProducts")
            ? "Visible==true"
            : "",
    }); //Big mess, basically if the user has the permission to see invisible products, we don't show them

    const [filtersDraweOpened, { open: openFiltersDrawer, close: closeFiltersDrawer }] = useDisclosure(false);
    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalStoreProduct, setDetailsModalStoreProduct] = useState<ShopIndexProductsResponse>();

    if (storeProducts.isLoading || lolos.isInitialLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (storeProducts.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <FiltersDrawer
                opened={filtersDraweOpened}
                close={closeFiltersDrawer}
                params={params}
                setParams={setParams}
                balance={lolos.data?.balance}
            />
            <Group position="apart" align="baseline" mb="md" spacing={0}>
                <Title>Bazár</Title>
                <Group align="baseline">
                    {lolos.data?.balance && (
                        <Badge
                            color={lolos.data.balance > 0 ? "green" : "red"}
                            variant="light"
                            sx={{ alignSelf: "center" }}
                        >
                            {lolos.data.balance} loló
                        </Badge>
                    )}
                    <ActionIcon variant="transparent" color="dark" onClick={() => openFiltersDrawer()}>
                        <IconFilter />
                    </ActionIcon>
                </Group>
            </Group>
            <SimpleGrid
                cols={4}
                breakpoints={[
                    { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                    { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                    { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                ]}
            >
                {storeProducts.data?.map((product) => (
                    <StoreProductCard
                        key={product.id}
                        storeProduct={product}
                        openDetails={(storeProducts) => {
                            setDetailsModalStoreProduct(storeProducts);
                            openDetailsModal();
                        }}
                    />
                ))}
            </SimpleGrid>
        </>
    );
};

export default ShopPage;
