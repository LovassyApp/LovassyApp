import {
    ActionIcon,
    Box,
    Button,
    Center,
    Divider,
    Group,
    Loader,
    Modal,
    NumberInput,
    Select,
    SimpleGrid,
    Stack,
    Switch,
    Text,
    TextInput,
    Title,
    createStyles,
    rem,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconPlus, IconTrash, IconX } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    getGetApiImageVotingsIdQueryKey,
    useDeleteApiImageVotingsId,
    useGetApiImageVotings,
    useGetApiImageVotingsId,
    usePatchApiImageVotingsId,
    usePostApiImageVotings,
} from "../../../api/generated/features/image-votings/image-votings";
import { useEffect, useMemo, useState } from "react";
import { useGetApiUserGroups, useGetApiUserGroupsId } from "../../../api/generated/features/user-groups/user-groups";

import { ImageVotingsIndexImageVotingsResponse } from "../../../api/generated/models";
import { Link } from "react-router-dom";
import { ManageImageVotingCard } from "../components/manageImageVotingCard";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useQueryClient } from "@tanstack/react-query";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

const CreateImageVotingModal = ({ opened, close }: { opened: boolean; close(): void }): JSX.Element => {
    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const userGroupsQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Auth.IndexUserGroups") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const userGroups = useGetApiUserGroups({}, { query: { enabled: userGroupsQueryEnabled } });

    const userGroupsData = useMemo(
        () =>
            userGroups.data?.map((userGroup) => ({
                value: userGroup.id.toString(),
                label: userGroup.name,
            })),
        [userGroups]
    );

    const createImageVoting = usePostApiImageVotings();

    const form = useForm({
        initialValues: {
            name: "",
            description: "",
            type: "SingleChoice",
            aspects: [],
            active: false,
            showUploaderInfo: false,
            uploaderUserGroupId: "",
            bannedUserGroupId: "",
            maxUploadsPerUser: 1,
            superIncrementAllowed: true,
            superIncrementValue: 2,
        },
        transformValues: (values) => ({
            ...values,
            uploaderUserGroupId: +values.uploaderUserGroupId,
            bannedUserGroupId: values.bannedUserGroupId ? +values.bannedUserGroupId : undefined,
        }),
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await createImageVoting.mutateAsync({ data: values });
            notifications.show({
                title: "Szavazás létrehozva",
                message: "A kép szavazást sikeresen létrehoztad.",
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

    const displayedAspects = useMemo(
        () =>
            form.values.aspects.map((aspect, index) => (
                <Stack key={aspect.key} spacing={0} mb="xs" w="100%">
                    <TextInput
                        label="Név"
                        required={true}
                        {...form.getInputProps(`aspects.${index}.name`)}
                        sx={{ flex: 1 }}
                    />
                    <TextInput
                        label="Leírás"
                        required={true}
                        {...form.getInputProps(`aspects.${index}.description`)}
                        mt={rem(4)}
                    />
                    <Button
                        variant="light"
                        color="red"
                        onClick={() => form.removeListItem("aspects", index)}
                        mt="sm"
                        leftIcon={<IconTrash stroke={1.5} />}
                    >
                        Törlés
                    </Button>
                </Stack>
            )),
        [form]
    );

    if (userGroups.isLoading)
        return (
            <Modal opened={opened} onClose={close} title="Új szavazás" size="lg">
                <Center>
                    <Loader />
                </Center>
            </Modal>
        );

    return (
        <Modal opened={opened} onClose={close} title="Új szavazás" size="lg">
            <form onSubmit={submit}>
                <TextInput label="Név" required={true} {...form.getInputProps("name")} />
                <TextInput label="Leírás" required={true} {...form.getInputProps("description")} mt="sm" />
                <Select
                    label="Típus"
                    data={[
                        { value: "SingleChoice", label: "Választásos" },
                        { value: "Increment", label: "Inkrementáló" },
                    ]}
                    required={true}
                    {...form.getInputProps("type")}
                    withinPortal={true}
                    mt="sm"
                />
                {form.values.type === "Increment" && (
                    <>
                        <Group position="apart" spacing={0} mt="md">
                            <Text size="sm">Szuperszavazat engedélyezése</Text>
                            <Switch {...form.getInputProps("superIncrementAllowed", { type: "checkbox" })} />
                        </Group>
                        <NumberInput
                            label="Szperszavazat értéke"
                            min={1}
                            required={true}
                            {...form.getInputProps("superIncrementValue")}
                            mt="sm"
                        />
                    </>
                )}
                <Group position="apart" spacing={0} mt="md">
                    <Text size="sm">Aktív</Text>
                    <Switch {...form.getInputProps("active", { type: "checkbox" })} />
                </Group>
                {userGroupsData ? (
                    <Select
                        label="Feltöltő csoport"
                        data={userGroupsData}
                        required={true}
                        {...form.getInputProps("uploaderUserGroupId")}
                        withinPortal={true}
                        mt="sm"
                    />
                ) : (
                    <TextInput
                        label="Feltöltő csoport id"
                        required={true}
                        {...form.getInputProps("uploaderUserGroupId")}
                        mt="sm"
                    />
                )}
                {userGroupsData ? (
                    <Select
                        label="Tiltott csoport"
                        data={userGroupsData}
                        {...form.getInputProps("bannedUserGroupId")}
                        withinPortal={true}
                        clearable={true}
                        mt="sm"
                    />
                ) : (
                    <TextInput label="Tiltott csoport id" {...form.getInputProps("bannedUserGroupId")} mt="sm" />
                )}
                <Group position="apart" spacing={0} mt="md">
                    <Text size="sm">Publikus felhasználói adatok</Text>
                    <Switch {...form.getInputProps("showUploaderInfo", { type: "checkbox" })} />
                </Group>
                <NumberInput
                    label="Max kép/felhasználó"
                    min={1}
                    required={true}
                    {...form.getInputProps("maxUploadsPerUser")}
                    mt="sm"
                />
                <Text size="sm" mt="sm" weight={500}>
                    Értékelési szempontok
                </Text>
                {displayedAspects}
                <Button
                    onClick={() =>
                        form.insertListItem("aspects", {
                            name: "",
                            key: `a${Math.random().toString(36).slice(2)}`,
                            description: "",
                        })
                    }
                    variant="outline"
                    fullWidth={true}
                    mt="md"
                >
                    Szempont hozzáadása
                </Button>
                <Button type="submit" fullWidth={true} loading={createImageVoting.isLoading} mt="md">
                    Létrehozás
                </Button>
            </form>
        </Modal>
    );
};

const DetailsModal = ({
    imageVoting,
    opened,
    close,
}: {
    imageVoting: ImageVotingsIndexImageVotingsResponse;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const queryClient = useQueryClient();
    const detailedQueryKey = getGetApiImageVotingsIdQueryKey(imageVoting?.id);

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const detailedQueryEnabled = useMemo(
        () =>
            (control.data?.permissions?.includes("ImageVotings.ViewImageVoting") ||
                control.data?.isSuperUser ||
                (imageVoting?.active && control.data?.permissions?.includes("ImageVotings.ViewActiveImageVoting"))) ??
            false,
        [control]
    );

    const userGroupQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Auth.ViewUserGroup") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const userGroupsQueryEnabled = useMemo(
        () => (control.data?.permissions?.includes("Auth.IndexUserGroups") || control.data?.isSuperUser) ?? false,
        [control]
    );

    const imageVotingDetailed = useGetApiImageVotingsId(imageVoting?.id, {
        query: { enabled: detailedQueryEnabled && !!imageVoting },
    });

    const uploaderUserGroup = useGetApiUserGroupsId(imageVoting?.uploaderUserGroupId, {
        query: { enabled: userGroupQueryEnabled && !!imageVoting },
    });

    const bannedUserGroup = useGetApiUserGroupsId(imageVoting?.bannedUserGroupId, {
        query: { enabled: userGroupQueryEnabled && !!imageVoting && !!imageVoting?.bannedUserGroupId },
    });

    const userGroups = useGetApiUserGroups(
        {},
        {
            query: { enabled: userGroupsQueryEnabled && !!imageVoting },
        }
    );

    const userGroupsData = useMemo(
        () =>
            userGroups.data?.map((userGroup) => ({
                value: userGroup.id.toString(),
                label: userGroup.name,
            })),
        [userGroups]
    );

    const deleteImageVoting = useDeleteApiImageVotingsId();
    const updateImageVoting = usePatchApiImageVotingsId();

    const form = useForm({
        initialValues: {
            name: imageVoting?.name,
            description: imageVoting?.description,
            type: imageVoting?.type,
            aspects:
                imageVotingDetailed.data?.aspects.map((aspect) => ({
                    key: aspect.key,
                    name: aspect.name,
                    description: aspect.description,
                })) ?? [],
            active: imageVoting?.active,
            showUploaderInfo: imageVoting?.showUploaderInfo,
            uploaderUserGroupId: imageVoting?.uploaderUserGroupId.toString(),
            bannedUserGroupId: imageVoting?.bannedUserGroupId?.toString(),
            maxUploadsPerUser: imageVoting?.maxUploadsPerUser,
            superIncrementAllowed: imageVoting?.superIncrementAllowed,
            superIncrementValue: imageVoting?.superIncrementValue,
        },
        transformValues: (values) => ({
            ...values,
            uploaderUserGroupId: +values.uploaderUserGroupId,
            bannedUserGroupId: values.bannedUserGroupId ? +values.bannedUserGroupId : undefined,
        }),
    });

    useEffect(() => {
        form.setValues({
            name: imageVoting?.name,
            description: imageVoting?.description,
            type: imageVoting?.type,
            aspects:
                imageVotingDetailed.data?.aspects.map((aspect) => ({
                    key: aspect.key,
                    name: aspect.name,
                    description: aspect.description,
                })) ?? [],
            active: imageVoting?.active,
            showUploaderInfo: imageVoting?.showUploaderInfo,
            uploaderUserGroupId: imageVoting?.uploaderUserGroupId.toString(),
            bannedUserGroupId: imageVoting?.bannedUserGroupId?.toString(),
            maxUploadsPerUser: imageVoting?.maxUploadsPerUser,
            superIncrementAllowed: imageVoting?.superIncrementAllowed,
            superIncrementValue: imageVoting?.superIncrementValue,
        });
    }, [imageVoting, imageVotingDetailed.data, opened]);

    const submit = form.onSubmit(async (values) => {
        try {
            await updateImageVoting.mutateAsync({ id: imageVoting.id, data: values });
            await queryClient.invalidateQueries({ queryKey: [detailedQueryKey[0]] });
            notifications.show({
                title: "Szavazás módosítva",
                message: "A kép szavazást sikeresen módosítottad.",
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

    const doDeleteImageVoting = async () => {
        try {
            await deleteImageVoting.mutateAsync({ id: imageVoting.id });
            notifications.show({
                title: "Szavazás törölve",
                message: "A kép szavazást sikeresen törölted.",
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

    const displayedAspects = useMemo(
        () =>
            form.values.aspects.map((aspect, index) => (
                <Stack key={aspect.key} spacing={0} mb="xs" w="100%">
                    <TextInput
                        label="Név"
                        required={true}
                        {...form.getInputProps(`aspects.${index}.name`)}
                        sx={{ flex: 1 }}
                    />
                    <TextInput
                        label="Leírás"
                        required={true}
                        {...form.getInputProps(`aspects.${index}.description`)}
                        mt={rem(4)}
                    />
                    <Button
                        variant="light"
                        color="red"
                        onClick={() => form.removeListItem("aspects", index)}
                        mt="sm"
                        leftIcon={<IconTrash stroke={1.5} />}
                    >
                        Törlés
                    </Button>
                </Stack>
            )),
        [form]
    );

    if (
        imageVotingDetailed.isInitialLoading ||
        uploaderUserGroup.isInitialLoading ||
        bannedUserGroup.isInitialLoading ||
        userGroups.isInitialLoading
    )
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
                <Text weight="bold">{imageVoting?.name}</Text>
            </Group>
            <Text>Leírás:</Text>
            <Text weight="bold">{imageVoting?.description}</Text>
            <Group position="apart" spacing={0}>
                <Text>Típus:</Text>
                <Text weight="bold">
                    {imageVoting?.type === "SingleChoice"
                        ? "Választásos"
                        : imageVoting?.type === "Increment"
                        ? "Inkrementáló"
                        : "Ismeretlen"}
                </Text>
            </Group>
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Aktív:</Text>
                <Text weight="bold">{imageVoting?.active ? "Igen" : "Nem"}</Text>
            </Group>
            {uploaderUserGroup.data ? (
                <Group position="apart" spacing={0}>
                    <Text>Feltöltő csoport:</Text>
                    <Text weight="bold">{uploaderUserGroup.data.name}</Text>
                </Group>
            ) : (
                <Group position="apart" spacing={0}>
                    <Text>Feltőltő csoport id:</Text>
                    <Text weight="bold">{imageVoting?.uploaderUserGroupId}</Text>
                </Group>
            )}
            {imageVoting?.bannedUserGroupId && bannedUserGroup.data ? (
                <Group position="apart" spacing={0}>
                    <Text>Tiltott csoport:</Text>
                    <Text weight="bold">{bannedUserGroup.data.name}</Text>
                </Group>
            ) : (
                imageVoting?.bannedUserGroupId && (
                    <Group position="apart" spacing={0}>
                        <Text>Tiltott csoport id:</Text>
                        <Text weight="bold">{imageVoting?.bannedUserGroupId}</Text>
                    </Group>
                )
            )}
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Publikus felhasználói adatok:</Text>
                <Text weight="bold">{imageVoting?.showUploaderInfo ? "Igen" : "Nem"}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Max kép/felhasználó:</Text>
                <Text weight="bold">{imageVoting?.maxUploadsPerUser}</Text>
            </Group>
            {imageVotingDetailed.data && imageVotingDetailed.data.aspects.length > 0 && (
                <>
                    <Divider my="sm" />
                    <Text>Értékelési szempontok:</Text>
                    {imageVotingDetailed.data.aspects.map((aspect) => (
                        <Box key={aspect.key} my="xs">
                            <Text>{aspect.name}</Text>
                            <Text color="dimmed">{aspect.description}</Text>
                        </Box>
                    ))}
                </>
            )}
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Létrehozás dátuma:</Text>
                <Text weight="bold">{new Date(imageVoting?.createdAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Módosítás dátuma:</Text>
                <Text weight="bold">{new Date(imageVoting?.updatedAt).toLocaleDateString("hu-HU", {})}</Text>
            </Group>
            <PermissionRequirement
                permissions={
                    imageVoting?.active
                        ? ["ImageVotings.ViewImageVoting", "ImageVotings.ViewActiveImageVoting"]
                        : ["ImageVotings.ViewImageVoting"]
                }
            >
                <Divider my="sm" />
                <Button component={Link} to={`/image-votings/${imageVoting?.id}`} fullWidth={true} color="blue">
                    Megtekintés
                </Button>
            </PermissionRequirement>
            <PermissionRequirement permissions={["ImageVotings.UpdateImageVoting"]}>
                <Divider my="sm" />
                <form onSubmit={submit}>
                    <TextInput label="Név" required={true} {...form.getInputProps("name")} />
                    <TextInput label="Leírás" required={true} {...form.getInputProps("description")} mt="sm" />
                    <Select
                        label="Típus"
                        data={[
                            { value: "SingleChoice", label: "Választásos" },
                            { value: "Increment", label: "Inkrementáló" },
                        ]}
                        required={true}
                        {...form.getInputProps("type")}
                        withinPortal={true}
                        mt="sm"
                    />
                    {form.values.type === "Increment" && (
                        <>
                            <Group position="apart" spacing={0} mt="md">
                                <Text size="sm">Szuperszavazat engedélyezése</Text>
                                <Switch {...form.getInputProps("superIncrementAllowed", { type: "checkbox" })} />
                            </Group>
                            <NumberInput
                                label="Szperszavazat értéke"
                                min={1}
                                required={true}
                                {...form.getInputProps("superIncrementValue")}
                                mt="sm"
                            />
                        </>
                    )}
                    <Group position="apart" spacing={0} mt="md">
                        <Text size="sm">Aktív</Text>
                        <Switch {...form.getInputProps("active", { type: "checkbox" })} />
                    </Group>
                    {userGroupsData ? (
                        <Select
                            label="Feltöltő csoport"
                            data={userGroupsData}
                            required={true}
                            {...form.getInputProps("uploaderUserGroupId")}
                            withinPortal={true}
                            mt="sm"
                        />
                    ) : (
                        <TextInput
                            label="Feltöltő csoport id"
                            required={true}
                            {...form.getInputProps("uploaderUserGroupId")}
                            mt="sm"
                        />
                    )}
                    {userGroupsData ? (
                        <Select
                            label="Tiltott csoport"
                            data={userGroupsData}
                            {...form.getInputProps("bannedUserGroupId")}
                            withinPortal={true}
                            clearable={true}
                            mt="sm"
                        />
                    ) : (
                        <TextInput label="Tiltott csoport id" {...form.getInputProps("bannedUserGroupId")} mt="sm" />
                    )}
                    <Group position="apart" spacing={0} mt="md">
                        <Text size="sm">Publikus felhasználói adatok</Text>
                        <Switch {...form.getInputProps("showUploaderInfo", { type: "checkbox" })} />
                    </Group>
                    <NumberInput
                        label="Max kép/felhasználó"
                        min={1}
                        required={true}
                        {...form.getInputProps("maxUploadsPerUser")}
                        mt="sm"
                    />
                    <Text size="sm" mt="sm" weight={500}>
                        Értékelési szempontok
                    </Text>
                    {displayedAspects}
                    <Button
                        onClick={() =>
                            form.insertListItem("aspects", {
                                name: "",
                                key: `a${Math.random().toString(36).slice(2)}`,
                                description: "",
                            })
                        }
                        variant="outline"
                        fullWidth={true}
                        mt="md"
                    >
                        Szempont hozzáadása
                    </Button>
                    <Button type="submit" fullWidth={true} loading={updateImageVoting.isLoading} mt="md">
                        Mentés
                    </Button>
                </form>
            </PermissionRequirement>
            <PermissionRequirement permissions={["ImageVotings.DeleteImageVoting"]}>
                <Divider my="sm" />
                <Button
                    fullWidth={true}
                    color="red"
                    onClick={async () => await doDeleteImageVoting()}
                    loading={deleteImageVoting.isLoading}
                >
                    Törlés
                </Button>
            </PermissionRequirement>
        </Modal>
    );
};

const ManageImageVotings = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const imageVotings = useGetApiImageVotings();

    const [createImageVotingModalOpened, { close: closeCreateImageVotingModal, open: openCreateImageVotingModal }] =
        useDisclosure(false);
    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalImageVoting, setDetailsModalImageVoting] = useState<ImageVotingsIndexImageVotingsResponse>();

    if (imageVotings.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (imageVotings.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <CreateImageVotingModal opened={createImageVotingModalOpened} close={closeCreateImageVotingModal} />
            <DetailsModal imageVoting={detailsModalImageVoting} opened={detailsModalOpened} close={closeDetailsModal} />
            <Group position="apart" align="baseline" mb="md" spacing={0}>
                <Title>Szavazások kezelése</Title>
                <PermissionRequirement permissions={["ImageVotings.CreateImageVoting"]}>
                    <ActionIcon variant="transparent" color="dark" onClick={() => openCreateImageVotingModal()}>
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
                {imageVotings.data.map((imageVoting) => (
                    <ManageImageVotingCard
                        imageVoting={imageVoting}
                        key={imageVoting.id}
                        openDetails={() => {
                            setDetailsModalImageVoting(imageVoting);
                            openDetailsModal();
                        }}
                    />
                ))}
            </SimpleGrid>
            {imageVotings.data.length === 0 && (
                <Text color="dimmed">
                    Úgy néz ki még nincsenek szavazások... Kattints a &quot;Szavazások kezelése&quot; felirat melleti
                    plusz ikonra, hogy létrehozz egyet.
                </Text>
            )}
        </>
    );
};

export default ManageImageVotings;
