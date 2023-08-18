import {
    ActionIcon,
    Button,
    Center,
    Divider,
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
import { AuthIndexPermissionsResponse, AuthIndexUserGroupsResponse } from "../../../api/generated/models";
import { IconCheck, IconPlus, IconX } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    useDeleteApiUserGroupsId,
    useGetApiUserGroups,
    usePatchApiUserGroupsId,
    usePostApiUserGroups,
} from "../../../api/generated/features/user-groups/user-groups";
import { useEffect, useMemo, useState } from "react";

import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { UserGroupCard } from "../components/userGroupCard";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useGetApiPermissions } from "../../../api/generated/features/permissions/permissions";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

const CreateUserGrouptModal = ({ opened, close }: { opened: boolean; close(): void }): JSX.Element => {
    const theme = useMantineTheme();

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const permissionsQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Auth.IndexPermissions") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const permissions = useGetApiPermissions({}, { query: { enabled: permissionsQueryEnabled } });

    const createUserGroup = usePostApiUserGroups();

    const [permissionsMap, setPermissionsMap] = useState<Map<string, boolean> | undefined>(undefined);

    const groupedPermissions = useMemo(() => {
        if (!permissions.data) return undefined;

        const map = new Map<string, AuthIndexPermissionsResponse[]>();

        for (const permission of permissions.data) {
            const [group] = permission.name.split(".");

            if (!map.has(group)) map.set(group, []);

            map.get(group)?.push(permission);
        }

        return map;
    }, [permissions.data]);

    useEffect(() => {
        if (permissions.data) {
            const map = new Map<string, boolean>();
            for (const permission of permissions.data) {
                map.set(permission.name, false);
            }
            setPermissionsMap(map);
        }
    }, [permissions.data]);

    const permissionSwitches = useMemo(() => {
        if (!permissionsMap || !groupedPermissions) return undefined;

        const disbaled = !control.data?.permissions?.includes("Auth.UpdateUserGroup") && !control.data?.isSuperUser;
        const switches: JSX.Element[] = [];

        for (const [group, permissions] of groupedPermissions) {
            switches.push(
                <Text key={`${group}_label`} weight="bold" my="sm">
                    {group}
                </Text>
            );
            switches.push(
                <SimpleGrid
                    key={`${group}_grid`}
                    cols={2}
                    breakpoints={[{ maxWidth: theme.breakpoints.xs, cols: 1 }]}
                    spacing="sm"
                >
                    {permissions.map((permission) => (
                        <Switch
                            key={permission.name}
                            label={permission.displayName}
                            description={permission.description}
                            checked={permissionsMap.get(permission.name) ?? false}
                            disabled={disbaled}
                            color={permission.dangerous ? "red" : "green"}
                            onChange={() => {
                                if (permissionsMap) {
                                    const map = new Map(permissionsMap);
                                    map.set(permission.name, !map.get(permission.name));
                                    setPermissionsMap(map);
                                }
                            }}
                        />
                    ))}
                </SimpleGrid>
            );
        }

        return switches;
    }, [groupedPermissions, permissionsMap]);

    const form = useForm({
        initialValues: {
            name: "",
        },
    });

    const submit = form.onSubmit(async (values) => {
        const newPermissions = [];

        for (const [permission, checked] of permissionsMap ?? []) {
            if (checked) newPermissions.push(permission);
        }

        try {
            await createUserGroup.mutateAsync({
                data: { name: values.name, permissions: newPermissions },
            });
            notifications.show({
                title: "Csoport módosítva",
                message: "A felhasználói csoportot sikeresen módosítottad.",
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

    if (permissions.isInitialLoading)
        return (
            <Modal opened={opened} onClose={close} title="Új csoport" size="lg">
                <Center>
                    <Loader />
                </Center>
            </Modal>
        );

    return (
        <Modal opened={opened} onClose={close} size="lg" title="Új csoport">
            <form onSubmit={submit}>
                <TextInput label="Név" required={true} {...form.getInputProps("name")} />
                {permissions.data && (!permissionsMap || !groupedPermissions) && (
                    <Center>
                        <Loader />
                    </Center>
                )}
                {permissions.data && permissionsMap && groupedPermissions && permissionSwitches}
                <Button type="submit" fullWidth={true} color="blue" mt="md" loading={createUserGroup.isLoading}>
                    Létrehozás
                </Button>
            </form>
        </Modal>
    );
};

const DetailsModal = ({
    userGroup,
    opened,
    close,
}: {
    userGroup: AuthIndexUserGroupsResponse;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const theme = useMantineTheme();

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const permissionsQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Auth.IndexPermissions") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const permissions = useGetApiPermissions({}, { query: { enabled: permissionsQueryEnabled && !!userGroup } });

    const updateUserGroup = usePatchApiUserGroupsId();
    const deleteUserGroup = useDeleteApiUserGroupsId();

    const [permissionsMap, setPermissionsMap] = useState<Map<string, boolean> | undefined>(undefined);

    const groupedPermissions = useMemo(() => {
        if (!permissions.data) return undefined;

        const map = new Map<string, AuthIndexPermissionsResponse[]>();

        for (const permission of permissions.data) {
            const [group] = permission.name.split(".");

            if (!map.has(group)) map.set(group, []);

            map.get(group)?.push(permission);
        }

        return map;
    }, [permissions.data]);

    useEffect(() => {
        if (permissions.data && userGroup && opened) {
            const map = new Map<string, boolean>();
            for (const permission of permissions.data) {
                map.set(permission.name, userGroup.permissions.includes(permission.name));
            }
            setPermissionsMap(map);
        }
    }, [permissions.data, userGroup, opened]);

    useEffect(() => {
        if (!opened) setPermissionsMap(undefined);
    }, [opened]);

    const permissionSwitches = useMemo(() => {
        if (!permissionsMap || !groupedPermissions) return undefined;

        const disbaled = !control.data?.permissions?.includes("Auth.UpdateUserGroup") && !control.data?.isSuperUser;
        const switches: JSX.Element[] = [];

        for (const [group, permissions] of groupedPermissions) {
            switches.push(
                <Text key={`${group}_label`} weight="bold" my="sm">
                    {group}
                </Text>
            );
            switches.push(
                <SimpleGrid
                    key={`${group}_grid`}
                    cols={2}
                    breakpoints={[{ maxWidth: theme.breakpoints.xs, cols: 1 }]}
                    spacing="sm"
                >
                    {permissions.map((permission) => (
                        <Switch
                            key={permission.name}
                            label={permission.displayName}
                            description={permission.description}
                            checked={permissionsMap.get(permission.name) ?? false}
                            disabled={disbaled}
                            color={permission.dangerous ? "red" : "green"}
                            onChange={() => {
                                if (permissionsMap) {
                                    const map = new Map(permissionsMap);
                                    map.set(permission.name, !map.get(permission.name));
                                    setPermissionsMap(map);
                                }
                            }}
                        />
                    ))}
                </SimpleGrid>
            );
        }

        return switches;
    }, [groupedPermissions, permissionsMap]);

    useEffect(() => {
        form.setValues({
            name: userGroup?.name,
        }); // If we don't do this, the form will be one version behind after an update
    }, [userGroup, opened]);

    const form = useForm({
        initialValues: {
            name: userGroup?.name,
        },
    });

    const submit = form.onSubmit(async (values) => {
        const newPermissions = [];

        for (const [permission, checked] of permissionsMap ?? []) {
            if (checked) newPermissions.push(permission);
        }

        try {
            await updateUserGroup.mutateAsync({
                id: userGroup.id,
                data: { name: values.name, permissions: newPermissions },
            });
            notifications.show({
                title: "Csoport módosítva",
                message: "A felhasználói csoportot sikeresen módosítottad.",
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

    const doDeleteUserGroup = async () => {
        try {
            await deleteUserGroup.mutateAsync({ id: userGroup.id });
            notifications.show({
                title: "Csoport törölve",
                message: "A felhasználói csoportot sikeresen törölted.",
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

    if (permissions.isInitialLoading)
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
                <Text weight="bold">{userGroup?.name}</Text>
            </Group>
            {!permissionsQueryEnabled && (
                <>
                    <Text>Jogosultságok:</Text>
                    <Text weight="bold">{userGroup?.permissions.join(", ")}</Text>
                </>
            )}
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Létrehozás dátuma:</Text>
                <Text weight="bold">{new Date(userGroup?.createdAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Módosítás dátuma:</Text>
                <Text weight="bold">{new Date(userGroup?.updatedAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            {permissions.data && (!permissionsMap || !groupedPermissions) && (
                <>
                    <Divider my="sm" />
                    <Center>
                        <Loader />
                    </Center>
                </>
            )}
            {permissions.data && permissionsMap && groupedPermissions && (
                <PermissionRequirement
                    permissions={["Auth.UpdateUserGroup"]}
                    fallback={
                        <>
                            <Divider my="sm" />
                            {permissionSwitches}
                        </>
                    }
                >
                    <Divider my="sm" />
                    <form onSubmit={submit}>
                        <TextInput label="Név" required={true} {...form.getInputProps("name")} />
                        {permissionSwitches}
                        <Button type="submit" fullWidth={true} color="blue" mt="md" loading={updateUserGroup.isLoading}>
                            Mentés
                        </Button>
                    </form>
                </PermissionRequirement>
            )}
            <PermissionRequirement permissions={["Auth.DeleteUserGroup"]}>
                <Divider my="sm" />
                <Button
                    fullWidth={true}
                    color="red"
                    onClick={async () => await doDeleteUserGroup()}
                    loading={deleteUserGroup.isLoading}
                >
                    Törlés
                </Button>
            </PermissionRequirement>
        </Modal>
    );
};

const UserGroupsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const userGroups = useGetApiUserGroups();

    const [createUserGroupModalOpened, { close: closeCreateUserGroupModal, open: openCreateUserGroupModal }] =
        useDisclosure(false);
    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalUserGroup, setDetailsModalUserGroup] = useState<AuthIndexUserGroupsResponse>();

    if (userGroups.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (userGroups.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <CreateUserGrouptModal opened={createUserGroupModalOpened} close={closeCreateUserGroupModal} />
            <DetailsModal userGroup={detailsModalUserGroup} opened={detailsModalOpened} close={closeDetailsModal} />
            <Group position="apart" align="baseline" mb="md" spacing={0}>
                <Title>Csoportok</Title>
                <PermissionRequirement permissions={["Auth.CreateUserGroup"]}>
                    <ActionIcon variant="transparent" color="dark" onClick={() => openCreateUserGroupModal()}>
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
                {userGroups.data.map((userGroup) => (
                    <UserGroupCard
                        key={userGroup.id}
                        userGroup={userGroup}
                        openDetails={(userGroup) => {
                            setDetailsModalUserGroup(userGroup);
                            openDetailsModal();
                        }}
                    />
                ))}
            </SimpleGrid>
        </>
    );
};

export default UserGroupsPage;
