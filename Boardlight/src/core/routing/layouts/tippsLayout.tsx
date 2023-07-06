import { Box, createStyles } from "@mantine/core";
import { useEffect, useState } from "react";

import { Outlet } from "react-router-dom";
import { getRandomTipp } from "../../../helpers/tippsHelpers";

const useStyles = createStyles((theme) => ({
    content: {
        width: "100%",
        height: "95vh",

        [`@media (max-width: ${theme.breakpoints.sm}px)`]: {
            height: "100vh",
        },
    },
    tipps: {
        display: "flex",
        justifyContent: "center",
        flexDirection: "row",

        [theme.fn.smallerThan("sm")]: {
            display: "none",
        },
    },
}));

export const TippsLayout = (): JSX.Element => {
    const { classes } = useStyles();

    const [tipp, setTipp] = useState<JSX.Element>();

    useEffect(() => {
        setTipp(getRandomTipp());
    }, []);

    return (
        <Box className={classes.content}>
            <Outlet />
            <Box className={classes.tipps}>{tipp}</Box>
        </Box>
    );
};
