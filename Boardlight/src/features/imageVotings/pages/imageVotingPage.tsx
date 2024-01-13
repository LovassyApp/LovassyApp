import {
    ActionIcon,
    Anchor,
    Badge,
    Box,
    Button,
    Card,
    Center,
    Divider,
    Group,
    Loader,
    Modal,
    Paper,
    Select,
    SimpleGrid,
    Text,
    TextInput,
    Title,
    createStyles,
    rem,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconPlus, IconTrophy } from "@tabler/icons-react";
import { ImageVotingIncrementableCard, IncrementType } from "../components/imageVotingIncrementableEntryCard";
import { NotFoundError, ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import { Suspense, lazy, useMemo, useState } from "react";
import { isNotEmpty, useForm } from "@mantine/form";
import {
    useGetApiImageVotingEntries,
    usePostApiImageVotingEntries,
} from "../../../api/generated/features/image-voting-entries/image-voting-entries";
import {
    useGetApiImageVotingsId,
    useGetApiImageVotingsIdResults,
} from "../../../api/generated/features/image-votings/image-votings";

import { FullScreenLoading } from "../../../core/components/fullScreenLoading";
import { ImageVotingChoosableEntryCard } from "../components/imageVotingChoosableEntryCard";
import { ImageVotingEntryImageSelector } from "../components/imageVotingEntryImageSelector";
import { ImageVotingsViewImageVotingResponse } from "../../../api/generated/models";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useParams } from "react-router-dom";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

