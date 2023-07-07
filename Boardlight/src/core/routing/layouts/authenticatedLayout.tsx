import {
    Accordion,
    Anchor,
    AppShell,
    Avatar,
    Box,
    Burger,
    Center,
    Container,
    Divider,
    Drawer,
    Group,
    Header,
    Loader,
    Menu,
    Modal,
    Stack,
    Text,
    UnstyledButton,
    createStyles,
    rem,
    useMantineColorScheme,
} from "@mantine/core";
import {
    IconChevronDown,
    IconInfoCircle,
    IconInfoSquareRounded,
    IconLogout,
    IconPalette,
    IconUserCircle,
} from "@tabler/icons-react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { useDeleteApiAuthLogout, useGetApiAuthControl } from "../../../api/generated/features/auth/auth";

import { FeaturesConditional } from "../../components/conditionals/featuresConditional";
import { PermissionsConditional } from "../../components/conditionals/permissionsConditional";
import { QueryClientProvider } from "@tanstack/react-query";
import { modals } from "@mantine/modals";
import { queryClient } from "../../../main";
import { useAuthStore } from "../../stores/authStore";
import { useDisclosure } from "@mantine/hooks";
import { useGetApiStatusVersion } from "../../../api/generated/features/status/status";

const HEADER_HEIGHT = rem(60);

const useStyles = createStyles((theme) => ({
    content: {
        height: HEADER_HEIGHT,
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
    },
    links: {
        [theme.fn.smallerThan("md")]: {
            display: "none",
        },
    },
    link: {
        display: "block",
        lineHeight: 1,
        padding: `${rem(8)} ${rem(12)}`,
        borderRadius: theme.radius.sm,
        textDecoration: "none",
        color: theme.colorScheme === "dark" ? theme.colors.dark[0] : theme.colors.gray[7],
        fontSize: theme.fontSizes.sm,
        fontWeight: 500,

        "&:hover": {
            backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
        },
    },
    linkLabel: {
        textAlign: "center",
        marginRight: rem(5),
    },
    burger: {
        [theme.fn.largerThan("md")]: {
            display: "none",
        },
    },
    avatar: {
        cursor: "pointer",
    },
    avatarPlaceholder: {
        color: theme.colorScheme === "dark" ? theme.white : theme.black,
    },
    accordionLink: {
        display: "block",
        lineHeight: 1,
        padding: `${rem(8)} ${rem(12)}`,
        textDecoration: "none",
        color: theme.colorScheme === "dark" ? theme.colors.dark[0] : theme.colors.gray[7],
        fontSize: theme.fontSizes.md,
        textAlign: "center",
    },
}));

const UserInformationModal = ({ opened, close }: { opened: boolean; close(): void }) => {
    const control = useGetApiAuthControl({ query: { retry: 0 } });

    if (control.isLoading)
        return (
            <Modal opened={opened} onClose={close} title="Fiók információk" size="xl">
                <Center>
                    <Loader size="xl" />
                </Center>
            </Modal>
        );

    // Error shouldn't be possible

    return (
        <Modal opened={opened} onClose={close} title="Fiók információk" size="xl">
            <Group position="apart">
                <Text>Név:</Text>
                <Text weight="bold">{control.data.user.realName ?? "Ismeretlen"}</Text>
            </Group>
            <Group position="apart">
                <Text>Osztály:</Text>
                <Text weight="bold">{control.data.user.class ?? "Ismeretlen"}</Text>
            </Group>
            <Group position="apart">
                <Text>Felhasználónév:</Text>
                <Text weight="bold">{control.data.user.name}</Text>
            </Group>
            <Group position="apart">
                <Text>Email cím:</Text>
                <Text weight="bold">{control.data.user.email}</Text>
            </Group>
            <Divider my="md" />
            <Text>Elérhető funkciók:</Text>
            <Text weight="bold">{control.data.features.join(", ")}</Text>
            <Divider my="md" />
            <Text>Felhasználói csoportok:</Text>
            <Text weight="bold">{control.data.userGroups.join(", ")}</Text>
            <Divider my="md" />
            <Group position="apart">
                <Text>Adminisztrátor:</Text>
                <Text weight="bold">{control.data.isSupeUser ? "Igen" : "Nem"}</Text>
            </Group>
            <Text>Jogosultságok:</Text>
            <Text weight="bold">{control.data.permissions.join(", ")}</Text>
        </Modal>
    );
};

