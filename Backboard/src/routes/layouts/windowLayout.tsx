import { AppShell, Navbar, Stack, UnstyledButton, createStyles, rem } from "@mantine/core";
import { IconDatabaseImport, IconKey, IconSatellite, IconSettings } from "@tabler/icons-react";
import { Outlet, useNavigate } from "react-router-dom";

import { ColorSchemeToggle } from "../../components/colorSchemeToggle";
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
        paddingTop: 10,
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
                    <ColorSchemeToggle />
                    <NavbarLink icon={IconSettings} onClick={() => {
                        setActive(links.length);
                        navigate("/settings");
                    }} active={active == links.length} />
                </Stack>
            </Navbar.Section>
        </Navbar>
    );
};

const WindowLayout = (): JSX.Element => {
    return (
        <AppShell navbar={<WindowNavbar />} padding="xs">
            <Outlet />
        </AppShell>
    );
};

export default WindowLayout;
