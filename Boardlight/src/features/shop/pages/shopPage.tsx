import {
    ActionIcon,
    Badge,
    Button,
    Center,
    Divider,
    Drawer,
    Group,
    Loader,
    Modal,
    SimpleGrid,
    Switch,
    Text,
    TextInput,
    Title,
    TypographyStylesProvider,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconFilter, IconSearch, IconX } from "@tabler/icons-react";
import {
    useGetApiProducts,
    useGetApiProductsId,
    usePostApiProductsBuyId,
} from "../../../api/generated/features/products/products";
import { useMemo, useState } from "react";

import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { ShopIndexProductsResponse } from "../../../api/generated/models";
import { StoreProductCard } from "../components/storeProductCard";
import { ValidationError } from "../../../helpers/apiHelpers";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useGetApiLolosOwn } from "../../../api/generated/features/lolos/lolos";

const useStyles = createStyles(() => ({
    center: {
        height: "100%",
    },
}));

interface ShopParams {
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
    params: ShopParams;
    setParams(arg0: ShopParams): void;
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
            <Button onClick={() => doSetParams()} fullWidth={true} variant="outline" mt="md">
                Gyerünk!
            </Button>
        </Drawer>
    );
};

const DetailsModal = ({
    storeProduct,
    balance,
    opened,
    close,
}: {
    storeProduct: ShopIndexProductsResponse;
    balance?: number;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already
    const detailedQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Shop.ViewStoreProduct") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const storeProductDetailed = useGetApiProductsId(storeProduct?.id, {
        query: { enabled: detailedQueryEnabled && !!storeProduct },
    });

    const buyProduct = usePostApiProductsBuyId();

    const doBuyProduct = async () => {
        try {
            await buyProduct.mutateAsync({ id: storeProduct?.id });
            // We don't need to invalidate the products query, because the realtime notification will do it for us
            notifications.show({
                title: "Termék megvásárolva",
                message: "A terméket sikeresen megvásároltad.",
                color: "green",
                icon: <IconCheck />,
            });
            close();
        } catch (err) {
            if (err instanceof ValidationError) {
                notifications.show({
                    title: "Hiba (400)",
                    message: err.message,
                    color: "red",
                    icon: <IconX />,
                });
            }
        }
    };

    if (storeProductDetailed.isInitialLoading)
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
                <Text>Név:</Text>
                <Text weight="bold">{storeProduct?.name}</Text>
            </Group>
            <Text>Leírás:</Text>
            <Text weight="bold">{storeProduct?.description}</Text>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Ár:</Text>
                <Text weight="bold">{storeProduct?.price} loló</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Mennyiség:</Text>
                <Text weight="bold">{storeProduct?.quantity} db</Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>QR kód aktivált:</Text>
                <Text weight="bold">{storeProduct?.qrCodeActivated ? "Igen" : "Nem"}</Text>
            </Group>
            {storeProductDetailed.data?.inputs.length > 0 && (
                <>
                    <Group position="apart" spacing={0}>
                        <Text>Inputok:</Text>
                        <Text weight="bold">
                            {storeProductDetailed.data.inputs.map((input) => input.label).join(", ")}
                        </Text>
                    </Group>
                </>
            )}
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Létrehozás dátuma:</Text>
                <Text weight="bold">{new Date(storeProduct?.createdAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Módosítás dátuma:</Text>
                <Text weight="bold">{new Date(storeProduct?.updatedAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            {storeProductDetailed.data && (
                <>
                    <Divider my="sm" />
                    <TypographyStylesProvider>
                        <div dangerouslySetInnerHTML={{ __html: storeProductDetailed.data?.richTextContent ?? "" }} />
                    </TypographyStylesProvider>
                </>
            )}
            <Divider my="sm" />
            <PermissionRequirement permissions={["Shop.BuyProduct"]}>
                <Button
                    fullWidth={true}
                    loading={buyProduct.isLoading}
                    onClick={() => doBuyProduct()}
                    disabled={storeProduct?.quantity < 1 || (balance && storeProduct?.price > balance)}
                >
                    Megveszem!
                </Button>
            </PermissionRequirement>
        </Modal>
    );
};

const ShopPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const [params, setParams] = useState<ShopParams>({});

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const userQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Shop.IndexOwnLolos") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const lolos = useGetApiLolosOwn({}, { query: { enabled: userQueryEnabled } });

    const storeProducts = useGetApiProducts({
        Search: params.Search ?? "",
        Filters: params.Filters
            ? params.Filters +
              (control.data?.permissions?.includes("Shop.IndexProducts") || control.data?.isSuperUser
                  ? ",Visible==true"
                  : "")
            : control.data?.permissions?.includes("Shop.IndexProducts") || control.data?.isSuperUser
            ? "Visible==true"
            : "",
        Sorts: "Name",
    }); // Big mess, basically if the user has the permission to see invisible products, we don't show them

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
            <DetailsModal
                storeProduct={detailsModalStoreProduct}
                balance={lolos.data?.balance}
                opened={detailsModalOpened}
                close={closeDetailsModal}
            />
            <Group position="apart" align="baseline" mb="md" spacing={0}>
                <Title>Bazár</Title>
                <Group align="baseline">
                    {lolos.data?.balance !== undefined && (
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
                        openDetails={() => {
                            setDetailsModalStoreProduct(product);
                            openDetailsModal();
                        }}
                    />
                ))}
            </SimpleGrid>
        </>
    );
};

export default ShopPage;
