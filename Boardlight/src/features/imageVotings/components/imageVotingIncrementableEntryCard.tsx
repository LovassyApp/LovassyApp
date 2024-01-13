import { ActionIcon, Box, Card, Group, Image, Overlay, Text, createStyles, rem, useMantineTheme } from "@mantine/core";
import {
    IconArrowBigDown,
    IconArrowBigDownFilled,
    IconArrowBigUp,
    IconArrowBigUpFilled,
    IconCheck,
    IconSquareForbid2,
    IconStar,
    IconStarFilled,
    IconTrash,
    IconX,
    IconZoomIn,
} from "@tabler/icons-react";
import {
    ImageVotingsIndexImageVotingEntriesResponse,
    ImageVotingsViewImageVotingResponse,
} from "../../../api/generated/models";
import {
    useDeleteApiImageVotingEntriesId,
    useDeleteApiImageVotingEntriesIdIncrement,
    usePostApiImageVotingEntriesIdIncrement,
} from "../../../api/generated/features/image-voting-entries/image-voting-entries";

import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { ValidationError } from "../../../helpers/apiHelpers";
import { getGetApiImageVotingsIdQueryKey } from "../../../api/generated/features/image-votings/image-votings";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useMemo } from "react";
import { useQueryClient } from "@tanstack/react-query";

const useStyles = createStyles((theme) => ({
    section: {
        padding: theme.spacing.sm,
        borderTop: `${rem(1)} solid ${theme.colorScheme === "dark" ? theme.colors.dark[4] : theme.colors.gray[3]}`,
    },
    overlayContainer: {
        ["&:after"]: {
            content: '""',
            position: "absolute",
            display: "block",
            left: 0,
            top: 0,
            width: "100%",
            height: "100%",
            background:
                "rgba(0, 0, 0, 0) linear-gradient(to bottom, rgba(0, 0, 0, 0) 0px, rgba(0, 0, 0, 0.6) 100%) repeat 0 0",
            zIndex: 1,
        },
    },
}));

export enum IncrementType {
    None = "None",
    Incremented = "Incremented",
    SuperIncremented = "SuperIncremented",
    Decremented = "Decremented",
}

