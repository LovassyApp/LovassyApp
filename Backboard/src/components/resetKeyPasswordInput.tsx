import { Box, PasswordInput, Popover, Progress, Text, createStyles } from "@mantine/core";
import { IconCheck, IconX } from "@tabler/icons-react";

import { useState } from "react";

const useStyles = createStyles((theme) => ({}));

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
    const { classes } = useStyles();

    const [popoverOpened, setPopoverOpened] = useState(false);
    const [value, setValue] = useState("");
    const checks = requirements.map((requirement, index) => (
        <PasswordRequirement key={index} label={requirement.label} meets={requirement.re.test(value)} />
    ));

    const strength = getStrength(value);
    const color = strength === 100 ? "teal" : strength > 50 ? "yellow" : "red";

    return (
        <Popover opened={popoverOpened} position="bottom" width="target" transitionProps={{ transition: "pop" }}>
            <Popover.Target>
                <div
                    onFocusCapture={() => setPopoverOpened(true)}
                    onBlurCapture={() => setPopoverOpened(false)}
                >
                    <PasswordInput
                        withAsterisk={true}
                        label="Visszaállítási jelszó"
                        description="Ez a jelszó a jövőben nem változtatható, amennyiben új értéket kapna a felhasználók nem tudnák megváltoztatni a jelszavukat"
                        value={value}
                        onChange={(event) => setValue(event.currentTarget.value)}
                    />
                </div>
            </Popover.Target>
            <Popover.Dropdown>
                <Progress color={color} value={strength} size={5} mb="xs" />
                <PasswordRequirement label="Tartalmaz legalább 8 karaktert" meets={value.length > 8} />
                {checks}
            </Popover.Dropdown>
        </Popover>
    );
};