const SystemInformationModal = ({ opened, close }: { opened: boolean; close(): void }) => {
    const version = useGetApiStatusVersion({ SendOk: true, SendMOTD: true });

    if (version.isLoading)
        return (
            <Modal opened={opened} onClose={close} title="Rendszer információk" size="lg">
                <Center>
                    <Loader />
                </Center>
            </Modal>
        );

    if (version.isError)
        return (
            <Modal opened={opened} onClose={close} title="Rendszer információk" size="lg">
                <Center>
                    <Text color="red">Hiba történt a szerver verziójának lekérdezése közben!</Text>
                </Center>
            </Modal>
        );

    return (
        <Modal opened={opened} onClose={close} title="Rendszer információk" size="lg">
            <Group position="apart">
                <Text>Frontend verzió:</Text>
                <Text weight="bold">Boardlight {import.meta.env.PACKAGE_VERSION}</Text>
            </Group>
            <Group position="apart">
                <Text>Backend verzió:</Text>
                <Text weight="bold">Blueboard {version.data.version}</Text>
            </Group>
            <Group position="apart">
                <Text>.NET verzió:</Text>
                <Text weight="bold">{version.data.dotNetVersion}</Text>
            </Group>
            <Divider my="md" />
            <Group position="apart">
                <Text>Fejlesztők:</Text>
                <Text weight="bold">Gyimesi Máté (minigyima), Ocskó Nándor (Xeretis)</Text>
            </Group>
            <Group position="apart">
                <Text>Forráskód:</Text>
                <Anchor href={version.data.repository} weight="bold">
                    Github
                </Anchor>
            </Group>
            <Divider my="md" />
            <Text>MOTD:</Text>
            <Text weight="bold">{version.data.motd}</Text>
        </Modal>
    );
};

const HeaderAvatar = () => {
    const { classes } = useStyles();

    const { toggleColorScheme } = useMantineColorScheme();

    const logout = useDeleteApiAuthLogout();

    const [userInformationModalOpened, { close: closeUserInformationModal, open: openUserInformationModal }] =
        useDisclosure();
    const [systemInformationModalOpened, { close: closeSystemInformationModal, open: openSystemInformationModal }] =
        useDisclosure();
    const setAccessToken = useAuthStore((state) => state.setAccessToken);

    const doLogout = async () => {
        try {
            await logout.mutateAsync();
        } finally {
            setAccessToken(undefined);
        }
    };

    return (
        <>
            <UserInformationModal opened={userInformationModalOpened} close={closeUserInformationModal} />
            <SystemInformationModal opened={systemInformationModalOpened} close={closeSystemInformationModal} />
            <Menu withArrow={true} withinPortal={true} closeOnItemClick={true}>
                <Menu.Target>
                    <Avatar
                        className={classes.avatar}
                        radius="xl"
                        classNames={{ placeholder: classes.avatarPlaceholder }}
                    >
                        <IconUserCircle size={24} stroke={1.5} />
                    </Avatar>
                </Menu.Target>
                <Menu.Dropdown>
                    <Menu.Label>Fiók</Menu.Label>
                    <Menu.Item
                        icon={<IconInfoCircle size={14} />}
                        color="blue"
                        onClick={() => openUserInformationModal()}
                    >
                        Fiók információk
                    </Menu.Item>
                    <Menu.Item icon={<IconLogout size={14} />} color="red" onClick={async () => await doLogout()}>
                        Kijelentkezés
                    </Menu.Item>
                    <Menu.Divider />
                    <Menu.Label>Rendszer</Menu.Label>
                    <Menu.Item icon={<IconPalette size={14} />} onClick={() => toggleColorScheme()}>
                        Téma megváltoztatása
                    </Menu.Item>
                    <Menu.Item icon={<IconInfoSquareRounded size={14} />} onClick={() => openSystemInformationModal()}>
                        Rendszer információk
                    </Menu.Item>
                </Menu.Dropdown>
            </Menu>
        </>
    );
};

interface AuthenticatedHeaderProps {
    links: Array<
        | { link: string; label: string; features?: string[]; permissions?: string[]; links?: undefined }
        | {
              label: string;
              features?: string[];
              permissions?: string[];
              links: Array<{ link: string; label: string; features?: string[]; permissions?: string[] }>;
          }
    >;
    toggleDrawer?(): void;
}

