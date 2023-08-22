import {
    ActionIcon,
    Box,
    Button,
    Center,
    Divider,
    Drawer,
    Group,
    Loader,
    Modal,
    SimpleGrid,
    Text,
    TextInput,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconFilter, IconSearch, IconX } from "@tabler/icons-react";
import { ShopIndexOwnOwnedItemsResponse, ShopIndexOwnedItemsResponse } from "../../../api/generated/models";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    useDeleteApiOwnedItemsId,
    useGetApiOwnedItems,
    useGetApiOwnedItemsOwn,
    usePatchApiOwnedItemsId,
} from "../../../api/generated/features/owned-items/owned-items";
import { useEffect, useMemo, useState } from "react";

import { DateInput } from "@mantine/dates";
import { OwnedItemCard } from "../components/ownedItemCard";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useGetApiUsersId } from "../../../api/generated/features/users/users";

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
                        <Text weight="bold">{user.data?.realName}</Text>
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
                    <DateInput label="Felhasználás dátuma" clearable={true} {...form.getInputProps("usedAt")} />
                    <Button type="submit" fullWidth={true} mt="md">
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

    const [filtersDraweOpened, { open: openFiltersDrawer, close: closeFiltersDrawer }] = useDisclosure(false);
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
                opened={filtersDraweOpened}
                close={closeFiltersDrawer}
                params={params}
                setParams={setParams}
            />
            <DetailsModal ownedItem={detailsModalOwnedItem} opened={detailsModalOpened} close={closeDetailsModal} />
            <Group position="apart" align="baseline" mb="md" spacing={0}>
                <Title>Egyesített kincstár</Title>
                <ActionIcon variant="transparent" color="dark" onClick={() => openFiltersDrawer()}>
                    <IconFilter />
                </ActionIcon>
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
