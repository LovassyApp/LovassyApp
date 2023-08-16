import {
    Button,
    Center,
    Divider,
    Group,
    Loader,
    Modal,
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
import { IconCheck, IconX } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    getGetApiLoloRequestsOwnQueryKey,
    getGetApiLoloRequestsQueryKey,
    useDeleteApiLoloRequestsId,
    useGetApiLoloRequests,
    usePatchApiLoloRequestsId,
    usePostApiLoloRequestsOverruleId,
} from "../../../api/generated/features/lolo-requests/lolo-requests";
import { useEffect, useMemo, useState } from "react";

import { LoloRequestCard } from "../components/loloRequestCard";
import { LoloRequestStats } from "../components/loloRequestStats";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { ShopIndexLoloRequestsResponse } from "../../../api/generated/models";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useGetApiUsersId } from "../../../api/generated/features/users/users";
import { useQueryClient } from "@tanstack/react-query";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

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
    const overruleLoloRequest = usePostApiLoloRequestsOverruleId();
    const deleteLoloRequest = useDeleteApiLoloRequestsId();
    const queryClient = useQueryClient();

    const ownQueryKey = getGetApiLoloRequestsOwnQueryKey();
    const allQueryKey = getGetApiLoloRequestsQueryKey();

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const userQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Users.ViewUser") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const user = useGetApiUsersId(loloRequest?.userId, { query: { enabled: userQueryEnabled && !!loloRequest } });

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
            await queryClient.invalidateQueries({ queryKey: [ownQueryKey[0]] });
            await queryClient.invalidateQueries({ queryKey: [allQueryKey[0]] });
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
            await queryClient.invalidateQueries({ queryKey: [ownQueryKey[0]] });
            await queryClient.invalidateQueries({ queryKey: [allQueryKey[0]] });
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
            await queryClient.invalidateQueries({ queryKey: [ownQueryKey[0]] });
            await queryClient.invalidateQueries({ queryKey: [allQueryKey[0]] });
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
                title="Részletek"
                size="lg"
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
            <DetailsModal opened={detailsModalOpened} close={closeDetailsModal} loloRequest={detailsModalLoloRequest} />
            <Title mb="md">Összevont statisztikák</Title>
            <SimpleGrid cols={2} breakpoints={[{ maxWidth: theme.breakpoints.sm, cols: 1, spacing: "sm" }]}>
                <LoloRequestStats data={loloRequests.data} />
            </SimpleGrid>
            <Title my="md">Összes kérvény</Title>
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
        </>
    );
};

export default LoloRequestsPage;