const AuthenticatedHeader = ({ links, toggleDrawer }: AuthenticatedHeaderProps) => {
    const { classes } = useStyles();

    const items = links.map((link) => {
        const menuItems = link.links?.map((item) => (
            <FeaturesConditional key={item.label} features={item.features}>
                <PermissionsConditional permissions={item.permissions}>
                    <Menu.Item component={Link} to={item.link}>
                        {item.label}
                    </Menu.Item>
                </PermissionsConditional>
            </FeaturesConditional>
        ));

        if (menuItems) {
            return (
                <FeaturesConditional key={link.label} features={link.features}>
                    <PermissionsConditional permissions={link.permissions}>
                        <Menu
                            trigger="hover"
                            transitionProps={{ transition: "fade", duration: 100 }}
                            withinPortal={true}
                        >
                            <Menu.Target>
                                <Text className={classes.link}>
                                    <Center>
                                        <span className={classes.linkLabel}>{link.label}</span>
                                        <IconChevronDown size={rem(12)} stroke={1.5} />
                                    </Center>
                                </Text>
                            </Menu.Target>
                            <Menu.Dropdown>{menuItems}</Menu.Dropdown>
                        </Menu>
                    </PermissionsConditional>
                </FeaturesConditional>
            );
        }

        return (
            <FeaturesConditional key={link.label} features={link.features}>
                <PermissionsConditional permissions={link.permissions}>
                    {/* @ts-ignore */}
                    <Link to={link.link} className={classes.link}>
                        {link.label}
                    </Link>
                </PermissionsConditional>
            </FeaturesConditional>
        );
    });

    return (
        <Header height={HEADER_HEIGHT} sx={{ borderBottom: 0 }}>
            <Container className={classes.content} fluid={true}>
                <Group>
                    <Text size="xl" weight={700} variant="gradient">
                        LovassyApp
                    </Text>
                </Group>
                <Group spacing={5} className={classes.links}>
                    {items}
                </Group>
                <Group>
                    <HeaderAvatar />
                    <Burger opened={false} onClick={toggleDrawer} className={classes.burger} size="sm" />
                </Group>
            </Container>
        </Header>
    );
};

interface AuthenticatedDrawerProps {
    links: Array<
        | { link: string; label: string; features?: string[]; permissions?: string[]; links?: undefined }
        | {
              label: string;
              features?: string[];
              permissions?: string[];
              links: Array<{ link: string; label: string; features?: string[]; permissions?: string[] }>;
          }
    >;
    drawerOpened: boolean;
    closeDrawer?(): void;
}

const AuthenticatedDrawer = ({ links, drawerOpened, closeDrawer }: AuthenticatedDrawerProps) => {
    const { classes } = useStyles();

    const navigate = useNavigate();

    const doNavigate = (link: string) => {
        closeDrawer();
        navigate(link);
    };

    const items = links.map((link) => {
        const accordionItems = link.links?.map((item) => (
            <FeaturesConditional key={item.label} features={item.features}>
                <PermissionsConditional permissions={item.permissions}>
                    <UnstyledButton onClick={() => doNavigate(item.link)} className={classes.accordionLink}>
                        {item.label}
                    </UnstyledButton>
                </PermissionsConditional>
            </FeaturesConditional>
        ));

        if (accordionItems) {
            return (
                <FeaturesConditional key={link.label} features={link.features}>
                    <PermissionsConditional permissions={link.permissions}>
                        <Accordion
                            variant="filled"
                            chevron={false}
                            styles={{
                                chevron: { marginLeft: 0 },
                                label: { padding: `${rem(8)} ${rem(16)}` },
                                content: { padding: `${rem(8)} ${rem(16)}` },
                            }}
                            w="100%"
                            chevronSize={0}
                        >
                            <Accordion.Item value={link.label}>
                                <Accordion.Control>
                                    <Text>
                                        <Center>
                                            <span className={classes.linkLabel}>{link.label}</span>
                                            <IconChevronDown size={rem(12)} stroke={1.5} />
                                        </Center>
                                    </Text>
                                </Accordion.Control>
                                <Accordion.Panel>
                                    <Stack align="center">{accordionItems}</Stack>
                                </Accordion.Panel>
                            </Accordion.Item>
                        </Accordion>
                    </PermissionsConditional>
                </FeaturesConditional>
            );
        }

        return (
            <FeaturesConditional key={link.label} features={link.features}>
                <PermissionsConditional permissions={link.permissions}>
                    {/* @ts-ignore */}
                    <UnstyledButton onClick={() => doNavigate(link.link)} className={classes.accordionLink}>
                        {link.label}
                    </UnstyledButton>
                </PermissionsConditional>
            </FeaturesConditional>
        );
    });

    return (
        <Drawer
            opened={drawerOpened}
            onClose={closeDrawer}
            position="right"
            size="100%"
            title="Navigáció"
            closeButtonProps={{ size: "md" }}
        >
            <Stack align="center">{items}</Stack>
        </Drawer>
    );
};

