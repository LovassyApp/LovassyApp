import {
    ActionIcon,
    Box,
    Button,
    Center,
    Divider,
    Group,
    Image,
    Loader,
    Modal,
    SimpleGrid,
    Stack,
    Text,
    TextInput,
    Textarea,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconPlus, IconPrinter, IconX } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    getGetApiQRCodesQueryKey,
    useDeleteApiQRCodesId,
    useGetApiQRCodes,
    useGetApiQRCodesId,
    usePatchApiQRCodesId,
    usePostApiQRCodes,
} from "../../../api/generated/features/qrcodes/qrcodes";
import { useEffect, useMemo, useState } from "react";

import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { QRCodeCard } from "../components/qrCodeCard";
import { ShopIndexQRCodesResponse } from "../../../api/generated/models";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useQueryClient } from "@tanstack/react-query";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
    printOverlay: {
        display: "none",
        position: "fixed",
        top: 0,
        left: 0,
        zIndex: 1000,
        width: "100%",
        height: "100%",
        backgroundColor: theme.white,
        ["@media print"]: {
            display: "block",
        },
    },
}));

const CreateQRCodeModal = ({ opened, close }: { opened: boolean; close(): void }): JSX.Element => {
    const createQRCode = usePostApiQRCodes();
    const queryClient = useQueryClient();

    const queryKey = getGetApiQRCodesQueryKey();

    const form = useForm({
        initialValues: {
            name: "",
            email: "",
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await createQRCode.mutateAsync({
                data: values,
            });
            await queryClient.invalidateQueries({ queryKey: [queryKey[0]] });
            notifications.show({
                title: "QR kód létrehozva",
                message: "A QR kódot sikeresen létrehoztad.",
                color: "green",
                icon: <IconCheck />,
            });
            close();
        } catch (err) {
            if (err instanceof ValidationError) handleValidationErrors(err, form);
        }
    });

    return (
        <Modal opened={opened} onClose={close} title="Új kérvény" size="lg">
            <form onSubmit={submit}>
                <TextInput label="Név" required={true} {...form.getInputProps("name")} mb="sm" />
                <TextInput label="Email" type="email" required={true} {...form.getInputProps("email")} mb="md" />
                <Button type="submit" fullWidth={true} loading={createQRCode.isLoading}>
                    Létrehozás
                </Button>
            </form>
        </Modal>
    );
};

