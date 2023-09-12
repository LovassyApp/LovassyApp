import "dayjs/locale/hu";

import {
    ActionIcon,
    Button,
    Center,
    Divider,
    Drawer,
    Group,
    Loader,
    Modal,
    MultiSelect,
    Select,
    SimpleGrid,
    Text,
    TextInput,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconFilter, IconSearch, IconTrash, IconX } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    getGetApiUsersIdQueryKey,
    getGetApiUsersQueryKey,
    useDeleteApiUsersId,
    useGetApiUsers,
    useGetApiUsersId,
    usePatchApiUsersId,
} from "../../../api/generated/features/users/users";
import { useEffect, useMemo, useState } from "react";

import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { UserCard } from "../componets/userCard";
import { UsersIndexUsersResponse } from "../../../api/generated/models";
import dayjs from "dayjs";
import { notifications } from "@mantine/notifications";
import relativeTime from "dayjs/plugin/relativeTime";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useGetApiUserGroups } from "../../../api/generated/features/user-groups/user-groups";
import { useQueryClient } from "@tanstack/react-query";

dayjs.extend(relativeTime);

const useStyles = createStyles(() => ({
    center: {
        height: "100%",
    },
}));

interface UsersParams {
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
    params: UsersParams;
    setParams(arg0: UsersParams): void;
}): JSX.Element => {
    const [nameSearch, setNameSearch] = useState<string>(
        params.Filters?.split(",")
            .find((str) => str.startsWith("Name@=*"))
            ?.replace(/^(Name@=\*)/, "") ?? ""
    );
    const [realNameSearch, setRealNameSearch] = useState<string>(
        params.Filters?.split(",")
            .find((str) => str.startsWith("RealName@=*"))
            ?.replace(/^(RealName@=\*)/, "") ?? ""
    );
    const [classSearch, setClassSearch] = useState<string>(
        params.Filters?.split(",")
            .find((str) => str.startsWith("Class@=*"))
            ?.replace(/^(Class@=\*)/, "") ?? ""
    );

    const doSetParams = () => {
        const filters = [];

        if (nameSearch !== "") filters.push(`Name@=*${nameSearch}`);
        if (realNameSearch !== "") filters.push(`RealName@=*${realNameSearch}`);
        if (classSearch !== "") filters.push(`Class@=*${classSearch}`);

        setParams({ Filters: filters.join(",") });
        close();
    };

    return (
        <Drawer opened={opened} onClose={close} title="Szűrés" position="right">
            <TextInput
                label="Keresés név alapján"
                value={nameSearch}
                icon={<IconSearch size={20} stroke={1.5} />}
                rightSection={
                    nameSearch !== "" && (
                        <ActionIcon variant="transparent" color="dark" onClick={() => setNameSearch("")}>
                            <IconX size={20} stroke={1.5} />
                        </ActionIcon>
                    )
                }
                onChange={(event) => setNameSearch(event.currentTarget.value)}
            />
            <TextInput
                label="Keresés valódi név alapján"
                value={realNameSearch}
                icon={<IconSearch size={20} stroke={1.5} />}
                rightSection={
                    realNameSearch !== "" && (
                        <ActionIcon variant="transparent" color="dark" onClick={() => setRealNameSearch("")}>
                            <IconX size={20} stroke={1.5} />
                        </ActionIcon>
                    )
                }
                onChange={(event) => setRealNameSearch(event.currentTarget.value)}
                mt="sm"
            />
            <TextInput
                label="Keresés osztály alapján"
                value={classSearch}
                icon={<IconSearch size={20} stroke={1.5} />}
                rightSection={
                    classSearch !== "" && (
                        <ActionIcon variant="transparent" color="dark" onClick={() => setClassSearch("")}>
                            <IconX size={20} stroke={1.5} />
                        </ActionIcon>
                    )
                }
                onChange={(event) => setClassSearch(event.currentTarget.value)}
                mt="sm"
            />
            <Button onClick={() => doSetParams()} fullWidth={true} variant="outline" mt="md">
                Gyerünk!
            </Button>
        </Drawer>
    );
};

