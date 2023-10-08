import {
    ActionIcon,
    Badge,
    Box,
    Button,
    Card,
    Center,
    Group,
    Loader,
    Modal,
    Select,
    SimpleGrid,
    Text,
    TextInput,
    Title,
    createStyles,
    rem,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconPlus } from "@tabler/icons-react";
import { NotFoundError, ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import { Suspense, lazy, useState } from "react";
import { isNotEmpty, useForm } from "@mantine/form";
import {
    useGetApiImageVotingEntries,
    useGetApiImageVotingEntriesId,
    usePostApiImageVotingEntries,
} from "../../../api/generated/features/image-voting-entries/image-voting-entries";

import { FullScreenLoading } from "../../../core/components/fullScreenLoading";
import { ImageVotingEntryCard } from "../components/imageVotingEntryCard";
import { ImageVotingEntryImageSelector } from "../components/imageVotingEntryImageSelector";
import { ImageVotingsViewImageVotingResponse } from "../../../api/generated/models";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useGetApiImageVotingsId } from "../../../api/generated/features/image-votings/image-votings";
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

const ImageVotingPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const { id } = useParams<{ id: string }>();

    const imageVoting = useGetApiImageVotingsId(parseInt(id), { query: { retry: 0 } });
    const imageVotingEntries = useGetApiImageVotingEntries({ Filters: `ImageVotingId==${id}` });

    const [createEnrtryModalOpened, { close: closeCreateEntryModal, open: openCreateEntryModal }] =
        useDisclosure(false);
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
            <Box mb="md">
                <Group position="apart" align="center">
                    <Box>
                        <Group spacing="xs" align="center">
                            <Title>{imageVoting.data.name}</Title>
                            {imageVoting.data.active ? (
                                <Badge mt={rem(8)} color="green">
                                    Aktív
                                </Badge>
                            ) : (
                                <Badge mt={rem(8)} color="red">
                                    Inaktív
                                </Badge>
                            )}
                        </Group>
                        <Text color="dimmed" sx={{ hyphens: "auto" }}>
                            {imageVoting.data.description}
                        </Text>
                    </Box>
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
            </Box>
            <SimpleGrid
                cols={4}
                breakpoints={[
                    { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                    { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                    { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                ]}
            >
                {imageVotingEntries.data.map((imageVotingEntry) => (
                    <ImageVotingEntryCard
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
