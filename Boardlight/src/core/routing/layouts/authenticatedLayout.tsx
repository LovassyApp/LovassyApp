import {
    Accordion,
    Anchor,
    AppShell,
    Avatar,
    Badge,
    Burger,
    Center,
    Container,
    Divider,
    Drawer,
    Group,
    Header,
    Indicator,
    Loader,
    Menu,
    Modal,
    Paper,
    Stack,
    Text,
    Tooltip,
    UnstyledButton,
    createStyles,
    rem,
    useMantineColorScheme,
} from "@mantine/core";
import {
    IconChevronDown,
    IconInfoCircle,
    IconInfoSquareRounded,
    IconKey,
    IconLogout,
    IconMail,
    IconPackages,
    IconPalette,
    IconUserCircle,
    IconUsersGroup,
} from "@tabler/icons-react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { useDeleteApiAuthLogout, useGetApiAuthControl } from "../../../api/generated/features/auth/auth";

import { FeatureRequirement } from "../../components/requirements/featuresRequirement";
import { PermissionRequirement } from "../../components/requirements/permissionsRequirement";
import { PrivacyPolicyModal } from "../../components/privacyPolicyModal";
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
    avatarSectionContainer: {
        backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[6] : theme.colors.gray[1],
        display: "flex",
        alignItems: "center",
        justifyContent: "space-between",

        [theme.fn.smallerThan("sm")]: {
            flexDirection: "column",
        },
    },
    avatarSectionLabel: {
        [theme.fn.largerThan("sm")]: {
            display: "none",
        },
    },
    avatarSectionIcon: {
        [theme.fn.smallerThan("sm")]: {
            display: "none",
        },
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
    const { classes } = useStyles();
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
            <Center>
                <Indicator
                    color="blue"
                    position="bottom-center"
                    label="Admin"
                    size={20}
                    offset={22}
                    inline={true}
                    disabled={!control.data.isSuperUser}
                    withBorder={true}
                >
                    <IconUserCircle stroke={1.5} size={96} />
                </Indicator>
            </Center>
            <Text weight="bold" align="center" size="xl">
                {control.data.user.realName
                    ? `${control.data.user.realName} (${control.data.user.class})`
                    : control.data.user.name}
            </Text>
            {control.data.user.realName && (
                <Text color="dimmed" align="center">
                    {control.data.user.name}
                </Text>
            )}
            <Paper p="xs" className={classes.avatarSectionContainer} mt="sm">
                <Tooltip label="Email" position="right" withArrow={true}>
                    <IconMail className={classes.avatarSectionIcon} stroke={1.5} size={20} />
                </Tooltip>
                <Group position="center" spacing={rem(2)} className={classes.avatarSectionLabel} mb="md">
                    <IconMail stroke={1.5} size={20} />
                    <Text align="center">Email</Text>
                </Group>
                <Text>{control.data.user.email}</Text>
            </Paper>
            <Divider variant="dashed" my="sm" />
            <Paper p="xs" className={classes.avatarSectionContainer}>
                <Tooltip label="Elérhtő funkciók" position="right" withArrow={true}>
                    <IconPackages className={classes.avatarSectionIcon} stroke={1.5} size={20} />
                </Tooltip>
                <Group position="center" spacing={rem(2)} className={classes.avatarSectionLabel} mb="md">
                    <IconPackages stroke={1.5} size={20} />
                    <Text align="center">Elérhető funkciók</Text>
                </Group>
                <Group position="center" spacing="xs">
                    {control.data.features.map((feature) => (
                        <Badge key={feature} color="violet" variant="filled">
                            {feature}
                        </Badge>
                    ))}
                </Group>
            </Paper>
            <Divider variant="dashed" my="sm" />
            <Paper p="xs" className={classes.avatarSectionContainer}>
                <Tooltip label="Felhasználói csoportok" position="right" withArrow={true}>
                    <IconUsersGroup className={classes.avatarSectionIcon} stroke={1.5} size={20} />
                </Tooltip>
                <Group position="center" spacing={rem(2)} className={classes.avatarSectionLabel} mb="md">
                    <IconUsersGroup stroke={1.5} size={20} />
                    <Text align="center">Felhasználói csoportok</Text>
                </Group>
                <Group position="center" spacing="xs">
                    {control.data.userGroups.map((userGroup) => (
                        <Badge key={userGroup} color="indigo" variant="filled">
                            {userGroup}
                        </Badge>
                    ))}
                </Group>
            </Paper>
            <Paper p="xs" className={classes.avatarSectionContainer} mt="sm">
                <Stack>
                    <Group position="center" spacing={rem(2)}>
                        <IconKey stroke={1.5} size={20} />
                        <Text align="center">Jogosultságok</Text>
                    </Group>
                    <Group spacing="xs" position="center">
                        {control.data.permissions.map((userGroup) => (
                            <Badge key={userGroup} variant="filled">
                                {userGroup}
                            </Badge>
                        ))}
                    </Group>
                </Stack>
            </Paper>
        </Modal>
    );
};

