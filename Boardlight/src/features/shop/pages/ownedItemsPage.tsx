import {
    ActionIcon,
    Button,
    Center,
    Divider,
    Drawer,
    Group,
    Loader,
    Modal,
    Select,
    SimpleGrid,
    Text,
    TextInput,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconFilter, IconPlus, IconSearch, IconX } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    useDeleteApiOwnedItemsId,
    useGetApiOwnedItems,
    usePatchApiOwnedItemsId,
    usePostApiOwnedItems,
} from "../../../api/generated/features/owned-items/owned-items";
import { useEffect, useMemo, useState } from "react";
import { useGetApiUsers, useGetApiUsersId } from "../../../api/generated/features/users/users";

import { DateInput } from "@mantine/dates";
import { OwnedItemCard } from "../components/ownedItemCard";
import { OwnedItemsStats } from "../components/ownedItemsStats";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { ShopIndexOwnedItemsResponse } from "../../../api/generated/models";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useGetApiProducts } from "../../../api/generated/features/products/products";

const useStyles = createStyles(() => ({
    center: {
        height: "100%",
    },
}));

interface OwnedItemsParams {
    Search?: string;
    Filters?: string;
}

const FiltersDrawer = ({
    opened,
    close,
    params,
    setParams,
}: {
    opened: boolean;
    close(): void;
    params: OwnedItemsParams;
    setParams(arg0: OwnedItemsParams): void;
}): JSX.Element => {
    const [search, setSearch] = useState<string>(params.Search ?? "");
    // const [unused, setUnused] = useState<boolean>(params.Filters?.includes("UsedAt==null") ?? false);

    const doSetParams = () => {
        const filters = [];

        // if (unused) filters.push("UsedAt==null");

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
            {/* <Group position="apart" align="center" mt="sm">
                <Text size="sm">Csak fel nem használt</Text>
                <Switch checked={unused} onChange={() => setUnused(!unused)} />
            </Group> */}
            <Button onClick={() => doSetParams()} fullWidth={true} variant="outline" mt="md">
                Gyerünk!
            </Button>
        </Drawer>
    );
};

const CreateOwnedItemModal = ({ opened, close }: { opened: boolean; close(): void }): JSX.Element => {
    const createOwnedItem = usePostApiOwnedItems();

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const usersQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Users.IndexUsers") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const productsQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Shop.IndexProducts") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const users = useGetApiUsers({}, { query: { enabled: usersQueryEnabled } });
    const products = useGetApiProducts({}, { query: { enabled: productsQueryEnabled } });

    const form = useForm({
        initialValues: {
            userId: undefined,
            productId: undefined,
            usedAt: undefined,
        },
        transformValues: (values) => ({
            ...values,
            usedAt: values.usedAt?.toISOString(),
        }),
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await createOwnedItem.mutateAsync({ data: values });
            notifications.show({
                title: "Termék létrehozva",
                message: "A terméket sikeresen létrehoztad a kincstárban.",
                color: "green",
                icon: <IconCheck />,
            });
            close();
        } catch (err) {
            if (err instanceof ValidationError) {
                handleValidationErrors(err, form);
            }
        }
    });

    const usersData = useMemo(
        () =>
            users.data?.map((user) => ({
                value: user.id,
                label: `${user.name} (${user.realName ?? "Ismeretlen"} - ${user.class ?? "Ismeretelen"})`,
            })) ?? [],
        [users]
    );

    const productsData = useMemo(
        () =>
            products.data?.map((product) => ({
                value: product.id.toString(),
                label: `${product.name} (${product.description.substring(
                    0,
                    Math.min(product.description.length, 50)
                )}...)`,
            })) ?? [],
        [products]
    );

    if (users.isInitialLoading || products.isInitialLoading)
        return (
            <Modal opened={opened} onClose={close} title="Új kincstári termék" size="lg">
                <Center>
                    <Loader />
                </Center>
            </Modal>
        );

    return (
        <Modal opened={opened} onClose={close} title="Új kincstári termék" size="lg">
            <form onSubmit={submit}>
                {usersData.length > 0 ? (
                    <Select
                        label="Felhasználó"
                        withinPortal={true}
                        required={true}
                        data={usersData}
                        onChange={(value) => form.setFieldValue("userId", value)}
                        searchable={true}
                        {...form.getInputProps("userId")}
                    />
                ) : (
                    <TextInput label="Felhasználó id" required={true} {...form.getInputProps("userId")} />
                )}
                {productsData.length > 0 ? (
                    <Select
                        label="Termék"
                        withinPortal={true}
                        required={true}
                        data={productsData}
                        onChange={(value) => form.setFieldValue("productId", value)}
                        searchable={true}
                        {...form.getInputProps("productId")}
                        mt="sm"
                    />
                ) : (
                    <TextInput label="Termék id" required={true} {...form.getInputProps("productId")} mt="sm" />
                )}
                <DateInput
                    popoverProps={{ withinPortal: true }}
                    label="Felhasználás dátuma"
                    clearable={true}
                    {...form.getInputProps("usedAt")}
                    mt="sm"
                />
                <Button type="submit" fullWidth={true} loading={createOwnedItem.isLoading} mt="md">
                    Létrehozás
                </Button>
            </form>
        </Modal>
    );
};

