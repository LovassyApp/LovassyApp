import {
    ActionIcon,
    Button,
    Center,
    Divider,
    Group,
    Loader,
    Modal,
    MultiSelect,
    NumberInput,
    SegmentedControl,
    SimpleGrid,
    Text,
    TextInput,
    Textarea,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconBell, IconCheck, IconX } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    useDeleteApiLoloRequestsId,
    useGetApiLoloRequests,
    usePatchApiLoloRequestsId,
    usePostApiLoloRequestsOverruleId,
} from "../../../api/generated/features/lolo-requests/lolo-requests";
import { useEffect, useMemo, useState } from "react";
import {
    useGetApiLoloRequestCreatedNotifiers,
    usePutApiLoloRequestCreatedNotifiers,
} from "../../../api/generated/features/lolo-request-created-notifiers/lolo-request-created-notifiers";

import { LoloRequestCard } from "../components/loloRequestCard";
import { LoloRequestStats } from "../components/loloRequestStats";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { ShopIndexLoloRequestsResponse } from "../../../api/generated/models";
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

const NotifiersModal = ({ opened, close }: { opened: boolean; close(): void }): JSX.Element => {
    const { classes } = useStyles();

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const notifiersQueryEnabled = useMemo(
        () =>
            (control.data?.permissions?.includes("Shop.IndexLoloRequestCreatedNotifiers") ||
                control.data?.isSuperUser) ??
            false,
        [control]
    );

    const notifiers = useGetApiLoloRequestCreatedNotifiers({}, { query: { enabled: notifiersQueryEnabled } });
    const updateNotifiers = usePutApiLoloRequestCreatedNotifiers();

    const [creatableNotifiersData, setCreatableNotifiersData] = useState([]);
    const [selectedNotifiers, setSelectedNotifiers] = useState([]);

    const form = useForm({
        initialValues: {
            emails: [],
        },
    });

    useEffect(() => {
        if (!notifiers.data) return;

        const notifiersData = notifiers.data.map((notifier) => ({
            label: notifier.email,
            value: notifier.email,
        }));

        setCreatableNotifiersData(notifiersData);
        form.setFieldValue(
            "emails",
            notifiersData.map((notifier) => notifier.value)
        );
    }, [notifiers.data]);

    if (notifiers.isLoading)
        return (
            <Modal opened={opened} onClose={close} size="lg" title="Értesítők">
                <Center className={classes.center}>
                    <Loader />
                </Center>
            </Modal>
        );

    const submit = form.onSubmit(async (values) => {
        try {
            await updateNotifiers.mutateAsync({ data: values });
            notifications.show({
                title: "Értesítők frissítve",
                message: "Az értesítőket sikeresen frissítetted.",
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
        <Modal opened={opened} onClose={close} size="lg" title="Értesítők">
            <form onSubmit={submit}>
                <MultiSelect
                    label="Értesítendő email címek"
                    description={
                        !notifiersQueryEnabled &&
                        "Figyelem! Az értesítők módosításával minden eddigi értesítőt felülírsz."
                    }
                    data={creatableNotifiersData}
                    {...form.getInputProps("emails")}
                    withinPortal={true}
                    searchable={true}
                    clearable={true}
                    creatable={true}
                    required={true}
                    getCreateLabel={(value) => `+ Email hozzáadása: ${value}`}
                    onCreate={(query) => {
                        setCreatableNotifiersData([...creatableNotifiersData, { label: query, value: query }]);
                        return { label: query, value: query };
                    }}
                />
                <Button type="submit" loading={updateNotifiers.isLoading} fullWidth={true} mt="md">
                    Mentés
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
    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const userQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Users.ViewUser") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const user = useGetApiUsersId(loloRequest?.userId, { query: { enabled: userQueryEnabled && !!loloRequest } });

    const updateLoloRequest = usePatchApiLoloRequestsId();
    const overruleLoloRequest = usePostApiLoloRequestsOverruleId();
    const deleteLoloRequest = useDeleteApiLoloRequestsId();

    useEffect(() => {
        updateForm.setValues({
            title: loloRequest?.title,
            body: loloRequest?.body,
        }); // If we don't do this, the form will be one version behind after an update
    }, [loloRequest, opened]);

    const updateForm = useForm({
        initialValues: {
            title: loloRequest?.title,
            body: loloRequest?.body,
        },
    });

    const updateSubmit = updateForm.onSubmit(async (values) => {
        try {
            await updateLoloRequest.mutateAsync({ data: values, id: loloRequest.id });
            notifications.show({
                title: "Kérvény módosítva",
                message: "A kérvényt sikeresen módosítottad.",
                color: "green",
                icon: <IconCheck />,
            });
            updateForm.reset();
            close();
        } catch (err) {
            if (err instanceof ValidationError) {
                handleValidationErrors(err, updateForm);
            }
        }
    });

    const overruleForm = useForm({
        initialValues: {
            accepted: false,
            loloAmount: 0,
        },
    });

    const overruleSubmit = overruleForm.onSubmit(async (values) => {
        try {
            await overruleLoloRequest.mutateAsync({ data: values, id: loloRequest.id });
            notifications.show({
                title: "Kérvény elbírálva",
                message: "A kérvényt sikeresen elbíráltad.",
                color: "green",
                icon: <IconCheck />,
            });
            overruleForm.reset();
            close();
        } catch (err) {
            if (err instanceof ValidationError) {
                handleValidationErrors(err, overruleForm);
            }
        }
    });

    const doDeleteLoloRequest = async () => {
        try {
            await deleteLoloRequest.mutateAsync({ id: loloRequest.id });
            notifications.show({
                title: "Kérvény törölve",
                message: "A kérvényt sikeresen törölted.",
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
            <Modal
                opened={opened}
                onClose={() => {
                    overruleForm.reset();
                    close();
                }}
                size="lg"
                title="Részletek"
            >
                <Center>
                    <Loader />
                </Center>
            </Modal>
        );

    return (
        <Modal
            opened={opened}
            onClose={() => {
                overruleForm.reset();
                close();
            }}
            size="lg"
            title="Részletek"
        >
            <Group position="apart" spacing={0}>
                <Text>Cím:</Text>
                <Text weight="bold">{loloRequest?.title}</Text>
            </Group>
            <Text>Törzsszöveg:</Text>
            <Text weight="bold">{loloRequest?.body}</Text>
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
                    <PermissionRequirement permissions={["Shop.UpdateLoloRequest"]}>
                        <Divider my="sm" />
                        <form onSubmit={updateSubmit}>
                            <TextInput required={true} label="Cím" {...updateForm.getInputProps("title")} />
                            <Textarea
                                required={true}
                                label="Törzsszöveg"
                                mt="sm"
                                {...updateForm.getInputProps("body")}
                            />
                            <Button type="submit" fullWidth={true} mt="md" loading={updateLoloRequest.isLoading}>
                                Módosítás
                            </Button>
                        </form>
                    </PermissionRequirement>
                    <PermissionRequirement permissions={["Shop.OverruleLoloRequest"]}>
                        <Divider my="sm" />
                        <form onSubmit={overruleSubmit}>
                            <SegmentedControl
                                data={[
                                    { label: "Elutasítás", value: "false" },
                                    { label: "Elfogadás", value: "true" },
                                ]}
                                fullWidth={true}
                                value={overruleForm.values.accepted ? "true" : "false"}
                                onChange={(value) => overruleForm.setFieldValue("accepted", value === "true")}
                            />
                            <NumberInput
                                label="Lolo mennyiség"
                                min={0}
                                required={true}
                                {...overruleForm.getInputProps("loloAmount")}
                                mt="xs"
                            />
                            <Button type="submit" fullWidth={true} mt="md" loading={overruleLoloRequest.isLoading}>
                                Elbírálás
                            </Button>
                        </form>
                    </PermissionRequirement>
                    <PermissionRequirement permissions={["Shop.DeleteLoloRequest"]}>
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

const LoloRequestsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const loloRequests = useGetApiLoloRequests();

    const [notifiersModalOpened, { close: closeNotifiersModal, open: openNotifiersModal }] = useDisclosure(false);
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
            <NotifiersModal opened={notifiersModalOpened} close={closeNotifiersModal} />
            <DetailsModal opened={detailsModalOpened} close={closeDetailsModal} loloRequest={detailsModalLoloRequest} />
            <Title mb="md">Összevont statisztikák</Title>
            <SimpleGrid cols={2} breakpoints={[{ maxWidth: theme.breakpoints.sm, cols: 1, spacing: "sm" }]}>
                <LoloRequestStats data={loloRequests.data} />
            </SimpleGrid>
            <Group position="apart" align="baseline" my="md" spacing={0}>
                <Title>Összes kérvény</Title>
                <PermissionRequirement permissions={["Shop.UpdateLoloRequestCreatedNotifiers"]}>
                    <ActionIcon variant="transparent" color="dark" onClick={() => openNotifiersModal()}>
                        <IconBell />
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
                    Úgy néz ki még nincsenek kérvények... Amint egy felhasználó létrehoz egy kérvényt, itt látod majd a
                    őket.
                </Text>
            )}
        </>
    );
};

export default LoloRequestsPage;
