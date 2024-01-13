import {
    ActionIcon,
    Box,
    Button,
    Center,
    FileInput,
    Group,
    Image,
    Loader,
    Stack,
    createStyles,
    rem,
} from "@mantine/core";
import {
    ImageVotingsIndexImageVotingEntryImagesResponse,
    ImageVotingsViewImageVotingResponse,
} from "../../../api/generated/models";
import React, { ReactNode, useEffect, useMemo } from "react";
import {
    getGetApiImageVotingEntryImagesQueryKey,
    useDeleteApiImageVotingEntryImagesId,
    useGetApiImageVotingEntryImages,
    usePostApiImageVotingEntryImages,
} from "../../../api/generated/features/image-voting-entry-images/image-voting-entry-images";

import { IconTrash } from "@tabler/icons-react";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { ValidationError } from "../../../helpers/apiHelpers";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useQueryClient } from "@tanstack/react-query";

const useStyles = createStyles((theme) => ({
    selectableImage: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
        cursor: "pointer",
        overflow: "hidden",
        transition: "box-shadow 100ms ease",
        border: `${rem(1)} solid ${theme.colorScheme === "dark" ? theme.colors.dark[4] : theme.colors.gray[3]}`,
        position: "relative",
    },
    selectedImage: {
        borderColor: `${theme.colors.blue[6]} !important`,
    },
}));

const SelectableImage = ({
    selectedImageId,
    setSelectedImageId,
    image,
    imageVotingId,
}: {
    selectedImageId: number | null;
    setSelectedImageId(id: number | null): void;
    image: ImageVotingsIndexImageVotingEntryImagesResponse;
    imageVotingId: number;
}): JSX.Element => {
    const { classes, cx } = useStyles();

    const queryClient = useQueryClient();
    const imagesQueryKey = getGetApiImageVotingEntryImagesQueryKey({ ImageVotingId: imageVotingId });

    const deleteEntryImage = useDeleteApiImageVotingEntryImagesId();

    const doDeleteImage = async () => {
        try {
            await deleteEntryImage.mutateAsync({ id: image.id });
            queryClient.invalidateQueries({ queryKey: [imagesQueryKey[0]] });
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <Box pos="relative">
            <Image
                src={image.url}
                key={image.id}
                width={75}
                height={75}
                fit="contain"
                className={cx(classes.selectableImage, { [classes.selectedImage]: selectedImageId === image.id })}
                onClick={() => setSelectedImageId(selectedImageId === image.id ? null : image.id)}
            />
            <PermissionRequirement
                permissions={[
                    "ImageVotings.DeleteOwnImageVotingEntryImage",
                    "ImageVotings.DeleteImageVotingEntryImage",
                ]}
            >
                <ActionIcon
                    onClick={() => doDeleteImage()}
                    variant="filled"
                    color="red"
                    size="sm"
                    style={{ position: "absolute", top: rem(6), left: rem(6) }}
                >
                    <IconTrash stroke={1.5} size={16} />
                </ActionIcon>
            </PermissionRequirement>
        </Box>
    );
};

export const ImageVotingEntryImageSelector = ({
    setImageUrl,
    imageVoting,
    error,
}: {
    setImageUrl(imageUrl: string | undefined): void;
    imageVoting: ImageVotingsViewImageVotingResponse;
    error: ReactNode;
}): JSX.Element => {
    const { classes } = useStyles();

    const queryClient = useQueryClient();
    const imagesQueryKey = getGetApiImageVotingEntryImagesQueryKey({ ImageVotingId: imageVoting.id });

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const indexQueryEnabled = useMemo(
        () =>
            (control.data?.permissions?.includes("ImageVotings.IndexOwnImageVotingEntryImages") ||
                control.data?.permissions?.includes("ImageVotings.IndexImageVotingEntryImages") ||
                control.data?.isSuperUser) ??
            false,
        [control]
    );

    const imageVotingEntryImages = useGetApiImageVotingEntryImages(
        {
            ImageVotingId: imageVoting.id,
            Filters:
                control.data?.permissions?.includes("ImageVotings.IndexImageVotingEntryImages") ||
                control.data?.isSuperUser
                    ? `UserId==${control.data?.user.id}`
                    : "",
        },
        {
            query: { enabled: indexQueryEnabled },
        }
    );

    const uploadImage = usePostApiImageVotingEntryImages();

    const [selectedImage, setSelectedImage] = React.useState<File | null>(null);
    const [imageError, setImageError] = React.useState<string | null>(null);
    const [selectedImageId, setSelectedImageId] = React.useState<number | null>(null);

    useEffect(() => {
        if (selectedImageId !== null) {
            const image = imageVotingEntryImages.data?.find((image) => image.id === selectedImageId);
            if (image) setImageUrl(image.url);
        } else {
            setImageUrl(undefined);
        }
    }, [selectedImageId]);

    const doUploadImage = async () => {
        try {
            setImageError(null);
            const res = await uploadImage.mutateAsync({
                data: {
                    ImageVotingId: imageVoting.id,
                    File: selectedImage,
                },
            });
            setImageUrl(res.url);
            queryClient.invalidateQueries({ queryKey: [imagesQueryKey[0]] });
            setSelectedImageId(res.id);
        } catch (err) {
            if (err instanceof ValidationError) {
                console.log(err);
                setImageError(err.errors.file !== undefined ? err.errors.file[0] : err.message);
            }
        }
    };

    return (
        <Stack spacing="xs">
            {imageVotingEntryImages.isInitialLoading && (
                <Center>
                    <Loader />
                </Center>
            )}
            {imageVotingEntryImages.data && (
                <Group>
                    {imageVotingEntryImages.data.map((image) => (
                        <SelectableImage
                            selectedImageId={selectedImageId}
                            setSelectedImageId={setSelectedImageId}
                            image={image}
                            imageVotingId={imageVoting.id}
                            key={image.id}
                        />
                    ))}
                </Group>
            )}
            <Group align="flex-end" w="100%">
                <FileInput
                    label="Kép feltöltése"
                    accept="image/bmp,image/jpeg,image/x-png,image/png,image/gif"
                    value={selectedImage}
                    onChange={setSelectedImage}
                    clearable={true}
                    error={imageError ?? error}
                    sx={{ flex: "1 !important" }}
                />
                <Button
                    onClick={() => doUploadImage()}
                    loading={uploadImage.isLoading}
                    mb={imageError || error ? rem(19) : 0}
                >
                    Feltöltés
                </Button>
            </Group>
        </Stack>
    );
};