const CreateEntryModal = ({
    opened,
    close,
    imageVoting,
}: {
    opened: boolean;
    close(): void;
    imageVoting: ImageVotingsViewImageVotingResponse;
}): JSX.Element => {
    const createEntry = usePostApiImageVotingEntries();

    const form = useForm({
        initialValues: {
            title: "",
            imageUrl: undefined,
        },
        validate: {
            imageUrl: isNotEmpty("Egy kép választása kötelező."),
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await createEntry.mutateAsync({ data: { ...values, imageVotingId: imageVoting.id } });
            notifications.show({
                title: "Kép létrehozva",
                message: "A képet sikeresen létrehoztad (sikeresen neveztél a szavazásba).",
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
        <Modal opened={opened} onClose={close} title="Új kép" size="lg">
            <form onSubmit={submit}>
                <TextInput label="Cím" required={true} {...form.getInputProps("title")} mb="md" />
                <ImageVotingEntryImageSelector
                    setImageUrl={(imageUrl) => form.setFieldValue("imageUrl", imageUrl)}
                    imageVoting={imageVoting}
                    error={form.errors.imageUrl}
                />
                <Button type="submit" loading={createEntry.isLoading} fullWidth={true} mt="md">
                    Létrehozás
                </Button>
            </form>
        </Modal>
    );
};

const ResultsModal = ({
    imageVoting,
    opened,
    close,
}: {
    imageVoting: ImageVotingsViewImageVotingResponse | undefined;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const results = useGetApiImageVotingsIdResults(imageVoting?.id, { query: { enabled: opened } });

    const [aspectKey, setAspectKey] = useState<string | null>(null);
    const [expanded, setExpanded] = useState<boolean>(false);

    const getAspectVotes = (entry, aspectKey) => {
        const aspect = entry.aspects.find((a) => a.key === aspectKey);
        return imageVoting.type === "SingleChoice" ? aspect?.choicesCount : aspect?.incrementSum;
    };

    const orderedEntries = useMemo(() => {
        if (!results.data) return [];

        if (imageVoting.type === "SingleChoice") {
            if (aspectKey === null || imageVoting.aspects.length === 0)
                return results.data.entries.sort((a, b) => b.choicesCount - a.choicesCount);
            return results.data.entries.sort(
                (a, b) =>
                    b.aspects.find((aspect) => aspect.key === aspectKey)?.choicesCount -
                    a.aspects.find((aspect) => aspect.key === aspectKey)?.choicesCount
            );
        } else if (imageVoting.type === "Increment") {
            if (aspectKey === null || imageVoting.aspects.length === 0)
                return results.data.entries.sort((a, b) => b.incrementSum - a.incrementSum);
            return results.data.entries.sort(
                (a, b) =>
                    b.aspects.find((aspect) => aspect.key === aspectKey)?.incrementSum -
                    a.aspects.find((aspect) => aspect.key === aspectKey)?.incrementSum
            );
        }
        return [];
    }, [results.data, imageVoting.type, aspectKey, imageVoting.aspects]);

    if (results.isLoading)
        return (
            <Modal opened={opened} onClose={close} title="Eredmények" size="lg">
                <Loader />
            </Modal>
        );

    return (
        <Modal opened={opened} onClose={close} title="Eredmények" size="lg">
            {imageVoting?.aspects.length > 0 && (
                <>
                    <Select
                        label="Értékelési szempont"
                        placeholder="Válassz értékelési szempontot"
                        value={aspectKey}
                        onChange={(value) => setAspectKey(value)}
                        data={imageVoting?.aspects.map((aspect) => ({
                            value: aspect.key,
                            label: aspect.name,
                        }))}
                        clearable={true}
                        allowDeselect={false}
                        withinPortal={true}
                    />
                    <Divider my="sm" />
                </>
            )}
            <Group position="apart" spacing={0}>
                <Text>Feltöltők száma:</Text>
                <Text weight="bold">{results.data.uploaderCount}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Szavazók száma:</Text>
                <Text weight="bold">{results.data.chooserCount ?? results.data.incrementerCount}</Text>
            </Group>
            <Divider my="sm" />
            <Text mb="md">Toplista {imageVoting.aspects.length > 0 && !aspectKey && " (összesített értékek)"}</Text>
            {orderedEntries.slice(0, !expanded ? 5 : undefined).map((entry) => (
                <Paper
                    key={entry.id}
                    radius="md"
                    sx={(theme) => ({
                        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
                    })}
                    mb="sm"
                    p="sm"
                >
                    <Group position="apart" sx={{ overflow: "hidden" }} w="100%" noWrap={true}>
                        <Text truncate={true} weight={600}>
                            {entry.title}
                        </Text>
                        <Text>
                            {aspectKey
                                ? getAspectVotes(entry, aspectKey)
                                : imageVoting.type === "SingleChoice"
                                ? entry.choicesCount
                                : entry.incrementSum}
                        </Text>
                    </Group>
                </Paper>
            ))}
            {orderedEntries.length > 5 && (
                <Button
                    fullWidth={true}
                    variant="outline"
                    onClick={() => setExpanded(!expanded)}
                    sx={{ justifyContent: "center" }}
                >
                    {expanded ? "Kevesebb" : "Több"}
                </Button>
            )}
        </Modal>
    );
};

const ImageVotingPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const { id } = useParams<{ id: string }>();

    const imageVoting = useGetApiImageVotingsId(parseInt(id), { query: { retry: 0 } });
    const imageVotingEntries = useGetApiImageVotingEntries({ Filters: `ImageVotingId==${id}` });

    const [createEnrtryModalOpened, { close: closeCreateEntryModal, open: openCreateEntryModal }] =
        useDisclosure(false);
    const [resultsModalOpened, { close: closeResultsModal, open: openResultsModal }] = useDisclosure(false);
    const [aspectKey, setAspectKey] = useState<string | null>(null);

    const FourOFour = lazy(() => import("../../../features/base/pages/fourOFour"));

    if ((imageVoting.isError && imageVoting.error instanceof NotFoundError) || Number.isNaN(parseInt(id))) {
        return (
            <Suspense fallback={<FullScreenLoading />}>
                <FourOFour />
            </Suspense>
        );
    } else if (imageVoting.isError || imageVotingEntries.isError) {
        console.error(imageVoting.error);

        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );
    }

    if (imageVoting.isLoading || imageVotingEntries.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    return (
        <>
            <CreateEntryModal
                opened={createEnrtryModalOpened}
                close={closeCreateEntryModal}
                imageVoting={imageVoting.data}
            />
            <ResultsModal imageVoting={imageVoting.data} opened={resultsModalOpened} close={closeResultsModal} />
            <Box mb="md">
                <Group position="apart" align="center">
                    <Box>
                        <Group spacing="xs" align="center">
                            <Title>
                                {imageVoting.data.name}

                                <Text component="span" ml="xs">
                                    {imageVoting.data.active ? (
                                        <Badge sx={{ transform: `translateY(${rem(-6)})` }} color="green" size="lg">
                                            Aktív
                                        </Badge>
                                    ) : (
                                        <Badge sx={{ transform: `translateY(${rem(-6)})` }} color="red" size="lg">
                                            Inaktív
                                        </Badge>
                                    )}
                                </Text>
                            </Title>
                        </Group>
                        <Text color="dimmed" sx={{ hyphens: "auto" }}>
                            {imageVoting.data.description}
                        </Text>
                    </Box>
                    <Group spacing="xs">
                        <PermissionRequirement
                            permissions={
                                imageVoting.data.active
                                    ? [
                                          "ImageVotings.ViewImageVotingResults",
                                          "ImageVotings.ViewActiveImageVotingResults",
                                      ]
                                    : ["ImageVotings.ViewImageVotingResults"]
                            }
                        >
                            <ActionIcon variant="transparent" color="dark" onClick={() => openResultsModal()}>
                                <IconTrophy />
                            </ActionIcon>
                        </PermissionRequirement>
                        {imageVoting.data.canUpload && (
                            <PermissionRequirement
                                permissions={
                                    imageVoting.data.active
                                        ? [
                                              "ImageVotings.CreateImageVotingEntry",
                                              "ImageVotings.CreateActiveImageVotingEntry",
                                          ]
                                        : ["ImageVotings.CreateImageVotingEntry"]
                                }
                            >
                                <ActionIcon variant="transparent" color="dark" onClick={() => openCreateEntryModal()}>
                                    <IconPlus />
                                </ActionIcon>
                            </PermissionRequirement>
                        )}
                    </Group>
                </Group>
            </Box>
            {imageVoting.data.aspects.length > 0 &&
                imageVoting.data.type === "SingleChoice" &&
                imageVoting.data.aspects.some((a) => a.canChoose) && (
                    <Text mb="md" color="dimmed" size="xs" align="center">
                        Válassz ki egy értékelési szempontot majd kattints egy képre, hogy rá szavazz!
                    </Text>
                )}
            {imageVoting.data.aspects.length === 0 &&
                imageVoting.data.type === "SingleChoice" &&
                imageVotingEntries.data.some((e) => e.canChoose) && (
                    <Text mb="md" color="dimmed" size="xs" align="center">
                        Kattints egy képre, hogy rá szavazz!
                    </Text>
                )}
            <SimpleGrid
                cols={4}
                breakpoints={[
                    { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                    { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                    { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                ]}
            >
                {imageVoting.data.type === "SingleChoice" &&
                    imageVotingEntries.data.map((imageVotingEntry) => (
                        <ImageVotingChoosableEntryCard
                            imageVoting={imageVoting.data}
                            imageVotingEntry={imageVotingEntry}
                            key={imageVotingEntry.id}
                            chosen={
                                imageVoting.data.aspects.find((aspect) => aspect.key === aspectKey)?.chosenEntryId ===
                                imageVotingEntry.id
                            }
                            aspectKey={aspectKey}
                        />
                    ))}
                {imageVoting.data.type === "Increment" &&
                    imageVotingEntries.data.map((imageVotingEntry) => (
                        <ImageVotingIncrementableCard
                            imageVoting={imageVoting.data}
                            imageVotingEntry={imageVotingEntry}
                            key={imageVotingEntry.id}
                            incrementType={
                                imageVoting.data.aspects
                                    .find((aspect) => aspect.key === aspectKey)
                                    ?.imageVotingAspectEntryIncrements.find((i) => i.entryId === imageVotingEntry.id)
                                    ?.type as IncrementType
                            }
                            aspectKey={aspectKey}
                        />
                    ))}
            </SimpleGrid>
            {imageVoting.data.aspects.length > 0 && (
                <Card
                    withBorder={true}
                    radius="md"
                    mb="md"
                    pos="fixed"
                    sx={{
                        bottom: 0,
                        left: "50%",
                        transform: "translateX(-50%)",
                        zIndex: 10,
                    }}
                    w={rem(300)}
                >
                    <Select
                        label="Értékelési szempont"
                        placeholder="Válassz értékelési szempontot"
                        value={aspectKey}
                        onChange={(value) => setAspectKey(value)}
                        data={imageVoting.data.aspects.map((aspect) => ({
                            value: aspect.key,
                            label: aspect.name,
                        }))}
                        clearable={true}
                        allowDeselect={false}
                        withinPortal={true}
                    />
                </Card>
            )}
        </>
    );
};

export default ImageVotingPage;
