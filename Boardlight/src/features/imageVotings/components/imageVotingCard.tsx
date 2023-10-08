import {
    Box,
    Button,
    Card,
    Divider,
    Group,
    Paper,
    Stack,
    Text,
    createStyles,
    rem,
    useMantineTheme,
} from "@mantine/core";
import { IconPhoto, IconPhotoCheck, IconPhotoQuestion, IconPhotoUp, IconUser } from "@tabler/icons-react";

import { ImageVotingsIndexImageVotingsResponse } from "../../../api/generated/models";
import { Link } from "react-router-dom";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";

const useStyles = createStyles((theme) => ({
    section: {
        padding: theme.spacing.md,
        borderTop: `${rem(1)} solid ${theme.colorScheme === "dark" ? theme.colors.dark[4] : theme.colors.gray[3]}`,
    },
}));

export const ImageVotingCard = ({
    imageVoting,
    openDetails,
}: {
    imageVoting: ImageVotingsIndexImageVotingsResponse;
    openDetails(): void;
}): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    return (
        <Card withBorder={true} radius="md">
            <Card.Section className={classes.section}>
                <Stack justify="space-between" align="stretch" spacing={0} sx={{ flex: 1, overflow: "hidden" }}>
                    <Box maw="100%">
                        <Text size="lg" weight={500} truncate={true}>
                            {imageVoting.name}
                        </Text>
                    </Box>
                    <Text size="sm" color="dimmed" truncate={true}>
                        {imageVoting.description}
                    </Text>
                </Stack>
            </Card.Section>
            <Card.Section className={classes.section} mt={0}>
                <Group spacing="xs" sx={{ overflow: "hidden" }}>
                    {imageVoting.type === "SingleChoice" ? (
                        <IconPhotoCheck stroke={1.5} size={20} />
                    ) : imageVoting.type === "Incerement" ? (
                        <IconPhotoUp stroke={1.5} size={20} />
                    ) : (
                        <IconPhotoQuestion stroke={1.5} size={20} />
                    )}
                    <Text size="sm" truncate={true}>
                        {imageVoting.type === "SingleChoice"
                            ? "Választásos"
                            : imageVoting.type === "Increment"
                            ? "Inkrementáló"
                            : "Ismeretlen"}
                    </Text>
                </Group>
            </Card.Section>
            <PermissionRequirement
                permissions={["ImageVotings.ViewImageVoting", "ImageVotings.ViewActiveImageVoting"]}
                fallback={<Card.Section mb={`-${rem(12)}`} />}
            >
                <Card.Section className={classes.section} mb={`-${rem(16)}`}>
                    <Button
                        component={Link}
                        to={`/image-votings/${imageVoting.id}`}
                        fullWidth={true}
                        color="blue"
                        onClick={() => openDetails()}
                    >
                        Mutasd!
                    </Button>
                </Card.Section>
            </PermissionRequirement>
        </Card>
    );
};
