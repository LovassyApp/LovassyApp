import {
    ActionIcon,
    Box,
    Button,
    Center,
    Divider,
    Group,
    Loader,
    Modal,
    MultiSelect,
    NumberInput,
    Select,
    SimpleGrid,
    Switch,
    Text,
    TextInput,
    Title,
    TypographyStylesProvider,
    createStyles,
    rem,
    useMantineTheme,
} from "@mantine/core";
import { CustomRichTextEditor, tiptapExtensions } from "../components/customRichTextEditor";
import { IconCheck, IconPlus, IconTrash, IconX } from "@tabler/icons-react";
import { ShopIndexProductsResponse, ShopUpdateProductRequestBody } from "../../../api/generated/models";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    getGetApiProductsIdQueryKey,
    useDeleteApiProductsId,
    useGetApiProducts,
    useGetApiProductsId,
    usePatchApiProductsId,
    usePostApiProducts,
} from "../../../api/generated/features/products/products";
import { useEffect, useMemo, useState } from "react";

import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { ProductCard } from "../components/productCard";
import { StoreProductCard } from "../components/storeProductCard";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useEditor } from "@tiptap/react";
import { useForm } from "@mantine/form";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useGetApiQRCodes } from "../../../api/generated/features/qrcodes/qrcodes";
import { useQueryClient } from "@tanstack/react-query";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