const SystemInformationModal = ({ opened, close }: { opened: boolean; close(): void }) => {
    const version = useGetApiStatusVersion({ SendOk: true, SendMOTD: true });
    const [privacyPolicyModalOpened, { open: openPrivacyPolicyModal, close: closePrivacyPolicyModal }] =
        useDisclosure();

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
        <>
            <PrivacyPolicyModal opened={privacyPolicyModalOpened} close={closePrivacyPolicyModal} />
            <Modal opened={opened} onClose={close} title="Rendszer információk" size="lg">
                <Group position="apart" spacing={0}>
                    <Text>Frontend verzió:</Text>
                    <Text weight="bold">Boardlight {import.meta.env.PACKAGE_VERSION}</Text>
                </Group>
                <Group position="apart" spacing={0}>
                    <Text>Backend verzió:</Text>
                    <Text weight="bold">Blueboard {version.data.version}</Text>
                </Group>
                <Group position="apart" spacing={0}>
                    <Text>.NET verzió:</Text>
                    <Text weight="bold">{version.data.dotNetVersion}</Text>
                </Group>
                <Divider my="md" />
                <Group position="apart" spacing={0}>
                    <Text>Fejlesztők:</Text>
                    <Text weight="bold">Ocskó Nándor (Xeretis), Gyimesi Máté (minigyima)</Text>
                </Group>
                <Group position="apart" spacing={0}>
                    <Text>Forráskód:</Text>
                    <Anchor href={version.data.repository} weight="bold">
                        Github
                    </Anchor>
                </Group>
                <Divider my="md" />
                <Group position="apart" spacing={0}>
                    <Text>Adatvédelmi tájékoztató:</Text>
                    <Anchor
                        weight="bold"
                        component="button"
                        onClick={() => {
                            close();
                            openPrivacyPolicyModal();
                        }}
                    >
                        Megtekintés
                    </Anchor>
                </Group>
                <Divider my="md" />
                <Text>MOTD:</Text>
                <Text weight="bold">{version.data.motd}</Text>
            </Modal>
        </>
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
                        Rendszerinformációk
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
            <FeatureRequirement key={item.label} features={item.features}>
                <PermissionRequirement permissions={item.permissions}>
                    <Menu.Item component={Link} to={item.link}>
                        {item.label}
                    </Menu.Item>
                </PermissionRequirement>
            </FeatureRequirement>
        ));

        if (menuItems) {
            return (
                <FeatureRequirement key={link.label} features={link.features}>
                    <PermissionRequirement permissions={link.permissions}>
                        <Menu
                            trigger="hover"
                            transitionProps={{ transition: "fade", duration: 100 }}
                            withinPortal={true}
                        >
                            <Menu.Target>
                                <Text className={classes.link}>
                                    <Center>
                                        <span className={classes.linkLabel}>{link.label}</span>
                                        <IconChevronDown size={12} stroke={1.5} />
                                    </Center>
                                </Text>
                            </Menu.Target>
                            <Menu.Dropdown>{menuItems}</Menu.Dropdown>
                        </Menu>
                    </PermissionRequirement>
                </FeatureRequirement>
            );
        }

        return (
            <FeatureRequirement key={link.label} features={link.features}>
                <PermissionRequirement permissions={link.permissions}>
                    {/* @ts-ignore */}
                    <Link to={link.link} className={classes.link}>
                        {link.label}
                    </Link>
                </PermissionRequirement>
            </FeatureRequirement>
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
            <FeatureRequirement key={item.label} features={item.features}>
                <PermissionRequirement permissions={item.permissions}>
                    <UnstyledButton onClick={() => doNavigate(item.link)} className={classes.accordionLink}>
                        {item.label}
                    </UnstyledButton>
                </PermissionRequirement>
            </FeatureRequirement>
        ));

        if (accordionItems) {
            return (
                <FeatureRequirement key={link.label} features={link.features}>
                    <PermissionRequirement permissions={link.permissions}>
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
                                            <IconChevronDown size={12} stroke={1.5} />
                                        </Center>
                                    </Text>
                                </Accordion.Control>
                                <Accordion.Panel>
                                    <Stack align="center">{accordionItems}</Stack>
                                </Accordion.Panel>
                            </Accordion.Item>
                        </Accordion>
                    </PermissionRequirement>
                </FeatureRequirement>
            );
        }

        return (
            <FeatureRequirement key={link.label} features={link.features}>
                <PermissionRequirement permissions={link.permissions}>
                    {/* @ts-ignore */}
                    <UnstyledButton onClick={() => doNavigate(link.link)} className={classes.accordionLink}>
                        {link.label}
                    </UnstyledButton>
                </PermissionRequirement>
            </FeatureRequirement>
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

const votingIndexPermissions = ["ImageVotings.IndexImageVotings", "ImageVotings.IndexActiveImageVotings"];

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
        link: "/school/grades",
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
                label: "Kérvények",
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
        label: "Szavazó rendszer",
        permissions: votingIndexPermissions,
        features: ["ImageVotings"],
        links: [
            {
                link: "/image-votings",
                label: "Szavazások",
                permissions: ["ImageVotings.IndexActiveImageVotings", "ImageVotings.IndexImageVotings"],
            },
            {
                link: "/image-votings/manage",
                label: "Szavazások kezelése",
                permissions: ["ImageVotings.IndexImageVotings"],
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
                label: "Összes kérvény",
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
