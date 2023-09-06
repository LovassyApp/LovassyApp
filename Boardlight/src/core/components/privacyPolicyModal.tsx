import { Anchor, Box, Button, Modal, Space, Text, Title, createStyles } from "@mantine/core";

const useStyles = createStyles((theme) => ({}));

export const PrivacyPolicyModal = ({ opened, close }: { opened: boolean; close(): void }): JSX.Element => {
    const { classes } = useStyles();

    return (
        <Modal opened={opened} onClose={close} size="lg" title="Adatvédelmi tájékoztató">
            <Title size="h5" mb="sm">
                Adatvédelem
            </Title>
            <Text mb="xs">
                A LovassyApp egy alkalmazás, mely az érdemjegyeid segítségével jutalmazza a tanulmányi munkádat. Az
                érdemjegyeid csak közvetlenül egy iskolai számítógép és a mi szerverünk között közlekednek, ahol
                titkosított formában, csak az általad kezelt titkosító kulcs segítségével olvashatóak. Az adatok minden
                esetben titkosítva maradnak, egészen addig, amíg a saját eszközöd meg nem jeleníti őket.
            </Text>
            <Text mb="xs">
                A LovassyApp fejlesztői és üzemeltetői ezekhez az adatokhoz semmilyen módon nem férnek hozzá. Az adatok
                az e-Kréta rendszer egy, az iskolavezetésnek szánt kimutatásából származnak, melyhez közvetlen
                hozzáférése a rendszer fejlesztőinek, karbantartóinak és üzemeltetőinek nincs. Ezáltal az érdemjegyek
                megtekintésére valamint módosítására a rendszer fejlesztőinek, karbantartóinak és üzemeltetőinek nincs
                lehetősége, valamint ezeket harmadik fél számára nem továbbítjuk.
            </Text>
            <Text mb="xs">
                Ha az adataiddal kapcsolatban bármilyen kérdésed van (mint például azok törlése), keress minket a{" "}
                <Anchor href="mailto:lovassyapp@gmail.com">lovassyapp@gmail.com</Anchor> címen.
            </Text>
            <Title size="h5" mb="sm">
                Cookie-k
            </Title>
            <Text mb="xs">
                Mint a legtöbb alkalmazás, a LovassyApp webes változata (Boardlight) is használ sütiket. Ezek csak és
                kizárólag az alkalmazás használatát segítik (például a jelszó megjegyzése), semmi féle adatgyűjtési
                céljuk nincs.
            </Text>
            <Text mb="md">Az alkalmazás használatával beleegyezel ezen sütik használatába.</Text>
            <Button fullWidth={true} onClick={close}>
                Igenis, Kapitány!
            </Button>
        </Modal>
    );
};
