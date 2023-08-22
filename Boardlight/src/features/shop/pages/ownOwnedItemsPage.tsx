import {
    ActionIcon,
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
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconFilter, IconSearch, IconX } from "@tabler/icons-react";
import { ShopIndexOwnOwnedItemsResponse, ShopUseOwnedItemRequestBody } from "../../../api/generated/models";
import {
    useDeleteApiOwnedItemsId,
    useGetApiOwnedItemsOwn,
    usePostApiOwnedItemsIdUse,
} from "../../../api/generated/features/owned-items/owned-items";
import { useEffect, useState } from "react";

import { OwnedItemCard } from "../components/ownedItemCard";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { QrCodeReader } from "../../../core/components/qrCodeReader";
import { ValidationError } from "../../../helpers/apiHelpers";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";

const useStyles = createStyles(() => ({
    center: {
        height: "100%",
    },
}));

interface OwnOwnedItemsParams {
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
    params: OwnOwnedItemsParams;
    setParams(arg0: OwnOwnedItemsParams): void;
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
    ownedItem: ShopIndexOwnOwnedItemsResponse;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const useOwnedItem = usePostApiOwnedItemsIdUse();
    const deleteOwnedItem = useDeleteApiOwnedItemsId();

    const form = useForm({
        initialValues: {
            qrCodeContent: undefined,
            inputs: ownedItem?.product.inputs.reduce((acc, input) => {
                acc[input.key] = input.type === "Textbox" ? "" : false;
                return acc;
            }, {}),
        },
        transformValues: (values) => {
            return {
                qrCodeContent: values.qrCodeContent,
                inputs: Object.keys(values.inputs).reduce((acc, key) => {
                    acc[key] = values.inputs[key].toString();
                    return acc;
                }, {}),
            };
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await useOwnedItem.mutateAsync({
                id: ownedItem?.id,
                data: values as unknown as ShopUseOwnedItemRequestBody,
            });
            notifications.show({
                title: "Termék felhasználva",
                message: "A terméket sikeresen felhasználtad. (Ez az üzenet 10 másodperc múlva eltűnik.)",
                color: "green",
                autoClose: 10000,
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
    });

    useEffect(() => {
        form.setFieldValue("qrCodeContent", undefined);
        form.setFieldValue(
            "inputs",
            ownedItem?.product.inputs.reduce((acc, input) => {
                acc[input.key] = input.type === "Textbox" ? "" : false;
                return acc;
            }, {})
        );
    }, [opened, ownedItem]);

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

    return (
        <Modal opened={opened} onClose={close} size="lg" title="Részletek">
            <Group position="apart" spacing={0}>
                <Text>Név:</Text>
                <Text weight="bold">{ownedItem?.product.name}</Text>
            </Group>
            <Text>Leírás:</Text>
            <Text weight="bold">{ownedItem?.product.description}</Text>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>QR kód aktivált:</Text>
                <Text weight="bold">{ownedItem?.product.qrCodeActivated ? "Igen" : "Nem"}</Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Létrehozás dátuma:</Text>
                <Text weight="bold">{new Date(ownedItem?.createdAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Módosítás dátuma:</Text>
                <Text weight="bold">{new Date(ownedItem?.updatedAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            {ownedItem?.usedAt ? (
                <>
                    <Divider my="sm" />
                    <Group position="apart" spacing={0}>
                        <Text>Felhasználás dátuma:</Text>
                        <Text weight="bold">{new Date(ownedItem?.usedAt).toLocaleDateString("hu-HU", {})}</Text>
                    </Group>
                </>
            ) : (
                <PermissionRequirement permissions={["Shop.UseOwnOwnedItem"]}>
                    <Divider my="sm" />
                    <form onSubmit={submit}>
                        {ownedItem?.product.qrCodeActivated && (
                            <>
                                <Text size="sm" weight={500}>
                                    QR kód
                                </Text>
                                {!form.values.qrCodeContent && (
                                    <QrCodeReader
                                        successCallback={(text, result) => form.setFieldValue("qrCodeContent", text)}
                                    />
                                )}
                                <Group position="apart" spacing={0}>
                                    <Text size="sm">Beolvasás állapota:</Text>
                                    <Text size="sm" color={form.values.qrCodeContent ? "green" : "yellow"}>
                                        {form.values.qrCodeContent ? "Sikeres" : "QR kódra vár"}
                                    </Text>
                                </Group>
                            </>
                        )}
                        {ownedItem?.product.inputs.length > 0 && (
                            <>
                                {Object.keys(form.values.inputs ?? {}).map((key) => {
                                    if (
                                        ownedItem?.product.inputs.find((input) => input.key === key)?.type === "Textbox"
                                    ) {
                                        return (
                                            <TextInput
                                                key={key}
                                                label={
                                                    ownedItem?.product.inputs.find((input) => input.key === key)?.label
                                                }
                                                value={form.values.inputs[key]}
                                                onChange={(event) =>
                                                    form.setFieldValue("inputs", {
                                                        ...form.values.inputs,
                                                        [key]: event.currentTarget.value,
                                                    })
                                                }
                                                required={true}
                                                mt="sm"
                                            />
                                        );
                                    } else if (
                                        ownedItem?.product.inputs.find((input) => input.key === key)?.type === "Boolean"
                                    ) {
                                        return (
                                            <Group key={key} position="apart" align="center" mt="sm">
                                                <Text size="sm">
                                                    {
                                                        ownedItem?.product.inputs.find((input) => input.key === key)
                                                            ?.label
                                                    }
                                                </Text>
                                                <Switch
                                                    checked={form.values.inputs[key]}
                                                    onChange={() =>
                                                        form.setFieldValue("inputs", {
                                                            ...form.values.inputs,
                                                            [key]: !form.values.inputs[key],
                                                        })
                                                    }
                                                />
                                            </Group>
                                        );
                                    }
                                })}
                            </>
                        )}
                        <Button type="submit" fullWidth={true} mt="md" loading={useOwnedItem.isLoading}>
                            Felhasználás
                        </Button>
                    </form>
                </PermissionRequirement>
            )}
            <PermissionRequirement permissions={["Shop.DeleteOwnOwnedItem", "Shop.DeleteOwnedItem"]}>
                <Divider my="sm" />
                <Button fullWidth={true} color="red" onClick={() => doDeleteOwnedItem()}>
                    Termék törlése
                </Button>
            </PermissionRequirement>
        </Modal>
    );
};

const OwnOwnedItemsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const [params, setParams] = useState<OwnOwnedItemsParams>({});

    const ownedItems = useGetApiOwnedItemsOwn({
        Search: params.Search ?? "",
        Filters: params.Filters ?? "",
        Sorts: "-CreatedAt",
    });

    const [filtersDraweOpened, { open: openFiltersDrawer, close: closeFiltersDrawer }] = useDisclosure(false);
    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalOwnedItem, setDetailsModalOwnedItem] = useState<ShopIndexOwnOwnedItemsResponse>();

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
                <Title>Kincstár</Title>
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

export default OwnOwnedItemsPage;