const DetailsModal = ({
    ownedItem,
    opened,
    close,
}: {
    ownedItem: ShopIndexOwnedItemsResponse;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const userQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Users.ViewUser") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const user = useGetApiUsersId(ownedItem?.userId, { query: { enabled: userQueryEnabled && !!ownedItem } });

    const updateOwnedItem = usePatchApiOwnedItemsId();
    const deleteOwnedItem = useDeleteApiOwnedItemsId();

    const form = useForm({
        initialValues: {
            usedAt: ownedItem?.usedAt ? new Date(ownedItem?.usedAt) : undefined,
        },
        transformValues: (values) => ({
            usedAt: values.usedAt?.toISOString(),
        }),
    });

    useEffect(() => {
        form.setValues({
            usedAt: ownedItem?.usedAt ? new Date(ownedItem?.usedAt) : undefined,
        });
    }, [ownedItem, opened]);

    const submit = form.onSubmit(async (values) => {
        try {
            await updateOwnedItem.mutateAsync({ id: ownedItem?.id, data: values });
            notifications.show({
                title: "Termék módosítva",
                message: "A terméket sikeresen módosítottad a kincstárban.",
                color: "green",
                icon: <IconCheck />,
            });
            close();
        } catch (err) {
            if (err instanceof ValidationError) {
                handleValidationErrors(err, form);
            }
        }
    });

    const doDeleteOwnedItem = async () => {
        try {
            await deleteOwnedItem.mutateAsync({ id: ownedItem?.id });
            notifications.show({
                title: "Termék törölve",
                message: "A terméket sikeresen törölted a kincstárból.",
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

    if (user.isInitialLoading)
        return (
            <Modal opened={opened} onClose={close} title="Részletek" size="lg">
                <Center>
                    <Loader />
                </Center>
            </Modal>
        );

    return (
        <Modal opened={opened} onClose={close} size="lg" title="Részletek">
            <Group position="apart" spacing={0}>
                <Text>Név:</Text>
                <Text weight="bold">{ownedItem?.product.name}</Text>
            </Group>
            <Text>Leírás:</Text>
            <Text weight="bold">{ownedItem?.product.description}</Text>
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
                <Text>QR kód aktivált:</Text>
                <Text weight="bold">{ownedItem?.product.qrCodeActivated ? "Igen" : "Nem"}</Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Állapot:</Text>
                <Text weight="bold">{ownedItem?.usedAt ? "Felhasználva" : "Nincs felhasználva"}</Text>
            </Group>
            {ownedItem?.usedAt && (
                <Group position="apart" spacing={0}>
                    <Text>Felhasználás dátuma:</Text>
                    <Text weight="bold">{new Date(ownedItem?.usedAt).toLocaleDateString("hu-HU", {})}</Text>
                </Group>
            )}
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Létrehozás dátuma:</Text>
                <Text weight="bold">{new Date(ownedItem?.createdAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Módosítás dátuma:</Text>
                <Text weight="bold">{new Date(ownedItem?.updatedAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            <PermissionRequirement permissions={["Shop.UpdateOwnedItem"]}>
                <Divider my="sm" />
                <form onSubmit={submit}>
                    <DateInput
                        popoverProps={{ withinPortal: true }}
                        label="Felhasználás dátuma"
                        clearable={true}
                        {...form.getInputProps("usedAt")}
                    />
                    <Button type="submit" fullWidth={true} mt="md" loading={updateOwnedItem.isLoading}>
                        Mentés
                    </Button>
                </form>
            </PermissionRequirement>
            <PermissionRequirement permissions={["Shop.DeleteOwnedItem"]}>
                <Divider my="sm" />
                <Button fullWidth={true} color="red" onClick={() => doDeleteOwnedItem()}>
                    Termék törlése
                </Button>
            </PermissionRequirement>
        </Modal>
    );
};

const OwnedItemsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const [params, setParams] = useState<OwnedItemsParams>({});

    const ownedItems = useGetApiOwnedItems({
        Search: params.Search ?? "",
        Filters: params.Filters ?? "",
        Sorts: "-CreatedAt",
    });

    const [filtersDrawerOpened, { open: openFiltersDrawer, close: closeFiltersDrawer }] = useDisclosure(false);
    const [createOwnedItemModalOpened, { close: closeCreateOwnedItemModal, open: openCreateOwnedItemModal }] =
        useDisclosure(false);
    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalOwnedItem, setDetailsModalOwnedItem] = useState<ShopIndexOwnedItemsResponse>();

    if (ownedItems.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (ownedItems.isError)
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
                opened={filtersDrawerOpened}
                close={closeFiltersDrawer}
                params={params}
                setParams={setParams}
            />
            <CreateOwnedItemModal opened={createOwnedItemModalOpened} close={closeCreateOwnedItemModal} />
            <DetailsModal ownedItem={detailsModalOwnedItem} opened={detailsModalOpened} close={closeDetailsModal} />
            <Title mb="md">Összevont statisztikák</Title>
            <SimpleGrid cols={2} breakpoints={[{ maxWidth: theme.breakpoints.sm, cols: 1, spacing: "sm" }]}>
                <OwnedItemsStats data={ownedItems.data} />
            </SimpleGrid>
            <Group position="apart" align="baseline" my="md" spacing={0}>
                <Title>Egyesített kincstár</Title>
                <Group>
                    <PermissionRequirement permissions={["Shop.CreateOwnedItem"]}>
                        <ActionIcon variant="transparent" color="dark" onClick={() => openCreateOwnedItemModal()}>
                            <IconPlus />
                        </ActionIcon>
                    </PermissionRequirement>
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
                {ownedItems.data.map((ownedItem) => (
                    <OwnedItemCard
                        key={ownedItem.id}
                        ownedItem={ownedItem}
                        openDetails={() => {
                            setDetailsModalOwnedItem(ownedItem);
                            openDetailsModal();
                        }}
                    />
                ))}
            </SimpleGrid>
        </>
    );
};

export default OwnedItemsPage;
