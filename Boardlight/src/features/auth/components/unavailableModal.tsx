import { Anchor, Button, Modal, Text, TextInput } from "@mantine/core";
import { IconCheck, IconMail } from "@tabler/icons-react";
import { ValidationError, handleValidationErrors } from "../../../helpers/apiHelpers";

import { modals } from "@mantine/modals";
import { notifications } from "@mantine/notifications";
import { useForm } from "@mantine/form";
import { useNavigate } from "react-router-dom";
import { usePostApiStatusNotifyOnResetKeyPasswordSet } from "../../../api/generated/features/status/status";

export const UnavailableModal = ({ opened }: { opened: boolean }): JSX.Element => {
    const navigate = useNavigate();
    const notifyOnResetKeyPasswordSet = usePostApiStatusNotifyOnResetKeyPasswordSet();

    const backToLogin = () => {
        modals.closeAll();
        navigate("/auth/login");
    };

    const form = useForm({
        initialValues: {
            email: "",
        },
    });

    const submit = form.onSubmit(async (values) => {
        try {
            await notifyOnResetKeyPasswordSet.mutateAsync({ data: values });
            notifications.show({
                title: "Felírtunk!",
                message: "Mostmár biztos hogy kapni fogsz egy emailt amint ez a funkció elérhető.",
                color: "green",
                icon: <IconCheck />,
            });
        } catch (err) {
            if (err instanceof ValidationError) handleValidationErrors(err, form);
        }
    });

    return (
        <Modal
            opened={opened}
            // eslint-disable-next-line @typescript-eslint/no-empty-function
            onClose={() => {}}
            size="lg"
            withCloseButton={false}
            closeOnEscape={false}
            closeOnClickOutside={false}
        >
            <Text mb="sm">Sajnáljuk, de jelenleg ez a funkció nem elérhető...</Text>
            <Text weight="bold" mb="sm">
                Miért van ez?
            </Text>
            <Text mb="sm">
                A felhasználóink adatainak védelme érdekében minden személyes adatot titkosítunk a felhasználó
                jelszavával és az iskolavezőség által megadott visszaállítási jelszóval. A biztonság érdekében a
                visszaállítási jelszót nem tárolhatjuk a LovassyApp szerverein, ezért azt minden újraindítás után újra
                be kell állítani, és ez a legutóbbi óta még nem történt meg.
            </Text>
            <form onSubmit={submit}>
                <TextInput
                    label="Sebaj! Szeretnék értesítést amikor elérhető lesz"
                    description="Ide bármilyen email címet megadhatsz, nem kell hogy az legyen amelyikkel regisztráltál."
                    icon={<IconMail size={20} stroke={1.5} />}
                    required={true}
                    mb="sm"
                    {...form.getInputProps("email")}
                />
                <Button type="submit" fullWidth={true} mb="md">
                    Írjatok fel!
                </Button>
            </form>
            <Text align="center" size="sm">
                <Anchor component="button" onClick={backToLogin}>
                    Vissza a bejelentkezéshez
                </Anchor>
            </Text>
        </Modal>
    );
};
