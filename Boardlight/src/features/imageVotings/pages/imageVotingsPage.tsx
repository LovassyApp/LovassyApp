import { Box, Center, Loader, SimpleGrid, Text, Title, createStyles, useMantineTheme } from "@mantine/core";

import { ImageVotingCard } from "../components/imageVotingCard";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";
import { useGetApiImageVotings } from "../../../api/generated/features/image-votings/image-votings";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

const ImageVotingsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const control = useGetApiAuthControl({ query: { enabled: false } }); // Should have it already

    const imageVotings = useGetApiImageVotings({
        Filters:
            control.data?.permissions?.includes("ImageVotings.IndexImageVotings") || control.data?.isSuperUser
                ? "Active==true"
                : "",
        Sorts: "-UpdatedAt",
    });

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
            <Title mb="md">Szavazások</Title>
            <SimpleGrid
                cols={4}
                breakpoints={[
                    { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                    { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                    { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                ]}
            >
                {imageVotings.data.map((imageVoting) => (
                    <ImageVotingCard imageVoting={imageVoting} key={imageVoting.id} openDetails={() => {}} />
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

export default ImageVotingsPage;
