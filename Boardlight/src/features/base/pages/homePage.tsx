import { Box, Grid, SimpleGrid, Skeleton, Stack, Title, createStyles, px, rem, useMantineTheme } from "@mantine/core";

import { DisabledFallbackCard } from "../components/disabledFallbackCard";
import { FeatureRequirement } from "../../../core/components/requirements/featuresRequirement";
import { FeedCard } from "../components/feedCard";
import { ForbiddenFallbackCard } from "../components/forbiddenFallbackCard";
import { GradesAverageCard } from "../components/gradesAverageCard";
import { LoloStatsCard } from "../components/loloStatsCard";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { SubjectsAverageCard } from "../components/subjectsAverageCard";
import { useViewportSize } from "@mantine/hooks";

const useStyles = createStyles((theme) => ({}));

const PRIMARY_COL_HEIGHT = rem(606);

const HomePage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const SECONDARY_COL_HEIGHT = `calc(${PRIMARY_COL_HEIGHT} / 2 - ${theme.spacing.md} / 2)`;

    return (
        <>
            <Title mb="md">Kezdőlap</Title>
            <SimpleGrid cols={2} spacing="md" breakpoints={[{ maxWidth: "sm", cols: 1 }]}>
                <Box h={PRIMARY_COL_HEIGHT}>
                    <FeatureRequirement features={["Feed"]} fallback={<DisabledFallbackCard />}>
                        <PermissionRequirement
                            permissions={["Feed.IndexFeedItems"]}
                            fallback={<ForbiddenFallbackCard />}
                        >
                            <FeedCard />
                        </PermissionRequirement>
                    </FeatureRequirement>
                </Box>
                <Grid gutter="md">
                    <Grid.Col>
                        <Box h={SECONDARY_COL_HEIGHT}>
                            <FeatureRequirement features={["Shop"]} fallback={<DisabledFallbackCard />}>
                                <PermissionRequirement
                                    permissions={["Shop.IndexOwnLolos"]}
                                    fallback={<ForbiddenFallbackCard />}
                                >
                                    <LoloStatsCard />
                                </PermissionRequirement>
                            </FeatureRequirement>
                        </Box>
                    </Grid.Col>
                    <Grid.Col span={6}>
                        <Box h={SECONDARY_COL_HEIGHT}>
                            <FeatureRequirement features={["School"]} fallback={<DisabledFallbackCard />}>
                                <PermissionRequirement
                                    permissions={["School.IndexGrades"]}
                                    fallback={<ForbiddenFallbackCard />}
                                >
                                    <SubjectsAverageCard />
                                </PermissionRequirement>
                            </FeatureRequirement>
                        </Box>
                    </Grid.Col>
                    <Grid.Col span={6}>
                        <Box h={SECONDARY_COL_HEIGHT}>
                            <FeatureRequirement features={["School"]} fallback={<DisabledFallbackCard />}>
                                <PermissionRequirement
                                    permissions={["School.IndexGrades"]}
                                    fallback={<ForbiddenFallbackCard />}
                                >
                                    <GradesAverageCard />
                                </PermissionRequirement>
                            </FeatureRequirement>
                        </Box>
                    </Grid.Col>
                </Grid>
            </SimpleGrid>
        </>
    );
};

export default HomePage;
