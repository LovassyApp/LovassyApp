import { ActionIcon, Box, Card, Image, Overlay, Text, createStyles, rem } from "@mantine/core";
import { IconCheck, IconSquareForbid2, IconTrash, IconX, IconZoomIn } from "@tabler/icons-react";
import {
    ImageVotingsIndexImageVotingEntriesResponse,
    ImageVotingsViewImageVotingResponse,
} from "../../../api/generated/models";
import {
    useDeleteApiImageVotingEntriesId,
    useDeleteApiImageVotingEntriesIdChoice,
    usePostApiImageVotingEntriesIdChoice,
} from "../../../api/generated/features/image-voting-entries/image-voting-entries";
import { useMemo, useState } from "react";

import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { ValidationError } from "../../../helpers/apiHelpers";
import { getGetApiImageVotingsIdQueryKey } from "../../../api/generated/features/image-votings/image-votings";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useQueryClient } from "@tanstack/react-query";

const useStyles = createStyles((theme) => ({
    section: {
        padding: theme.spacing.sm,
        borderTop: `${rem(1)} solid ${theme.colorScheme === "dark" ? theme.colors.dark[4] : theme.colors.gray[3]}`,
    },
    activeCard: {
        borderColor: `${theme.colors.blue[6]} !important`,
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

export const ImageVotingChoosableEntryCard = ({
    imageVotingEntry,
    chosen,
    aspectKey,
    imageVoting,
}: {
    imageVotingEntry: ImageVotingsIndexImageVotingEntriesResponse;
    chosen?: boolean;
    aspectKey?: string;
    imageVoting: ImageVotingsViewImageVotingResponse;
}): JSX.Element => {
    const { classes } = useStyles();

    const queryClient = useQueryClient();
    const imageVotingQueryKey = getGetApiImageVotingsIdQueryKey(imageVoting.id);

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const canChoose = useMemo(
        () =>
            (control.data.permissions.includes("ImageVotings.ChooseImageVotingEntry") ||
                (control.data.permissions.includes("ImageVotings.ChooseActiveImageVotingEntry") &&
                    imageVoting.active) ||
                control.data.isSuperUser) &&
            !chosen &&
            !imageVotingEntry.chosen &&
            (imageVotingEntry.canChoose || (imageVotingEntry.canChoose === null && aspectKey)),
        [control]
    );

    const canUnchoose = useMemo(
        () =>
            (control.data.permissions.includes("ImageVotings.UnchooseImageVotingEntry") ||
                (control.data.permissions.includes("ImageVotings.UnchooseActiveImageVotingEntry") &&
                    imageVoting.active) ||
                control.data.isSuperUser) &&
            (chosen || imageVotingEntry.chosen),
        [control]
    );

    const chooseEntry = usePostApiImageVotingEntriesIdChoice();
    const unchooseEntry = useDeleteApiImageVotingEntriesIdChoice();

    const deleteImageVotingEntry = useDeleteApiImageVotingEntriesId();

    const [magnifiedViewOpen, { close: closeMagnifiedViewOpen, open: openMagnifiedViewOpen }] = useDisclosure();
    const [optimisticallyChosen, setOptimisticallyChosen] = useState<boolean | undefined>(undefined);

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

    const onClick = async () => {
        try {
            if (chosen || imageVotingEntry.chosen) {
                setOptimisticallyChosen(false);
                await unchooseEntry.mutateAsync({ id: imageVotingEntry.id, data: { aspectKey: aspectKey } });
                notifications.show({
                    title: "Kép kiválasztásának visszavonása",
                    color: "green",
                    icon: <IconCheck />,
                    message: "A kép kiválasztását sikeresen visszavontad.",
                });
                setOptimisticallyChosen(undefined);
            } else {
                setOptimisticallyChosen(true);
                await chooseEntry.mutateAsync({ id: imageVotingEntry.id, data: { aspectKey: aspectKey } });
                notifications.show({
                    title: "Kép kiválasztva",
                    color: "green",
                    icon: <IconCheck />,
                    message: "A képet sikeresen kiválasztottad.",
                });
                setOptimisticallyChosen(undefined);
            }
            if (aspectKey) await queryClient.invalidateQueries({ queryKey: [imageVotingQueryKey[0]] });
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
            <Card
                radius="md"
                withBorder={true}
                className={chosen || imageVotingEntry.chosen ? classes.activeCard : undefined}
                sx={{ cursor: canChoose || canUnchoose || optimisticallyChosen ? "pointer" : "default" }}
                onClick={canChoose || canUnchoose ? onClick : undefined}
            >
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
            </Card>
        </>
    );
};
