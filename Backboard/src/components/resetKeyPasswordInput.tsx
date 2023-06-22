import { ActionIcon, Box, Group, PasswordInput, Popover, Progress, Text } from "@mantine/core";
import { IconCheck, IconLock, IconLockOpen, IconX } from "@tabler/icons-react";

import { openConfirmModal } from "@mantine/modals";
import { preferencesStore } from "../preferencesStore";
import { useSecurityStore } from "../stores/securityStore";
import { useState } from "react";

const PasswordRequirement = ({ meets, label }: { meets: boolean; label: string }) => {
    return (
        <Text
            color={meets ? "teal" : "red"}
            sx={{ display: "flex", alignItems: "center" }}
            mt={7}
            size="sm"
        >
            {meets ? <IconCheck size="0.9rem" /> : <IconX size="0.9rem" />} <Box ml={10}>{label}</Box>
        </Text>
    );
};

const requirements = [
    { re: /[0-9]/, label: "Tartalmaz számot" },
    { re: /[a-z]/, label: "Tartalmaz kisbetűt" },
    { re: /[A-Z]/, label: "Tartalmaz nagybetűt" },
    { re: /[$&+,:;=?@#|'<>.^*()%!-]/, label: "Tartalmaz szimbólumot" },
];

const getStrength = (password: string) => {
    let multiplier = password.length > 8 ? 0 : 1;

    requirements.forEach((requirement) => {
        if (!requirement.re.test(password)) {
            multiplier += 1;
        }
    });

    return Math.max(100 - (100 / (requirements.length + 1)) * multiplier, 10);
};

export const ResetKeyPasswordInput = (): JSX.Element => {
    const security = useSecurityStore();

    const saveSecurity = async () => {
        await preferencesStore.save();
    };

    const [popoverOpened, setPopoverOpened] = useState(false);
    const [disabled, setDisabled] = useState(security.resetKeyPassword.length !== 0);

    const checks = requirements.map((requirement, index) => (
        <PasswordRequirement key={index}
            label={requirement.label}
            meets={requirement.re.test(security.resetKeyPassword)} />
    ));

    const showWarning = () => {
        if (security.resetKeyPassword.length !== 0) {
            openConfirmModal({
                title: "Visszaállítási jelszó feloldása",
                children: (
                    <Text size="sm">A biztonsági jelszó feloldása nem ajánlott, csak
                        akkor változtasd meg a biztonsági jelszót, ha tudod mit csinálsz!</Text>
                ),
                labels: { confirm: "Igen, tudom mit csinálok", cancel: "Mégse" },
                confirmProps: { color: "red" },
                onConfirm: () => {
                    setDisabled(false);
                },
                onCancel: () => {
                    setDisabled(true);
                }
            });
        }
    };

    const strength = getStrength(security.resetKeyPassword);
    const color = strength === 100 ? "teal" : strength > 50 ? "yellow" : "red";

    return (
        <Popover opened={popoverOpened} position="bottom" width="target" transitionProps={{ transition: "pop" }}>
            <Popover.Target>
                <div
                    onFocusCapture={() => setPopoverOpened(true)}
                    onBlurCapture={() => setPopoverOpened(false)}
                >
                    <Group>
                        <Box sx={{ flex: 1 }}>
                            <PasswordInput
                                withAsterisk={true}
                                label="Visszaállítási jelszó"
                                description="Ez a jelszó a jövőben nem változtatható, amennyiben új értéket kapna a felhasználók nem tudnák megváltoztatni a jelszavukat"
                                value={security.resetKeyPassword}
                                onChange={(event) => security.setResetKeyPassword(event.currentTarget.value)}
                                onBlur={async () => {
                                    await saveSecurity();
                                    if (security.resetKeyPassword.length !== 0) {
                                        setDisabled(true);
                                    }
                                }}
                                disabled={disabled}
                            />
                        </Box>
                        <ActionIcon
                            variant="default"
                            size={36} sx={{ alignSelf: "flex-end" }}
                            disabled={!disabled} onClick={() => showWarning()}>
                            {disabled ? <IconLockOpen stroke={1} /> : <IconLock stroke={1} />}
                        </ActionIcon>
                    </Group>
                </div>
            </Popover.Target>
            <Popover.Dropdown>
                <Progress color={color} value={strength} size={5} mb="xs" />
                <PasswordRequirement label="Tartalmaz legalább 8 karaktert"
                    meets={security.resetKeyPassword.length > 8} />
                {checks}
            </Popover.Dropdown>
        </Popover>
    );
};
