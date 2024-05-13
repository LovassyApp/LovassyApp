import {
    Center,
    Divider,
    Group,
    Loader,
    Modal,
    SimpleGrid,
    Stack,
    Text,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";

import { CoinCard } from "../components/coinCard";
import { CoinsStats } from "../components/coinStats";
import { GradeCard } from "../../school/components/gradeCard";
import { ShopIndexOwnLolosResponseCoin } from "../../../api/generated/models";
import { SchoolIndexGradesResponseGrade } from "../../../api/generated/models";
import { useDisclosure } from "@mantine/hooks";
import { useGetApiGrades } from "../../../api/generated/features/grades/grades";
import { useGetApiLolosOwn } from "../../../api/generated/features/lolos/lolos";
import { useState } from "react";
import { GradeDetailsModal } from "../../school/components/gradeDetailsModal";
const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

const DetailsModal = ({
    coin,
    opened,
    close,
}: {
    coin: ShopIndexOwnLolosResponseCoin;
    opened: boolean;
    close(): void;
}): JSX.Element => {
    const [gdetailsModalOpened, { close: closegDetailsModal, open: opengDetailsModal }] = useDisclosure(false);
    const [gdetailsModalGrade, setgDetailsModalGrade] = useState<SchoolIndexGradesResponseGrade>();
    return (
        <>
        <Modal opened={opened} onClose={close} title="Részletek" size="lg">
            <GradeDetailsModal grade={gdetailsModalGrade} opened={gdetailsModalOpened} close={closegDetailsModal}/>
            <Group position="apart" spacing={0}>
                <Text>Indoklás:</Text>
                <Text weight="bold">{coin?.reason}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Elköltve:</Text>
                <Text weight="bold">{coin?.isSpent ? "Igen" : "Nem"}</Text>
            </Group>
            {coin?.grades && (
                <>
                    <Divider my="sm" />
                    <Text mb="sm">Jegyek:</Text>
                    <Stack mb="lg">
                        {coin.grades.map((grade) => (
                            <GradeCard key={grade.id} grade={grade} openDetails={() => {
                                setgDetailsModalGrade(grade);
                                opengDetailsModal();
                            }} />
                        ))}
                    </Stack>
                </>
            )}
            <Divider my="sm" />
            <Group position="apart" spacing={0}>
                <Text>Létrehozva:</Text>
                <Text weight="bold">{new Date(coin?.createdAt).toLocaleString("hu-HU", {})}</Text>
            </Group>
            <Group position="apart" spacing={0}>
                <Text>Módosítva:</Text>
                <Text weight="bold">{new Date(coin?.updatedAt).toLocaleString("hu-HU", {})}</Text>
            </Group>
        </Modal>
        </>
    );
};

const OwnCoinsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const coins = useGetApiLolosOwn({ Sorts: "isSpent" });
    const grades = useGetApiGrades();

    const [detailsModalOpened, { close: closeDetailsModal, open: openDetailsModal }] = useDisclosure(false);
    const [detailsModalCoin, setDetailsModalCoin] = useState<ShopIndexOwnLolosResponseCoin>();

    if (coins.isLoading || grades.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (coins.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <DetailsModal coin={detailsModalCoin} opened={detailsModalOpened} close={closeDetailsModal} />
            <Title mb="md">Statisztikák</Title>
            <SimpleGrid cols={2} breakpoints={[{ maxWidth: theme.breakpoints.sm, cols: 1, spacing: "sm" }]}>
                <CoinsStats data={coins.data} grades={grades.isError ? undefined : grades.data} />
            </SimpleGrid>
            <Title my="md">Érmék</Title>
            <SimpleGrid
                cols={4}
                breakpoints={[
                    { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                    { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                    { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                ]}
            >
                {coins.data?.coins.map((coin) => (
                    <CoinCard
                        openDetails={() => {
                            setDetailsModalCoin(coin);
                            openDetailsModal();
                        }}
                        key={coin.id}
                        coin={coin}
                    />
                ))}
            </SimpleGrid>
            {coins.data.coins.length === 0 && (
                <Text color="dimmed">
                    Úgy néz ki még nincs egy lolód sem... Amint lesz, itt látod majd a loló érméidet.
                </Text>
            )}
        </>
    );
};

export default OwnCoinsPage;
