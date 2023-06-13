import { Button, FileInput, Stack, Title } from "@mantine/core";

import { open } from "@tauri-apps/api/dialog";
import { useState } from "react";

const GradeImportPage = (): JSX.Element => {
    const [fileValue, setFileValue] = useState<File | null>(null);
    const [filePath, setFilePath] = useState<string | null>(null);
    const [fileError, setFileError] = useState<string | null>(null);
    const [fileLoading, setFileLoading] = useState<boolean>(false);
    const [fileDisabled, setFileDisabled] = useState<boolean>(false);

    const importGrades = async () => {
        if (filePath === null) {
            setFileError("Nincs kiválasztva fájl");
            return;
        }
        setFileError(null);
        setFileLoading(true);
        setFileDisabled(true);
        // call rust function
        console.log(filePath);
        setTimeout(() => {
            setFileLoading(false);
            setFileDisabled(false);
        }, 1000);
    };

    return (
        <Stack spacing="xs">
            <Title order={2} size="h1">Importálás</Title>
            <FileInput onClick={async (event) => {
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
                    setFilePath(path as string);
                    setFileValue(new File([], (path as string).split("/").pop() as string));
                }
            }}
            value={fileValue}
            onChange={(value) => {
                if (value === null) {
                    setFilePath(null);
                    setFileValue(null);
                }
            }}
            label="Jegyek"
            description="A diákok jegyeit tartalmazó Excel táblázat"
            placeholder="Tanulok_evkozi_jegyei_XXXXXXXX.xlsx"
            withAsterisk={true}
            clearable={true}
            disabled={fileDisabled}
            error={fileError} />
            <Button loading={fileLoading}
                variant="default"
                sx={{ alignSelf: "center" }}
                mt="xs"
                onClick={async () => await importGrades()}>
                    Importálás
            </Button>
        </Stack>
    );
};

export default GradeImportPage;
