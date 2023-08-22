import {
    ActionIcon,
    Button,
    Center,
    Divider,
    Group,
    Loader,
    Modal,
    SimpleGrid,
    Text,
    TextInput,
    Textarea,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconPlus, IconX } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    useDeleteApiLoloRequestsId,
    useGetApiLoloRequestsOwn,
    usePatchApiLoloRequestsId,
    usePostApiLoloRequests,
} from "../../../api/generated/features/lolo-requests/lolo-requests";
import { useEffect, useState } from "react";

import { LoloRequestCard } from "../components/loloRequestCard";
import { LoloRequestStats } from "../components/loloRequestStats";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { ShopIndexLoloRequestsResponse } from "../../../api/generated/models";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";

const useStyles = createStyles(() => ({
    center: {
        height: "100%",
    },
}));

const CreateLoloRequestModal = ({ opened, close }: { opened: boolean; close(): void }): JSX.Element => {
    const createLoloRequest = usePostApiLoloRequests();

    const form = useForm({
        initialValues: {
            title: "",
            body: "",
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await createLoloRequest.mutateAsync({
                data: values,
            });
            notifications.show({
                title: "Kérvény létrehozva",
                message: "A kérvényt sikeresen létrehoztad.",
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

    return (
        <Modal opened={opened} onClose={close} title="Új kérvény" size="lg">
            <form onSubmit={submit}>
                <TextInput label="Cím" required={true} {...form.getInputProps("title")} mb="sm" />
                <Textarea label="Törzsszöveg" required={true} {...form.getInputProps("body")} mb="md" />
                <Button type="submit" fullWidth={true} loading={createLoloRequest.isLoading}>
                    Létrehozás
                </Button>
            </form>
        </Modal>
    );
};

const DetailsModal = ({
    loloRequest,
    opened,
    close,
}: {
    loloRequest: ShopIndexLoloRequestsResponse;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const updateLoloRequest = usePatchApiLoloRequestsId();
    const deleteLoloRequest = useDeleteApiLoloRequestsId();

    useEffect(() => {
        form.setValues({
            title: loloRequest?.title,
            body: loloRequest?.body,
        }); // If we don't do this, the form will be one version behind after an update
    }, [loloRequest, opened]);

    const form = useForm({
        initialValues: {
            title: loloRequest?.title,
            body: loloRequest?.body,
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await updateLoloRequest.mutateAsync({ data: values, id: loloRequest.id });
            notifications.show({
                title: "Kérvény módosítva",
                message: "A kérvényed sikeresen módosítottad.",
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

    const doDeleteLoloRequest = async () => {
        try {
            await deleteLoloRequest.mutateAsync({ id: loloRequest.id });
            notifications.show({
                title: "Kérvény törölve",
                message: "A kérvényed sikeresen törölted.",
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
                <Text>Cím:</Text>
                <Text weight="bold">{loloRequest?.title}</Text>
            </Group>
            <Text>Törzsszöveg:</Text>
            <Text weight="bold">{loloRequest?.body}</Text>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Állapot:</Text>
                {loloRequest?.acceptedAt && (
                    <Text weight="bold" color="green">
                        Elfogadva
                    </Text>
                )}
                {loloRequest?.deniedAt && (
                    <Text weight="bold" color="red">
                        Elutasítva
                    </Text>
                )}
                {!loloRequest?.acceptedAt && !loloRequest?.deniedAt && (
                    <Text weight="bold" color="yellow">
                        Függőben
                    </Text>
                )}
            </Group>
            {loloRequest?.acceptedAt && (
                <Group position="apart" spacing={0}>
                    <Text>Elfogadás dátuma:</Text>
                    <Text weight="bold">{new Date(loloRequest?.acceptedAt).toLocaleDateString("hu-HU", {})}</Text>
                </Group>
            )}
            {loloRequest?.deniedAt && (
                <Group position="apart" spacing={0}>
                    <Text>Elutasítás dátuma:</Text>
                    <Text weight="bold">{new Date(loloRequest?.deniedAt).toLocaleDateString("hu-HU", {})}</Text>
                </Group>
            )}
            <Group position="apart" spacing={0}>
                <Text>Létrehozás dátuma:</Text>
                <Text weight="bold">{new Date(loloRequest?.createdAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            {!loloRequest?.acceptedAt && !loloRequest?.deniedAt && (
                <>
                    <PermissionRequirement permissions={["Shop.UpdateOwnLoloRequest", "Shop.UpdateLoloRequest"]}>
                        <Divider my="sm" />
                        <form onSubmit={submit}>
                            <TextInput required={true} label="Cím" {...form.getInputProps("title")} />
                            <Textarea required={true} label="Törzsszöveg" mt="sm" {...form.getInputProps("body")} />
                            <Button type="submit" fullWidth={true} mt="md" loading={updateLoloRequest.isLoading}>
                                Módosítás
                            </Button>
                        </form>
                    </PermissionRequirement>
                    <PermissionRequirement permissions={["Shop.DeleteOwnLoloRequest", "Shop.DeleteLoloRequest"]}>
                        <Divider my="sm" />
                        <Button
                            fullWidth={true}
                            color="red"
                            onClick={async () => await doDeleteLoloRequest()}
                            loading={deleteLoloRequest.isLoading}
                        >
                            Törlés
                        </Button>
                    </PermissionRequirement>
                </>
            )}
        </Modal>
    );
};

const OwnLoloRequestsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const loloRequests = useGetApiLoloRequestsOwn();

    const [createLoloRequestModalOpened, { close: closeCreateLoloRequestModal, open: openCreateLoloRequestModal }] =
        useDisclosure(false);
    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalLoloRequest, setDetailsModalLoloRequest] = useState<ShopIndexLoloRequestsResponse>();

    if (loloRequests.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (loloRequests.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <CreateLoloRequestModal opened={createLoloRequestModalOpened} close={closeCreateLoloRequestModal} />
            <DetailsModal opened={detailsModalOpened} close={closeDetailsModal} loloRequest={detailsModalLoloRequest} />
            <Title mb="md">Statisztikák</Title>
            <SimpleGrid cols={2} breakpoints={[{ maxWidth: theme.breakpoints.sm, cols: 1, spacing: "sm" }]}>
                <LoloRequestStats data={loloRequests.data} />
            </SimpleGrid>
            <Group position="apart" align="baseline" my="md" spacing={0}>
                <Title>Kérvények</Title>
                <PermissionRequirement permissions={["Shop.CreateLoloRequest"]}>
                    <ActionIcon variant="transparent" color="dark" onClick={() => openCreateLoloRequestModal()}>
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
                {loloRequests.data?.map((loloRequest) => (
                    <LoloRequestCard
                        key={loloRequest.id}
                        loloRequest={loloRequest}
                        openDetails={() => {
                            setDetailsModalLoloRequest(loloRequest);
                            openDetailsModal();
                        }}
                    />
                ))}
            </SimpleGrid>
            {loloRequests.data.length === 0 && (
                <Text color="dimmed">
                    Úgy néz ki még nincs egy kérvényed sem. Kattints a &quot;Kérvények&quot; felirat melleti plusz
                    ikonra, hogy létrehozz egyet.
                </Text>
            )}
        </>
    );
};

export default OwnLoloRequestsPage;
