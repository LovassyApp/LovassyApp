import { ActionIcon, AppShell, Divider, Group, Header, Navbar, Stack, Title, UnstyledButton, createStyles, rem } from "@mantine/core";
import { IconDatabaseImport, IconKey, IconMinus, IconSatellite, IconSettings, IconSquare, IconX } from "@tabler/icons-react";
import { Outlet, useLocation, useNavigate } from "react-router-dom";

import { ColorSchemeToggle } from "../../components/colorSchemeToggle";
import { appWindow } from "@tauri-apps/api/window";
import { useState } from "react";

const useStyles = createStyles((theme) => ({
    windowHeader: {
        borderBottom: 0,
    },
    icon: {
        color: theme.colorScheme === "dark" ? theme.white : theme.black,
    },
    navbarLink: {
        width: rem(36),
        height: rem(36),
        borderRadius: theme.radius.md,
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        color: theme.colorScheme === "dark" ? theme.white : theme.black,
        opacity: 0.85,

        "&:hover": {
            opacity: 1,
            backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[0],
        }
    },
    navbarActive: {
        opacity: 1,
        "&, &:hover": {
            backgroundColor: theme.fn.variant({ variant: "filled", color: theme.primaryColor }).background,
            color: theme.white
        },
    },
    navbarSection: {
        display: "flex",
        alignItems: "center",
    },
    navbar: {
        borderRight: 0,
    }
}));

const links = [
    { icon: IconDatabaseImport, path: "/" },
    { icon: IconKey, path: "/security" },
    { icon: IconSatellite, path: "/status" },
];

interface NavbarLinkProps {
    icon: React.FC<any>;
    active?: boolean;
    onClick?(): void;
  }

const NavbarLink = ({ icon: Icon, active, onClick }: NavbarLinkProps) => {
    const { classes, cx } = useStyles();

    return (
        <UnstyledButton onClick={onClick} className={cx(classes.navbarLink, { [classes.navbarActive]: active })}>
            <Icon size="1.2rem" stroke={1.5} />
        </UnstyledButton>
    );
};

const WindowNavbar = () => {
    const { classes } = useStyles();

    const [active, setActive] = useState(0);

    const location = useLocation();
    const navigate = useNavigate();

    const displayLinks = links.map((link, index) => (
        <NavbarLink
            {...link}
            key={index}
            active={index === active}
            onClick={() => {
                setActive(index);
                navigate(link.path);
            }}
        />
    ));

    return (
        <Navbar width={{ base: 60 }} className={classes.navbar}>
            <Navbar.Section grow={true}>
                <Stack justify="center" spacing="xs" className={classes.navbarSection}>
                    {displayLinks}
                </Stack>
            </Navbar.Section>
            <Navbar.Section mb="sm">
                <Stack justify="center" spacing="xs" className={classes.navbarSection}>
                    <NavbarLink icon={IconSettings} onClick={() => {
                        setActive(links.length);
                        navigate("/settings");
                    }} active={active == links.length} />
                </Stack>
            </Navbar.Section>
        </Navbar>
    );
};

const WindowHeader = () => {
    const { classes } = useStyles();

    return (
        <Header className={classes.windowHeader} height={50} fixed={true}>
            <Group data-tauri-drag-region={true} position="apart" p="xs">
                <Title order={1} size="h3" variant="gradient">LovassyApp</Title>
                <Group position="right" align="center">
                    <ColorSchemeToggle />
                    <Divider orientation="vertical" />
                    <ActionIcon size="md"
                        variant="light"
                        className={classes.icon}
                        onClick={() => appWindow.minimize()}>
                        <IconMinus stroke={1.5} size={16} />
                    </ActionIcon>
                    <ActionIcon
                        size="md"
                        variant="light"
                        className={classes.icon}
                        onClick={() => appWindow.toggleMaximize()}>
                        <IconSquare stroke={1.5} size={16} />
                    </ActionIcon>
                    <ActionIcon
                        size="md"
                        variant="light"
                        className={classes.icon}
                        onClick={() => appWindow.close()}>
                        <IconX stroke={1.5} size={16} />
                    </ActionIcon>
                </Group>
            </Group>
        </Header>
    );
};

const WindowLayout = (): JSX.Element => {
    return (
        <AppShell header={<WindowHeader />} navbar={<WindowNavbar />}>
            <Outlet />
        </AppShell>
    );
};

export default WindowLayout;