const DetailsModal = ({
    user,
    opened,
    close,
}: {
    user: UsersIndexUsersResponse;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const queryClient = useQueryClient();

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already
    const detailedQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Users.ViewUser") || control.data?.isSuperUser) ?? false,
        [control]
    );
    const groupsQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Auth.IndexUserGroups") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const detailedUser = useGetApiUsersId(user?.id, { query: { enabled: detailedQueryEnabled && !!user } });
    const groups = useGetApiUserGroups({}, { query: { enabled: groupsQueryEnabled && !!user } });

    const updateUser = usePatchApiUsersId();
    const deleteUser = useDeleteApiUsersId();

    const usersQueryKey = getGetApiUsersQueryKey();
    const detailedUserQueryKey = getGetApiUsersIdQueryKey(user?.id);

    const form = useForm({
        initialValues: {
            name: user?.name,
            email: user?.email,
            userGroups: user?.userGroups.map((group) => group.id.toString()),
        },
    });

    useEffect(() => {
        form.setValues({
            name: user?.name,
            email: user?.email,
            userGroups: user?.userGroups.map((group) => group.id.toString()),
        });
    }, [user, opened]);

    const submit = form.onSubmit(async (values) => {
        try {
            await updateUser.mutateAsync({
                data: { ...values, userGroups: values.userGroups.map((id) => +id) },
                id: user.id,
            });
            await queryClient.invalidateQueries({ queryKey: [usersQueryKey[0]] });
            await queryClient.invalidateQueries({ queryKey: [detailedUserQueryKey[0]] });
            notifications.show({
                title: "Felhasználó módosítva",
                message: "A felhasználót sikeresen módosítottad.",
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

    const doDeleteUser = async () => {
        try {
            await deleteUser.mutateAsync({ id: user.id });
            await queryClient.invalidateQueries({ queryKey: [usersQueryKey[0]] });
            notifications.show({
                title: "Felhasználó törölve",
                message: "A felhasználót sikeresen törölted.",
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

    const groupsData = useMemo(
        () => groups.data?.map((group) => ({ value: group.id.toString(), label: group.name })),
        [groups]
    );

    const displayedUserGroups = useMemo(() => {
        if (groupsData) {
            return (
                <MultiSelect label="Felhasználói csoportok" data={groupsData} {...form.getInputProps("userGroups")} />
            );
        }
    }, [form, groupsData]);

    if (detailedUser.isInitialLoading || groups.isInitialLoading)
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
                <Text weight="bold">{user?.name}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Valódi név:</Text>
                <Text weight="bold">{user?.realName ?? "Ismeretlen"}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Osztály:</Text>
                <Text weight="bold">{user?.class ?? "Ismeretlen"}</Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Email:</Text>
                <Text weight="bold">{user?.email}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Email megerősítve:</Text>
                <Text weight="bold">
                    {user?.emailVerifiedAt ? new Date(user?.emailVerifiedAt).toLocaleDateString("hu-HU", {}) : "Nem"}
                </Text>
            </Group>
            {detailedUser.data && (
                <>
                    <Divider my="sm" />
                    <Group position="apart" spacing={0}>
                        <Text>Utoljára online:</Text>
                        <Text weight="bold">
                            {detailedUser.data.lastOnlineAt
                                ? dayjs(detailedUser.data.lastOnlineAt).locale("hu").fromNow()
                                : "Ismeretlen"}
                        </Text>
                    </Group>
                </>
            )}
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Csoportok:</Text>
                <Text weight="bold">{user?.userGroups.map((group) => group.name).join(", ")}</Text>
            </Group>
            {detailedUser.data && (
                <>
                    <Text>Jogosultságok:</Text>
                    <Text weight="bold">
                        {detailedUser?.data.userGroups
                            .flatMap((group) => group.permissions)
                            .filter((value, index, array) => array.indexOf(value) === index)
                            .join(", ")}
                    </Text>
                </>
            )}
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Létrehozás dátuma:</Text>
                <Text weight="bold">{new Date(user?.createdAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Módosítás dátuma:</Text>
                <Text weight="bold">{new Date(user?.updatedAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            <PermissionRequirement permissions={["Users.UpdateUser"]}>
                <Divider my="sm" />
                <form onSubmit={submit}>
                    <TextInput required={true} label="Név" {...form.getInputProps("name")} />
                    <TextInput required={true} label="Email" {...form.getInputProps("email")} mt="sm" />
                    <PermissionRequirement permissions={["Auth.IndexUserGroups"]}>
                        <Divider my="sm" />
                        {groupsData && (
                            <MultiSelect
                                label="Felhasználói csoportok"
                                data={groupsData}
                                {...form.getInputProps("userGroups")}
                            />
                        )}
                    </PermissionRequirement>

                    <Button type="submit" fullWidth={true} mt="md" loading={updateUser.isLoading}>
                        Módosítás
                    </Button>
                </form>
            </PermissionRequirement>
            <PermissionRequirement permissions={["Users.DeleteUser"]}>
                <Divider my="sm" />
                <Button
                    fullWidth={true}
                    color="red"
                    onClick={async () => await doDeleteUser()}
                    loading={deleteUser.isLoading}
                    disabled={control.data?.user.id === user?.id}
                >
                    Törlés
                </Button>
            </PermissionRequirement>
        </Modal>
    );
};

const UsersPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const [params, setParams] = useState<UsersParams>({});

    const users = useGetApiUsers({ Filters: params.Filters ?? "", Sorts: "Name" });

    const [filtersDrawerOpened, { open: openFiltersDrawer, close: closeFiltersDrawer }] = useDisclosure(false);
    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalUser, setDetailsModalUser] = useState<UsersIndexUsersResponse>();

    if (users.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (users.isError)
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
            <DetailsModal user={detailsModalUser} opened={detailsModalOpened} close={closeDetailsModal} />
            <Group position="apart" align="baseline" mb="md" spacing={0}>
                <Title>Felhasználók</Title>
                <Group align="baseline">
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
                {users.data?.map((user) => (
                    <UserCard
                        key={user.id}
                        user={user}
                        openDetails={() => {
                            setDetailsModalUser(user);
                            openDetailsModal();
                        }}
                    />
                ))}
            </SimpleGrid>
        </>
    );
};

export default UsersPage;