export const ImageVotingIncrementableCard = ({
    imageVotingEntry,
    incrementType,
    aspectKey,
    imageVoting,
}: {
    imageVotingEntry: ImageVotingsIndexImageVotingEntriesResponse;
    incrementType?: IncrementType;
    aspectKey?: string;
    imageVoting: ImageVotingsViewImageVotingResponse;
}): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const queryClient = useQueryClient();
    const imageVotingQueryKey = getGetApiImageVotingsIdQueryKey(imageVoting.id);

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const canCreateIncrement = useMemo(
        () =>
            (control.data.permissions.includes("ImageVotings.CreateImageVotingEntryIncrement") ||
                (control.data.permissions.includes("ImageVotings.CreateActiveImageVotingEntryIncrement") &&
                    imageVoting.active) ||
                control.data.isSuperUser) &&
            imageVotingEntry.userId !== control.data.user.id &&
            (!imageVotingEntry.incrementType ? aspectKey : true),
        [control]
    );

    const canDeleteIncrement = useMemo(
        () =>
            control.data.permissions.includes("ImageVotings.DeleteImageVotingEntryIncrement") ||
            (control.data.permissions.includes("ImageVotings.DeleteActiveImageVotingEntryIncrement") &&
                imageVoting.active) ||
            control.data.isSuperUser,
        [control]
    );

    const createIncrement = usePostApiImageVotingEntriesIdIncrement();
    const deleteIncrement = useDeleteApiImageVotingEntriesIdIncrement();

    const deleteImageVotingEntry = useDeleteApiImageVotingEntriesId();

    const [magnifiedViewOpen, { close: closeMagnifiedViewOpen, open: openMagnifiedViewOpen }] = useDisclosure();

    const doDeleteImageVotingEntry = async () => {
        try {
            await deleteImageVotingEntry.mutateAsync({ id: imageVotingEntry.id });
            notifications.show({
                title: "Kép törölve",
                color: "green",
                icon: <IconCheck />,
                message: "A képet (nevezést) sikeresen törölted.",
            });
            await queryClient.invalidateQueries({ queryKey: [imageVotingQueryKey[0]] });
        } catch (err) {
            if (err instanceof ValidationError) {
                notifications.show({
                    title: "Hiba (400)",
                    color: "red",
                    icon: <IconX />,
                    message: err.message,
                });
            }
        }
    };

    const onIncrementClick = async () => {
        const shouldRemove =
            (incrementType === IncrementType.Incremented ||
                imageVotingEntry.incrementType === IncrementType.Incremented) &&
            canDeleteIncrement;

        try {
            if (shouldRemove) {
                await deleteIncrement.mutateAsync({
                    id: imageVotingEntry.id,
                    data: {
                        aspectKey: aspectKey,
                    },
                });
            } else {
                await createIncrement.mutateAsync({
                    id: imageVotingEntry.id,
                    data: {
                        type: "Increment",
                        aspectKey: aspectKey,
                    },
                });
            }
            await queryClient.invalidateQueries({ queryKey: [imageVotingQueryKey[0]] });
        } catch (err) {
            if (err instanceof ValidationError) {
                notifications.show({
                    title: "Hiba (400)",
                    color: "red",
                    icon: <IconX />,
                    message: err.message,
                });
            }
        }
    };

    const onSuperIncrementClick = async () => {
        const shouldRemove =
            (incrementType === IncrementType.SuperIncremented ||
                imageVotingEntry.incrementType === IncrementType.SuperIncremented) &&
            canDeleteIncrement;

        try {
            if (shouldRemove) {
                await deleteIncrement.mutateAsync({
                    id: imageVotingEntry.id,
                    data: {
                        aspectKey: aspectKey,
                    },
                });
            } else {
                await createIncrement.mutateAsync({
                    id: imageVotingEntry.id,
                    data: {
                        type: "SuperIncrement",
                        aspectKey: aspectKey,
                    },
                });
            }
            await queryClient.invalidateQueries({ queryKey: [imageVotingQueryKey[0]] });
        } catch (err) {
            if (err instanceof ValidationError) {
                notifications.show({
                    title: "Hiba (400)",
                    color: "red",
                    icon: <IconX />,
                    message: err.message,
                });
            }
        }
    };

    const onDecrementClick = async () => {
        const shouldRemove =
            incrementType ===
                (IncrementType.Decremented || imageVotingEntry.incrementType === IncrementType.Decremented) &&
            canDeleteIncrement;

        try {
            if (shouldRemove) {
                await deleteIncrement.mutateAsync({
                    id: imageVotingEntry.id,
                    data: {
                        aspectKey: aspectKey,
                    },
                });
            } else {
                await createIncrement.mutateAsync({
                    id: imageVotingEntry.id,
                    data: {
                        type: "Decrement",
                        aspectKey: aspectKey,
                    },
                });
            }
            await queryClient.invalidateQueries({ queryKey: [imageVotingQueryKey[0]] });
        } catch (err) {
            if (err instanceof ValidationError) {
                notifications.show({
                    title: "Hiba (400)",
                    color: "red",
                    icon: <IconX />,
                    message: err.message,
                });
            }
        }
    };

    return (
        <>
            {magnifiedViewOpen && (
                <Overlay
                    fixed={true}
                    onClick={(event) => {
                        event.stopPropagation();
                        closeMagnifiedViewOpen();
                    }}
                    center={true}
                >
                    <Image
                        src={imageVotingEntry.imageUrl}
                        alt={imageVotingEntry.title}
                        fit="contain"
                        maw="90vw"
                        height={"90vh"}
                    />
                    <Text pos="fixed" bottom={0} size="lg" weight={500} mt="md" align="center" m="sm" color="white">
                        {imageVotingEntry.title}
                    </Text>
                </Overlay>
            )}
            <Card radius="md" withBorder={true}>
                <Card.Section>
                    <Box className={classes.overlayContainer} pos="relative">
                        <Image
                            src={imageVotingEntry.imageUrl}
                            alt={imageVotingEntry.title}
                            height={200}
                            fit="contain"
                            sx={(theme) => ({
                                backgroundColor:
                                    theme.colorScheme === "dark" ? theme.colors.dark[5] : theme.colors.gray[1],
                            })}
                        />
                        <PermissionRequirement
                            permissions={[
                                "ImageVotings.DeleteImageVotingEntry",
                                "ImageVotings.DeleteOwnImageVotingEntry",
                            ]}
                            fallback={
                                <Box c="white" pos="absolute" bottom={rem(8)} left={rem(8)} sx={{ zIndex: 2 }}>
                                    <IconSquareForbid2 />
                                </Box>
                            }
                        >
                            {(imageVotingEntry.canChoose === false &&
                                imageVotingEntry.userId === control.data.user.id) ||
                            control.data.permissions.includes("ImageVotings.DeleteImageVotingEntry") ||
                            control.data.isSuperUser ? (
                                <ActionIcon
                                    variant="transparent"
                                    pos="absolute"
                                    bottom={rem(8)}
                                    left={rem(8)}
                                    sx={{ zIndex: 2 }}
                                    color="red.5"
                                    onClick={(event) => {
                                        event.stopPropagation();
                                        doDeleteImageVotingEntry();
                                    }}
                                >
                                    <IconTrash />
                                </ActionIcon>
                            ) : undefined}
                        </PermissionRequirement>
                        <ActionIcon
                            c="white"
                            variant="transparent"
                            pos="absolute"
                            bottom={rem(8)}
                            right={rem(8)}
                            sx={{ zIndex: 2 }}
                            onClick={(event) => {
                                event.stopPropagation();
                                openMagnifiedViewOpen();
                            }}
                        >
                            <IconZoomIn />
                        </ActionIcon>
                    </Box>
                </Card.Section>
                <Card.Section className={classes.section} mt={0}>
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {imageVotingEntry.title}
                        </Text>
                    </Box>
                </Card.Section>
                {imageVotingEntry.user && (
                    <Card.Section className={classes.section}>
                        <Box maw="100%">
                            <Text size="sm" color="dimmed" truncate={true}>
                                {imageVotingEntry.user.realName
                                    ? `${imageVotingEntry.user.realName} - ${imageVotingEntry.user.class}`
                                    : imageVotingEntry.user.name}
                            </Text>
                        </Box>
                    </Card.Section>
                )}
                {canCreateIncrement && (
                    <Card.Section className={classes.section} mt={1}>
                        <Group spacing="xs" position="center">
                            <ActionIcon variant="subtle" color={theme.primaryColor} onClick={onIncrementClick}>
                                {(imageVotingEntry.incrementType as IncrementType) === IncrementType.Incremented ||
                                incrementType === IncrementType.Incremented ? (
                                    <IconArrowBigUpFilled size={rem(18)} />
                                ) : (
                                    <IconArrowBigUp size={rem(18)} />
                                )}
                            </ActionIcon>
                            {imageVoting.superIncrementAllowed && (
                                <ActionIcon variant="subtle" color={theme.primaryColor} onClick={onSuperIncrementClick}>
                                    {(imageVotingEntry.incrementType as IncrementType) ===
                                        IncrementType.SuperIncremented ||
                                    incrementType === IncrementType.SuperIncremented ? (
                                        <IconStarFilled size={rem(18)} />
                                    ) : (
                                        <IconStar size={rem(18)} />
                                    )}
                                </ActionIcon>
                            )}
                            <ActionIcon variant="subtle" color={theme.primaryColor} onClick={onDecrementClick}>
                                {(imageVotingEntry.incrementType as IncrementType) === IncrementType.Decremented ||
                                incrementType === IncrementType.Decremented ? (
                                    <IconArrowBigDownFilled size={rem(18)} />
                                ) : (
                                    <IconArrowBigDown size={rem(18)} />
                                )}
                            </ActionIcon>
                        </Group>
                    </Card.Section>
                )}
            </Card>
        </>
    );
};
