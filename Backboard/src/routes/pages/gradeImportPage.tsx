import { Button, FileInput, Group, Progress, Stack, Text, Title } from "@mantine/core";
import { UnlistenFn, listen } from "@tauri-apps/api/event";
import { useEffect, useState } from "react";

import { IconCheck } from "@tabler/icons-react";
import { invoke } from "@tauri-apps/api";
import { notifications } from "@mantine/notifications";
import { open } from "@tauri-apps/api/dialog";
import { preferencesStore } from "../../preferencesStore";
import { useSecurityStore } from "../../stores/securityStore";
import { useSettingStore } from "../../stores/settingsStore";

const GradeImportPage = (): JSX.Element => {
    const security = useSecurityStore();
    const settings = useSettingStore();

    const [gradesFileValue, setGradesFileValue] = useState<File | null>(null);
    const [gradesFilePath, setGradesFilePath] = useState<string | null>(null);
    const [gradesFileError, setGradesFileError] = useState<string | null>(null);

    const [studentsFileValue, setStudentsFileValue] = useState<File | null>(null);
    const [studentsFilePath, setStudentsFilePath] = useState<string | null>(null);

    const [fileLoading, setFileLoading] = useState<boolean>(false);
    const [fileDisabled, setFileDisabled] = useState<boolean>(false);

    const [userCount, setUserCount] = useState<number | undefined>(undefined);
    const [progress, setProgress] = useState<number>(0);

    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        let unlistenProgress: UnlistenFn;
        let unlistenUserCount: UnlistenFn;

        (async () => {
            unlistenProgress = await listen("import-progress", (event) => {
                setProgress(event.payload as number);
            });
            unlistenUserCount = await listen("import-users", (event) => {
                console.log(event.payload);
                setUserCount(event.payload as number);
            });
        })();

        return () => {
            if (unlistenProgress) unlistenProgress();
            if (unlistenUserCount) unlistenUserCount();
        };
    }, []);

    const importGrades = async () => {
        if (gradesFilePath === null) {
            setGradesFileError("Nincs kiválasztva fájl");
            return;
        }
        setGradesFileError(null);
        setFileLoading(true);
        setFileDisabled(true);
        setError(null);
        try {
            await invoke("import_grades", {
                gradesFilePath,
                studentsFilePath,
                blueboardUrl: settings.blueboardUrl,
                importKey: settings.importKey,
                resetKeyPassword: security.resetKeyPassword,
                updateResetKeyPassword: security.updateResetKeyPasswordOnImport,
            });

            notifications.show({
                id: "grades-imported",
                withCloseButton: true,
                autoClose: 3000,
                title: "Sikeres feltöltés",
                message: "A felhasználók jegyei sikeresen feltöltve!",
                icon: <IconCheck />,
                color: "green",
            });
        } catch (error) {
            if (error === "401") setError("Hibás import kulcs!");
            else if (error === "429") setError("Túl sok kérelem rövid idő alatt!");
            else if (error === "404") setError("Egy megadott felhasználó nem létezik!");
            else if (error === "500") setError("Szerver hiba történt!");
            else if (error === "unknown") setError("Ismeretlen reqwest hiba történt!");
            else setError("Nem sikerült feltölteni a jegyeket!");
        }

        setTimeout(() => {
            setFileLoading(false);
            setFileDisabled(false);
            setProgress(0);
        }, 500);
    };

    return (
        <Stack spacing="xs">
            <Title order={2} size="h1">
                Importálás
            </Title>
            <FileInput
                onClick={async (event) => {
                    event.preventDefault();
                    setFileDisabled(true);
                    const path = await open({
                        multiple: false,
                        filters: [
                            {
                                name: "Excel táblázat",
                                extensions: ["xlsx"],
                            },
                        ],
                    });
                    setFileDisabled(false);
                    if (path) {
                        setGradesFilePath(path as string);
                        setGradesFileValue(new File([], (path as string).split("/").pop() as string));
                    }
                }}
                value={gradesFileValue}
                onChange={(value) => {
                    if (value === null) {
                        setGradesFilePath(null);
                        setGradesFileValue(null);
                    }
                }}
                label="Jegyek"
                description="A diákok jegyeit tartalmazó Excel táblázat"
                placeholder="Tanulok_evkozi_jegyei_XXXXXXXX.xlsx"
                withAsterisk={true}
                clearable={true}
                disabled={fileDisabled}
                error={gradesFileError}
            />
            <FileInput
                onClick={async (event) => {
                    event.preventDefault();
                    setFileDisabled(true);
                    const path = await open({
                        multiple: false,
                        filters: [
                            {
                                name: "Excel táblázat",
                                extensions: ["xlsx"],
                            },
                        ],
                    });
                    setFileDisabled(false);
                    if (path) {
                        setStudentsFilePath(path as string);
                        setStudentsFileValue(new File([], (path as string).split("/").pop() as string));
                    }
                }}
                value={studentsFileValue}
                onChange={(value) => {
                    if (value === null) {
                        setStudentsFilePath(null);
                        setStudentsFileValue(null);
                    }
                }}
                label="Tanulók"
                description="A tanulók adatait (OM azonosító, név, osztály) tartalmazó Excel táblázat"
                clearable={true}
                disabled={fileDisabled}
            />
            {fileLoading && <Progress value={progress} label={`${progress}%`} size="xl" radius="xl" mt="xs" />}
            <Button
                loading={fileLoading}
                variant="default"
                sx={{ alignSelf: "center" }}
                mt="xs"
                onClick={async () => await importGrades()}
            >
                Importálás
            </Button>
            {error && (
                <Text color="red" size="sm" sx={{ alignSelf: "center" }}>
                    {error}
                </Text>
            )}
            <Group position="apart">
                <Text size="sm">Felhasználók száma:</Text>
                <Text size="sm" weight="bold">
                    {userCount ?? "Ismeretlen"}
                </Text>
            </Group>
        </Stack>
    );
};

export default GradeImportPage;
