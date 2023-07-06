import { Box, Text, createStyles } from "@mantine/core";

import { authApiClient } from "../../../api/apiClients";
import { useEffect } from "react";

const useStyles = createStyles((theme) => ({}));

const LoginPage = (): JSX.Element => {
    const { classes } = useStyles();

    useEffect(() => {
        (async() => {
            const res = await authApiClient.apiAuthControlGet();
            console.log(res);
        })();
    }, []);

    return <Text>asd</Text>;
};

export default LoginPage;