const CreateProductModal = ({ opened, close }: { opened: boolean; close(): void }): JSX.Element => {
    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const qrCodesQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Shop.IndexQRCodes") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const qrCodes = useGetApiQRCodes({}, { query: { enabled: qrCodesQueryEnabled } });

    const createProduct = usePostApiProducts();

    const form = useForm({
        initialValues: {
            name: "",
            description: "",
            richTextContent: "",
            visible: false,
            qrCodeActivated: false,
            qrCodes: [],
            price: 0,
            quantity: 0,
            inputs: [],
            notifiedEmails: [],
            thumbnailUrl: "",
        },
        transformValues: (values) => ({
            ...values,
            qrCodes: values.qrCodes.map((qrCode) => +qrCode),
        }),
        validate: (values) => ({
            notifiedEmails: values.notifiedEmails.some((email) => !email.includes("@"))
                ? "Található egy érvénytelen email"
                : null,
        }),
    });

    const rteEditor = useEditor({
        extensions: tiptapExtensions,
        content: "",
        onUpdate: ({ editor }) => {
            form.setFieldValue("richTextContent", editor.getHTML());
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await createProduct.mutateAsync({ data: values as ShopUpdateProductRequestBody });
            notifications.show({
                title: "Termék létrehozva",
                message: "A terméket sikeresen létrehoztad.",
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

    const qrCodesData = useMemo(() => {
        if (!qrCodes.data) return [];
        return qrCodes.data.map((qrCode) => ({
            label: qrCode.name,
            value: qrCode.id.toString(),
        }));
    }, [qrCodes.data]);

    const [creatableQRCodesData, setCreatableQRCodesData] = useState([]);
    const [creatableNotifiedEmailsData, setCreatableNotifiedEmailsData] = useState([]);

    const displayedInputs = useMemo(
        () =>
            form.values.inputs.map((input, index) => (
                <Group key={input.key} spacing="sm" mb="xs" noWrap={true} align="flex-end">
                    <TextInput
                        label="Név"
                        required={true}
                        {...form.getInputProps(`inputs.${index}.label`)}
                        sx={{ flex: 1 }}
                    />
                    <Select
                        data={[
                            { label: "Szövegdoboz", value: "Textbox" },
                            { label: "Igen/Nem", value: "Boolean" },
                        ]}
                        label="Típus"
                        required={true}
                        {...form.getInputProps(`inputs.${index}.type`)}
                    />
                    <ActionIcon
                        variant="transparent"
                        color="red"
                        onClick={() => form.removeListItem("inputs", index)}
                        mb={rem(6)}
                    >
                        <IconTrash stroke={1.5} />
                    </ActionIcon>
                </Group>
            )),
        [form]
    );

    if (qrCodes.isInitialLoading)
        return (
            <Modal opened={opened} onClose={close} title="Új termék" size="lg">
                <Center>
                    <Loader />
                </Center>
            </Modal>
        );

    return (
        <Modal opened={opened} onClose={close} size="lg" title="Új termék">
            <Text align="center" color="dimmed" mb="sm">
                Előnézet
            </Text>
            <Center>
                <Box maw={400}>
                    <StoreProductCard storeProduct={form.values} openDetails={() => {}} />
                </Box>
            </Center>
            <Divider my="sm" />
            <form onSubmit={submit}>
                <TextInput label="Név" required={true} {...form.getInputProps("name")} />
                <TextInput label="Leírás" required={true} {...form.getInputProps("description")} mt="sm" />
                <CustomRichTextEditor editor={rteEditor} mt="md" />
                {form.errors.richTextContent && (
                    <Text color="red" size="sm">
                        {form.errors.richTextContent}
                    </Text>
                )}
                <Group position="apart" spacing={0} mt="md">
                    <Text size="sm">Látható</Text>
                    <Switch {...form.getInputProps("visible", { type: "checkbox" })} />
                </Group>
                <Group position="apart" spacing={0} mt="md">
                    <Text size="sm">QR kód aktivált</Text>
                    <Switch {...form.getInputProps("qrCodeActivated", { type: "checkbox" })} />
                </Group>
                {qrCodes.data && qrCodesData.length > 0 ? (
                    <MultiSelect
                        label="QR kódok"
                        data={qrCodesData}
                        {...form.getInputProps("qrCodes")}
                        mt="sm"
                        withinPortal={true}
                        clearable={true}
                        searchable={true}
                    />
                ) : (
                    <MultiSelect
                        label="QR kód id-k"
                        {...form.getInputProps("qrCodes")}
                        data={creatableQRCodesData}
                        mt="sm"
                        withinPortal={true}
                        searchable={true}
                        clearable={true}
                        creatable={true}
                        getCreateLabel={(value) => `+ Id hozzáadása: ${value}`}
                        onCreate={(query) => {
                            const id = +query;
                            if (isNaN(id)) return;
                            setCreatableQRCodesData([
                                ...creatableQRCodesData,
                                { label: id.toString(), value: id.toString() },
                            ]);
                            return { label: id.toString(), value: id.toString() };
                        }}
                    />
                )}
                <NumberInput label="Ár" required={true} min={0} {...form.getInputProps("price")} mt="sm" />
                <NumberInput label="Mennyiség" required={true} min={0} {...form.getInputProps("quantity")} mt="sm" />
                <Text size="sm" mt="sm" weight={500}>
                    Beviteli mezők
                </Text>
                {displayedInputs}{" "}
                <Button
                    onClick={() =>
                        form.insertListItem("inputs", {
                            label: "",
                            key: `i${form.values.inputs.length.toString()}`,
                            type: "Textbox",
                        })
                    }
                    variant="outline"
                    fullWidth={true}
                    mt="md"
                >
                    Mező hozzáadása
                </Button>
                <MultiSelect
                    label="Értesítendő email címek"
                    data={creatableNotifiedEmailsData}
                    {...form.getInputProps("notifiedEmails")}
                    withinPortal={true}
                    searchable={true}
                    clearable={true}
                    creatable={true}
                    getCreateLabel={(value) => `+ Email hozzáadása: ${value}`}
                    onCreate={(query) => {
                        setCreatableNotifiedEmailsData([
                            ...creatableNotifiedEmailsData,
                            { label: query, value: query },
                        ]);
                        return { label: query, value: query };
                    }}
                    mt="sm"
                />
                <TextInput required={true} label="Kép URL" {...form.getInputProps("thumbnailUrl")} mt="sm" />
                <Button type="submit" fullWidth={true} mt="md">
                    Létrehozás
                </Button>
            </form>
        </Modal>
    );
};

const DetailsModal = ({
    product,
    opened,
    close,
}: {
    product: ShopIndexProductsResponse;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const queryClient = useQueryClient();

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const detailedQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Shop.ViewProduct") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const qrCodesQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Shop.IndexQRCodes") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const productDetailed = useGetApiProductsId(product?.id, { query: { enabled: detailedQueryEnabled && !!product } });
    const qrCodes = useGetApiQRCodes({}, { query: { enabled: qrCodesQueryEnabled } });

    const updateProduct = usePatchApiProductsId();
    const deleteProduct = useDeleteApiProductsId();

    const detailedQueryKey = getGetApiProductsIdQueryKey(product?.id);

    const rteEditor = useEditor(
        {
            extensions: tiptapExtensions,
            content: productDetailed.data?.richTextContent ?? "",
            onUpdate: ({ editor }) => {
                form.setFieldValue("richTextContent", editor.getHTML());
            },
        },
        [productDetailed.data?.richTextContent]
    );

    const form = useForm({
        initialValues: {
            name: product?.name,
            description: product?.description,
            richTextContent: productDetailed.data?.richTextContent ?? "",
            visible: product?.visible,
            qrCodeActivated: product?.qrCodeActivated,
            qrCodes: productDetailed.data?.qrCodes.map((qrCode) => qrCode.id.toString()) ?? [],
            price: product?.price,
            quantity: product?.quantity,
            inputs: productDetailed.data?.inputs ?? [],
            notifiedEmails: productDetailed.data?.notifiedEmails ?? [],
            thumbnailUrl: product?.thumbnailUrl,
        },
        transformValues: (values) => ({
            ...values,
            qrCodes: values.qrCodes.map((qrCode) => +qrCode),
        }),
        validate: (values) => ({
            notifiedEmails: values.notifiedEmails.some((email) => !email.includes("@"))
                ? "Legalább egy email érvénytelen"
                : null,
        }),
    });

    useEffect(() => {
        form.setValues({
            name: product?.name,
            description: product?.description,
            richTextContent: productDetailed.data?.richTextContent ?? "",
            visible: product?.visible,
            qrCodeActivated: product?.qrCodeActivated,
            qrCodes: productDetailed.data?.qrCodes.map((qrCode) => qrCode.id.toString()) ?? [],
            price: product?.price,
            quantity: product?.quantity,
            inputs: productDetailed.data?.inputs ?? [],
            notifiedEmails: productDetailed.data?.notifiedEmails ?? [],
            thumbnailUrl: product?.thumbnailUrl,
        });
        if (productDetailed.data?.notifiedEmails.length > 0) {
            setCreatableNotifiedEmailsData(
                productDetailed.data.notifiedEmails.map((email) => ({ label: email, value: email }))
            );
        }
    }, [product, productDetailed.data, opened]);

    const doDeleteProduct = async () => {
        try {
            await deleteProduct.mutateAsync({ id: product.id });
            notifications.show({
                title: "Termék törölve",
                message: "A terméket sikeresen törölted.",
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

    const submit = form.onSubmit(async (values) => {
        try {
            await updateProduct.mutateAsync({ id: product.id, data: values as ShopUpdateProductRequestBody });
            await queryClient.invalidateQueries({ queryKey: [detailedQueryKey[0]] });
            notifications.show({
                title: "Termék módosítva",
                message: "A terméket sikeresen módosítottad.",
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

    const qrCodesData = useMemo(() => {
        if (!qrCodes.data) return [];
        return qrCodes.data.map((qrCode) => ({
            label: qrCode.name,
            value: qrCode.id.toString(),
        }));
    }, [qrCodes.data]);

    const [creatableQRCodesData, setCreatableQRCodesData] = useState([]);
    const [creatableNotifiedEmailsData, setCreatableNotifiedEmailsData] = useState([]);

    const displayedInputs = useMemo(
        () =>
            form.values.inputs.map((input, index) => (
                <Group key={input.key} spacing="sm" mb="xs" noWrap={true} align="flex-end">
                    <TextInput
                        label="Név"
                        required={true}
                        {...form.getInputProps(`inputs.${index}.label`)}
                        sx={{ flex: 1 }}
                    />
                    <Select
                        data={[
                            { label: "Szövegdoboz", value: "Textbox" },
                            { label: "Igen/Nem", value: "Boolean" },
                        ]}
                        label="Típus"
                        required={true}
                        {...form.getInputProps(`inputs.${index}.type`)}
                    />
                    <ActionIcon
                        variant="transparent"
                        color="red"
                        onClick={() => form.removeListItem("inputs", index)}
                        mb={rem(6)}
                    >
                        <IconTrash stroke={1.5} />
                    </ActionIcon>
                </Group>
            )),
        [form]
    );

    if (productDetailed.isInitialLoading || qrCodes.isInitialLoading)
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
                <Text weight="bold">{product?.name}</Text>
            </Group>
            <Text>Leírás:</Text>
            <Text weight="bold">{product?.description}</Text>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Ár:</Text>
                <Text weight="bold">{product?.price} loló</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Mennyiség:</Text>
                <Text weight="bold">{product?.quantity} db</Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>QR kód aktivált:</Text>
                <Text weight="bold">{product?.qrCodeActivated ? "Igen" : "Nem"}</Text>
            </Group>
            {productDetailed.data?.inputs.length > 0 && (
                <>
                    <Group position="apart" spacing={0}>
                        <Text>Inputok:</Text>
                        <Text weight="bold">{productDetailed.data.inputs.map((input) => input.label).join(", ")}</Text>
                    </Group>
                </>
            )}
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Látható:</Text>
                <Text weight="bold">{product?.visible ? "Igen" : "Nem"}</Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Létrehozás dátuma:</Text>
                <Text weight="bold">{new Date(product?.createdAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Módosítás dátuma:</Text>
                <Text weight="bold">{new Date(product?.updatedAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            {productDetailed.data && (
                <>
                    <Divider my="sm" />
                    <TypographyStylesProvider>
                        <div dangerouslySetInnerHTML={{ __html: productDetailed.data?.richTextContent ?? "" }} />
                    </TypographyStylesProvider>
                </>
            )}
            <Divider my="sm" />
            <Text align="center" color="dimmed" mb="sm">
                Előnézet
            </Text>
            <Center>
                <Box maw={400}>
                    <StoreProductCard storeProduct={form.values} openDetails={() => {}} />
                </Box>
            </Center>
            <PermissionRequirement permissions={["Shop.UpdateProduct"]}>
                <Divider my="sm" />
                <form onSubmit={submit}>
                    <TextInput label="Név" required={true} {...form.getInputProps("name")} />
                    <TextInput label="Leírás" required={true} {...form.getInputProps("description")} mt="sm" />
                    <CustomRichTextEditor editor={rteEditor} mt="md" />
                    {form.errors.richTextContent && (
                        <Text color="red" size="sm">
                            {form.errors.richTextContent}
                        </Text>
                    )}
                    <Group position="apart" spacing={0} mt="md">
                        <Text size="sm">Látható</Text>
                        <Switch {...form.getInputProps("visible", { type: "checkbox" })} />
                    </Group>
                    <Group position="apart" spacing={0} mt="md">
                        <Text size="sm">QR kód aktivált</Text>
                        <Switch {...form.getInputProps("qrCodeActivated", { type: "checkbox" })} />
                    </Group>
                    {qrCodes.data && qrCodesData.length > 0 ? (
                        <MultiSelect
                            label="QR kódok"
                            data={qrCodesData}
                            {...form.getInputProps("qrCodes")}
                            mt="sm"
                            withinPortal={true}
                            clearable={true}
                            searchable={true}
                        />
                    ) : (
                        <MultiSelect
                            label="QR kód id-k"
                            description="Figyelmeztetés: Az itt megadottak fölülírják a jelenlegi QR kódokat!"
                            {...form.getInputProps("qrCodes")}
                            data={creatableQRCodesData}
                            mt="sm"
                            withinPortal={true}
                            searchable={true}
                            clearable={true}
                            creatable={true}
                            getCreateLabel={(value) => `+ Id hozzáadása: ${value}`}
                            onCreate={(query) => {
                                const id = +query;
                                if (isNaN(id)) return;
                                setCreatableQRCodesData([
                                    ...creatableQRCodesData,
                                    { label: id.toString(), value: id.toString() },
                                ]);
                                return { label: id.toString(), value: id.toString() };
                            }}
                        />
                    )}
                    <NumberInput label="Ár" required={true} min={0} {...form.getInputProps("price")} mt="sm" />
                    <NumberInput
                        label="Mennyiség"
                        required={true}
                        min={0}
                        {...form.getInputProps("quantity")}
                        mt="sm"
                    />
                    <Text size="sm" mt="sm" weight={500}>
                        Beviteli mezők
                    </Text>
                    {!productDetailed.data && (
                        <Text size="xs" color="dimmed">
                            Figyelmeztetés: Az itt megadottak fölülírják a jelenlegi beviteli mezőket!
                        </Text>
                    )}
                    {displayedInputs}{" "}
                    <Button
                        onClick={() =>
                            form.insertListItem("inputs", {
                                label: "",
                                key: `i${form.values.inputs.length.toString()}`,
                                type: "Textbox",
                            })
                        }
                        variant="outline"
                        fullWidth={true}
                        mt="md"
                    >
                        Mező hozzáadása
                    </Button>
                    <MultiSelect
                        label="Értesítendő email címek"
                        data={creatableNotifiedEmailsData}
                        {...form.getInputProps("notifiedEmails")}
                        withinPortal={true}
                        searchable={true}
                        clearable={true}
                        creatable={true}
                        getCreateLabel={(value) => `+ Email hozzáadása: ${value}`}
                        onCreate={(query) => {
                            setCreatableNotifiedEmailsData([
                                ...creatableNotifiedEmailsData,
                                { label: query, value: query },
                            ]);
                            return { label: query, value: query };
                        }}
                        mt="sm"
                    />
                    <TextInput required={true} label="Kép URL" {...form.getInputProps("thumbnailUrl")} mt="sm" />
                    <Button type="submit" fullWidth={true} mt="md">
                        Mentés
                    </Button>
                </form>
            </PermissionRequirement>
            <PermissionRequirement permissions={["Shop.DeleteProduct"]}>
                <Divider my="sm" />
                <Button
                    fullWidth={true}
                    color="red"
                    onClick={async () => await doDeleteProduct()}
                    loading={deleteProduct.isLoading}
                >
                    Törlés
                </Button>
            </PermissionRequirement>
        </Modal>
    );
};

const ProductsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const products = useGetApiProducts();

    const [createProductModalOpened, { close: closeCreateProductModal, open: openCreateProductModal }] =
        useDisclosure(false);
    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalProduct, setDetailsModalProduct] = useState<ShopIndexProductsResponse>();

    if (products.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (products.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <CreateProductModal opened={createProductModalOpened} close={closeCreateProductModal} />
            <DetailsModal product={detailsModalProduct} opened={detailsModalOpened} close={closeDetailsModal} />
            <Group position="apart" align="baseline" mb="md" spacing={0}>
                <Title>Termékek</Title>
                <PermissionRequirement permissions={["Shop.CreateProduct"]}>
                    <ActionIcon variant="transparent" color="dark" onClick={() => openCreateProductModal()}>
                        <IconPlus />
                    </ActionIcon>
                </PermissionRequirement>
            </Group>
            <SimpleGrid
                cols={4}
                breakpoints={[
                    { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                    { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                    { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                ]}
            >
                {products.data.map((product) => (
                    <ProductCard
                        key={product.id}
                        product={product}
                        openDetails={() => {
                            setDetailsModalProduct(product);
                            openDetailsModal();
                        }}
                    />
                ))}
            </SimpleGrid>
        </>
    );
};

export default ProductsPage;