const shopIndexPermissions = [
    "Shop.IndexOwnLolos",
    "Shop.IndexStoreProducts",
    "Shop.IndexOwnOwnedItems",
    "Shop.IndexOwnLoloRequests",
];

const administratorIndexPermissions = [
    "Auth.IndexPermissions",
    "Auth.IndexUserGroups",
    "Import.IndexImportKeys",
    "Shop.IndexOwnedItems", // This is the permission for indexing every owned item, not just the user's own
    "Shop.IndexLolos", // This is the permission for indexing every lolo, not just the user's own
    "Users.IndexUsers",
];

const managerIndexPermissions = ["Shop.IndexProducts", "Shop.IndexLoloRequests", "Shop.IndexQRCodes"];

const links = [
    {
        link: "/",
        label: "Kezdőlap",
    },
    {
        link: "/grades",
        label: "Jegyek",
        permissions: ["School.IndexGrades"],
        features: ["School"],
    },
    {
        label: "Loló rendszer",
        permissions: shopIndexPermissions,
        features: ["Shop"],
        links: [
            {
                link: "/shop/own-coins",
                label: "Érmék",
                permissions: ["Shop.IndexOwnLolos"],
            },
            {
                link: "/shop/own-lolo-requests",
                label: "Saját kérvények",
                permissions: ["Shop.IndexOwnLoloRequests"],
            },
            {
                link: "/shop",
                label: "Bazár",
                permissions: ["Shop.IndexStoreProducts"],
            },
            {
                link: "/shop/own-owned-items",
                label: "Kincstár",
                permissions: ["Shop.IndexOwnOwnedItems"],
            },
        ],
    },
    {
        label: "Menedzsment",
        permissions: managerIndexPermissions,
        features: ["Shop"], // This might be temporary as more management features are added
        links: [
            {
                link: "/shop/lolo-requests",
                label: "Kérvények",
                permissions: ["Shop.IndexLoloRequests"],
            },
            {
                link: "/shop/products",
                label: "Termékek",
                permissions: ["Shop.IndexProducts"],
            },
            {
                link: "/shop/qr-codes",
                label: "QR kódok",
                permissions: ["Shop.IndexQRCodes"],
            },
        ],
    },
    {
        label: "Adminisztráció",
        permissions: administratorIndexPermissions,
        links: [
            {
                link: "/users",
                label: "Felhasználók",
                permissions: ["Users.IndexUsers"],
                features: ["Users"],
            },
            {
                link: "/auth/user-groups",
                label: "Csoportok",
                permissions: ["Auth.IndexUserGroups"],
            },
            {
                link: "/import/import-keys",
                label: "Import kulcsok",
                permissions: ["Import.IndexImportKeys"],
                features: ["Import"],
            },
            {
                link: "/shop/coins",
                label: "Összes érme",
                permissions: ["Shop.IndexLolos"],
                features: ["Shop"],
            },
            {
                link: "/shop/owned-items",
                label: "Egyesített kincstár",
                permissions: ["Shop.IndexOwnedItems"],
                features: ["Shop"],
            },
        ],
    },
];

const AuthenticatedLayout = (): JSX.Element => {
    const [darwerOpened, { toggle, close }] = useDisclosure(false);

    return (
        <AppShell header={<AuthenticatedHeader links={links} toggleDrawer={toggle} />}>
            <AuthenticatedDrawer links={links} drawerOpened={darwerOpened} closeDrawer={close} />
            <Outlet />
        </AppShell>
    );
};

export default AuthenticatedLayout;
