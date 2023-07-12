import {
    ActionIcon,
    Button,
    Center,
    Group,
    Loader,
    Modal,
    SimpleGrid,
    Text,
    TextInput,
    Textarea,
    Title,
    createStyles,
    useMantineTheme,
} from "@mantine/core";
import { IconCheck, IconPlus } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";
import {
    useGetApiLoloRequests,
    useGetApiLoloRequestsOwn,
    usePostApiLoloRequests,
} from "../../../api/generated/features/lolo-requests/lolo-requests";

import { LoloRequestCard } from "../components/loloRequestCard";
import { LoloRequestStats } from "../components/loloRequestStats";
import { PermissionRequirement } from "../../../core/components/requirements/permissionsRequirement";
import { notifications } from "@mantine/notifications";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";
import { useQueryClient } from "@tanstack/react-query";

const useStyles = createStyles((theme) => ({
    center: {
        height: "100%",
    },
}));

const CreateLoloRequestModal = ({ opened, close }: { opened: boolean; close(): void }): JSX.Element => {
    const createLoloRequest = usePostApiLoloRequests();
    const queryClient = useQueryClient();

    const { queryKey: ownQueryKey } = useGetApiLoloRequestsOwn({}, { query: { enabled: false } });
    const { queryKey: allQueryKey } = useGetApiLoloRequests({}, { query: { enabled: false } });

    const form = useForm({
        initialValues: {
            title: "",
            body: "",
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await createLoloRequest.mutateAsync({
                data: values,
            });
            await queryClient.invalidateQueries({ queryKey: [ownQueryKey[0]] });
            await queryClient.invalidateQueries({ queryKey: [allQueryKey[0]] });
            notifications.show({
                title: "Kérvény létrehozva",
                message: "A kérvényed sikeresen létrehoztuk.",
                color: "green",
                icon: <IconCheck />,
            });
            close();
        } catch (err) {
            if (err instanceof ValidationError) handleValidationErrors(err, form);
        }
    });

    return (
        <Modal opened={opened} onClose={close} title="Új kérvény" size="lg">
            <form onSubmit={submit}>
                <TextInput label="Cím" required={true} {...form.getInputProps("title")} mb="sm" />
                <Textarea label="Törzsszöveg" required={true} {...form.getInputProps("body")} mb="md" />
                <Button type="submit" fullWidth={true}>
                    Létrehozás
                </Button>
            </form>
        </Modal>
    );
};

const OwnLoloRequestsPage = (): JSX.Element => {
    const { classes } = useStyles();
    const theme = useMantineTheme();

    const loloRequests = useGetApiLoloRequestsOwn();

    const [createLoloRequestModalOpened, { close: closeCreateLoloRequestModal, open: openCreateLoloRequestModal }] =
        useDisclosure(false);

    if (loloRequests.isLoading)
        return (
            <Center className={classes.center}>
                <Loader />
            </Center>
        );

    if (loloRequests.isError)
        return (
            <Center className={classes.center}>
                <Text color="red" align="center">
                    Hiba történt az adatok lekérésekor.
                </Text>
            </Center>
        );

    return (
        <>
            <CreateLoloRequestModal opened={createLoloRequestModalOpened} close={closeCreateLoloRequestModal} />
            <Title mb="md">Statisztikák</Title>
            <SimpleGrid cols={2} breakpoints={[{ maxWidth: theme.breakpoints.sm, cols: 1, spacing: "sm" }]}>
                <LoloRequestStats data={loloRequests.data} />
            </SimpleGrid>
            <Group position="apart" align="baseline" my="md" spacing={0}>
                <Title>Kérvények</Title>
                <PermissionRequirement permissions={["Shop.CreateLoloRequest"]}>
                    <ActionIcon variant="transparent" color="dark" onClick={() => openCreateLoloRequestModal()}>
                        <IconPlus />
                    </ActionIcon>
                </PermissionRequirement>
            </Group>
            <SimpleGrid
                cols={4}
                breakpoints={[
                    { maxWidth: theme.breakpoints.md, cols: 3, spacing: "md" },
                    { maxWidth: theme.breakpoints.sm, cols: 2, spacing: "sm" },
                    { maxWidth: theme.breakpoints.xs, cols: 1, spacing: "sm" },
                ]}
            >
                {loloRequests.data?.map((loloRequest) => (
                    <LoloRequestCard key={loloRequest.id} loloRequest={loloRequest} />
                ))}
            </SimpleGrid>
        </>
    );
};

export default OwnLoloRequestsPage;