const DetailsModal = ({
    qrCode,
    opened,
    close,
}: {
    qrCode: ShopIndexQRCodesResponse;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const { classes } = useStyles();

    const queryClient = useQueryClient();

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already
    const detailedQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Shop.ViewQRCode") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const qrCodeDetailed = useGetApiQRCodesId(qrCode?.id, { query: { enabled: detailedQueryEnabled && !!qrCode } });

    const updateQRCode = usePatchApiQRCodesId();
    const deleteQRCode = useDeleteApiQRCodesId();

    const queryKey = getGetApiQRCodesQueryKey();

    useEffect(() => {
        form.setValues({
            name: qrCode?.name,
            email: qrCode?.email,
        }); // If we don't do this, the form will be one version behind after an update
    }, [qrCode, opened]);

    const form = useForm({
        initialValues: {
            name: qrCode?.name,
            email: qrCode?.email,
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await updateQRCode.mutateAsync({ id: qrCode.id, data: values });
            await queryClient.invalidateQueries({ queryKey: [queryKey[0]] });
            notifications.show({
                title: "QR kód módosítva",
                message: "A QR kódot sikeresen módosítottad.",
                color: "green",
                icon: <IconCheck />,
            });
            form.reset();
            close();
        } catch (err) {
            if (err instanceof ValidationError) {
                handleValidationErrors(err, form);
            }
        }
    });

    const doDeleteQRCode = async () => {
        try {
            await deleteQRCode.mutateAsync({ id: qrCode.id });
            await queryClient.invalidateQueries({ queryKey: [queryKey[0]] });
            notifications.show({
                title: "QR kód törölve",
                message: "A QR kódot sikeresen törölted.",
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

    if (qrCodeDetailed.isInitialLoading)
        return (
            <Modal opened={opened} onClose={close} title="Részletek" size="lg">
                <Center>
                    <Loader />
                </Center>
            </Modal>
        );

    return (
        <>
            {qrCodeDetailed.data && opened && (
                <Box className={classes.printOverlay}>
                    <Group position="left" align="flex-start" spacing="lg" noWrap>
                        <Box w="50%">
                            <Image
                                src={`data:image/svg+xml;base64,${btoa(
                                    unescape(encodeURIComponent(qrCodeDetailed.data.imageSvg))
                                )}`}
                                radius="md"
                            />
                        </Box>
                        <Box mt={4}>
                            <Text>Név: {qrCode?.name}</Text>
                            <Text>Email: {qrCode?.email}</Text>
                        </Box>
                    </Group>
                </Box>
            )}
            <Modal opened={opened} onClose={close} title="Részletek" size="lg">
                <Group position="apart" spacing={0}>
                    <Text>Név:</Text>
                    <Text weight="bold">{qrCode?.name}</Text>
                </Group>
                <Group position="apart" spacing={0}>
                    <Text>Email:</Text>
                    <Text weight="bold">{qrCode?.email}</Text>
                </Group>
                <Divider my="sm" />
                <Group position="apart" spacing={0}>
                    <Text>Létrehozás dátuma:</Text>
                    <Text weight="bold">{new Date(qrCode?.createdAt).toLocaleDateString("hu-HU", {})}</Text>
                </Group>
                <Group position="apart" spacing={0}>
                    <Text>Módosítás dátuma:</Text>
                    <Text weight="bold">{new Date(qrCode?.updatedAt).toLocaleDateString("hu-HU", {})}</Text>
                </Group>
                {qrCodeDetailed.data && (
                    <>
                        <Divider my="sm" />
                        <Image
                            src={`data:image/svg+xml;base64,${btoa(
                                unescape(encodeURIComponent(qrCodeDetailed.data.imageSvg))
                            )}`}
                            width="100%"
                            radius="md"
                            alt="QR kód"
                        />
                        <Button fullWidth onClick={() => print()} mt="md" leftIcon={<IconPrinter stroke={1.5} />}>
                            Nyomtatás
                        </Button>
                    </>
                )}
                <PermissionRequirement permissions={["Shop.UpdateQRCode"]}>
                    <Divider my="sm" />
                    <form onSubmit={submit}>
                        <TextInput required={true} label="Név" {...form.getInputProps("name")} />
                        <TextInput
                            type="email"
                            required={true}
                            label="Email"
                            mt="sm"
                            {...form.getInputProps("email")}
                        />
                        <Button type="submit" fullWidth={true} mt="md" loading={updateQRCode.isLoading}>
                            Módosítás
                        </Button>
                    </form>
                </PermissionRequirement>
                <PermissionRequirement permissions={["Shop.DeleteQRCode"]}>
                    <Divider my="sm" />
                    <Button
                        fullWidth={true}
                        color="red"
                        onClick={async () => await doDeleteQRCode()}
                        loading={deleteQRCode.isLoading}
                    >
                        Törlés
                    </Button>
                </PermissionRequirement>
            </Modal>
        </>
    );
};

const QRCodesPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const qrCodes = useGetApiQRCodes();

    const [createQRCodeModalOpened, { close: closeCreateQRCodeModal, open: openCreateQRCodeModal }] =
        useDisclosure(false);
    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalQRCode, setDetailsModalQRCode] = useState<ShopIndexQRCodesResponse>();

    if (qrCodes.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (qrCodes.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <CreateQRCodeModal opened={createQRCodeModalOpened} close={closeCreateQRCodeModal} />
            <DetailsModal opened={detailsModalOpened} close={closeDetailsModal} qrCode={detailsModalQRCode} />
            <Group position="apart" align="baseline" mb="md" spacing={0}>
                <Title>QR kódok</Title>
                <PermissionRequirement permissions={["Shop.CreateQRCode"]}>
                    <ActionIcon variant="transparent" color="dark" onClick={() => openCreateQRCodeModal()}>
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
                {qrCodes.data?.map((qrCode) => (
                    <QRCodeCard
                        key={qrCode.id}
                        qrCode={qrCode}
                        openDetails={() => {
                            setDetailsModalQRCode(qrCode);
                            openDetailsModal();
                        }}
                    />
                ))}
            </SimpleGrid>
        </>
    );
};

export default QRCodesPage;
