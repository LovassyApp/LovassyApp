import {
    Button,
    Center,
    Modal,
    Text,
    TextInput,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import { usePostApiIncrement } from "../../../api/generated/features/increment/increment";
import { IncrementIncrementYearRequestBody } from "../../../api/generated/models";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";

const useStyles = createStyles(() => ({
    center: {
        height: "100%",
    },
}));

const IncrementYearModal = ({ opened, close }: { opened: boolean; close(): void }): JSX.Element => {
    const incrementYear = usePostApiIncrement();

    const form = useForm({
        initialValues: {
            confirmationText: "",
        },
        validate: {
            confirmationText: (value) => (value !== "Mit sütsz, kis szűcs? Sós húst sütsz, kis szűcs?" ? "A megerősítőszöveg helytelen!" : null)
        }
    });

    const submit = form.onSubmit(async (values) => {
        form.reset();
        try {
            await incrementYear.mutateAsync({
                data: values as IncrementIncrementYearRequestBody,
            });
            notifications.show({
                title: "Tanév léptetve",
                message: "A tanév léptetése sikeres.",
                color: "green",
                icon: <IconCheck />,
            });
            close();
        } catch (err) {
            if (err instanceof ValidationError) handleValidationErrors(err, form);
        }
    });

    return (
        <Modal opened={opened} onClose={close} title="Tanév léptetése" size="lg">
            <form onSubmit={submit}>
                <Text mb="md">A folytatáshoz írd be az alábbi mezőbe a következő szöveget: „<i>Mit sütsz, kis szűcs? Sós húst sütsz, kis szűcs?</i>”</Text>
                <TextInput label="Megerősítő szöveg" required={true} {...form.getInputProps("confirmationText")} mb="sm" placeholder="Mit sütsz, kis szűcs? Sós húst sütsz, kis szűcs?"/>
                <Button type="submit" fullWidth={true} mt="sm" color="red" loading={incrementYear.isLoading}>
                    Tanév léptetése
                </Button>
            </form>
        </Modal>
    );
};

const IncrementYearPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();


    const [incrementYearModalOpened, { close: closeIncrementYearModal, open: openIncrementYearModal }] = useDisclosure(false);

    return (
        <>
            <IncrementYearModal opened={incrementYearModalOpened} close={closeIncrementYearModal} />
            <Title mb="md">Tanév léptetése</Title>
            <Text color="dimmed">
                Az alábbi gombbal új tanévet lehet kezdeni. Ekkor az adatbázisból törlődnek az előző tanévben szerzett osztályzatok, lolók; az évfolyamok léptetésre kerülnek.
            </Text>
            <Text color="dimmed" mb="md">A gombot használd körültekintően!</Text>
            <Center>
                <Button color="red" onClick={() => openIncrementYearModal()}>Tanév léptetése</Button>
            </Center>
        </>
    );
};

export default IncrementYearPage;
