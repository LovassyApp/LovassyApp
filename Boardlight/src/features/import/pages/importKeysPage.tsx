import {
    ActionIcon,
    Button,
    Center,
    CopyButton,
    Divider,
    Group,
    Loader,
    Modal,
    SimpleGrid,
    Switch,
    Text,
    TextInput,
    Title,
    Tooltip,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconCopy, IconPlus, IconX } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    getGetApiImportKeysQueryKey,
    useDeleteApiImportKeysId,
    useGetApiImportKeys,
    useGetApiImportKeysId,
    usePatchApiImportKeysId,
    usePostApiImportKeys,
} from "../../../api/generated/features/import-keys/import-keys";
import { useEffect, useMemo, useState } from "react";

import { ImportIndexImportKeysResponse } from "../../../api/generated/models";
import { ImportKeyCard } from "../components/ImportKeyCard";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useQueryClient } from "@tanstack/react-query";

const useStyles = createStyles(() => ({
    center: {
        height: "100%",
    },
}));

const CreateImportKeyModal = ({ opened, close }: { opened: boolean; close(): void }): JSX.Element => {
    const createQRCode = usePostApiImportKeys();
    const queryClient = useQueryClient();

    const queryKey = getGetApiImportKeysQueryKey();

    const form = useForm({
        initialValues: {
            name: "",
            enabled: false,
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await createQRCode.mutateAsync({
                data: values,
            });
            await queryClient.invalidateQueries({ queryKey: [queryKey[0]] });
            notifications.show({
                title: "Import kulcs létrehozva",
                message: "Az import kulcsot sikeresen létrehoztad.",
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
                <Group position="apart" spacing={0} mt="sm">
                    <Text size="sm">Állapot</Text>
                    <Switch {...form.getInputProps("enabled", { type: "checkbox" })} />
                </Group>
                <Button type="submit" fullWidth={true} loading={createQRCode.isLoading} mt="sm">
                    Létrehozás
                </Button>
            </form>
        </Modal>
    );
};

const DetailsModal = ({
    importKey,
    opened,
    close,
}: {
    importKey: ImportIndexImportKeysResponse;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const queryClient = useQueryClient();

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already
    const detailedQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Import.ViewImportKey") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const importKeyDetailed = useGetApiImportKeysId(importKey?.id, {
        query: { enabled: detailedQueryEnabled && !!importKey },
    });

    const updateImportKey = usePatchApiImportKeysId();
    const deleteImportKey = useDeleteApiImportKeysId();

    const queryKey = getGetApiImportKeysQueryKey();

    useEffect(() => {
        form.setValues({
            name: importKey?.name,
            enabled: importKey?.enabled,
        }); // If we don't do this, the form will be one version behind after an update
    }, [importKey, opened]);

    const form = useForm({
        initialValues: {
            name: importKey?.name,
            enabled: importKey?.enabled,
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await updateImportKey.mutateAsync({ id: importKey.id, data: values });
            await queryClient.invalidateQueries({ queryKey: [queryKey[0]] });
            notifications.show({
                title: "Import kulcs módosítva",
                message: "Az import kulcsot sikeresen módosítottad.",
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

    const doDeleteImportKey = async () => {
        try {
            await deleteImportKey.mutateAsync({ id: importKey.id });
            await queryClient.invalidateQueries({ queryKey: [queryKey[0]] });
            notifications.show({
                title: "Import kulcs törölve",
                message: "Az import kulcsot sikeresen törölted.",
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

    if (importKeyDetailed.isInitialLoading)
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
                <Text weight="bold">{importKey?.name}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Állapot:</Text>
                <Text weight="bold" color={importKey?.enabled ? "green" : "red"}>
                    {importKey?.enabled ? "Használható" : "Nem használható"}
                </Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Létrehozás dátuma:</Text>
                <Text weight="bold">{new Date(importKey?.createdAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Módosítás dátuma:</Text>
                <Text weight="bold">{new Date(importKey?.updatedAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            {importKeyDetailed.data && (
                <>
                    <Divider my="sm" />
                    <Text>Kulcs:</Text>
                    <Group position="apart" spacing={0} noWrap={true}>
                        <Text weight="bold" truncate={true}>
                            {importKeyDetailed.data.key}
                        </Text>
                        <CopyButton value={importKeyDetailed.data.key} timeout={2000}>
                            {({ copied, copy }) => (
                                <Tooltip label={copied ? "Kimásolva" : "Másolás"} withArrow={true} position="right">
                                    <ActionIcon color={copied ? "teal" : "gray"} onClick={copy}>
                                        {copied ? <IconCheck stroke={1.5} /> : <IconCopy stroke={1.5} />}
                                    </ActionIcon>
                                </Tooltip>
                            )}
                        </CopyButton>
                    </Group>
                </>
            )}
            <PermissionRequirement permissions={["Import.UpdateImportKey"]}>
                <Divider my="sm" />
                <form onSubmit={submit}>
                    <TextInput required={true} label="Név" {...form.getInputProps("name")} />
                    <Group position="apart" spacing={0} mt="sm">
                        <Text size="sm">Állapot</Text>
                        <Switch {...form.getInputProps("enabled", { type: "checkbox" })} />
                    </Group>
                    <Button type="submit" fullWidth={true} mt="sm" loading={updateImportKey.isLoading}>
                        Módosítás
                    </Button>
                </form>
            </PermissionRequirement>
            <PermissionRequirement permissions={["Import.DeleteImportKey"]}>
                <Divider my="sm" />
                <Button
                    color="red"
                    fullWidth={true}
                    onClick={async () => await doDeleteImportKey()}
                    loading={deleteImportKey.isLoading}
                >
                    Törlés
                </Button>
            </PermissionRequirement>
        </Modal>
    );
};

const ImportKeysPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const importKeys = useGetApiImportKeys();

    const [createImportKeyModalOpened, { close: closeCreateImportKeyModal, open: openCreateImportKeyModal }] =
        useDisclosure(false);
    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalImportKey, setDetailsModalImportKey] = useState<ImportIndexImportKeysResponse>();

    if (importKeys.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (importKeys.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <CreateImportKeyModal opened={createImportKeyModalOpened} close={closeCreateImportKeyModal} />
            <DetailsModal importKey={detailsModalImportKey} opened={detailsModalOpened} close={closeDetailsModal} />
            <Group position="apart" align="baseline" mb="md" spacing={0}>
                <Title>Import kulcsok</Title>
                <PermissionRequirement permissions={["Import.CreateImportKey"]}>
                    <ActionIcon variant="transparent" color="dark" onClick={() => openCreateImportKeyModal()}>
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
                {importKeys.data.map((importKey) => (
                    <ImportKeyCard
                        importKey={importKey}
                        key={importKey.id}
                        openDetails={() => {
                            setDetailsModalImportKey(importKey);
                            openDetailsModal();
                        }}
                    />
                ))}
            </SimpleGrid>
        </>
    );
};

export default ImportKeysPage;
