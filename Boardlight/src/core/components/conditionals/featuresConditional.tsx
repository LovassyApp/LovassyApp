import { ReactNode } from "react";
import { createStyles } from "@mantine/core";
import { useGetApiAuthControl } from "../../../api/generated/features/auth/auth";

const useStyles = createStyles((theme) => ({}));

export const FeaturesConditional = ({
    features,
    fallback,
    children,
    fetchControl = false,
}: {
    features?: string[];
    fallback?: ReactNode;
    children: ReactNode;
    fetchControl?: boolean;
}): JSX.Element => {
    const { classes } = useStyles();

    const control = useGetApiAuthControl({ query: { retry: 0, enabled: fetchControl } });

    if (control.isLoading) return undefined; // Shouldn't happen in theory

    if (!features) return <>{children}</>;

    if (control.isSuccess && control.data.features.some((feature) => features.includes(feature)))
        return <>{children}</>;

    if (fallback) return <>{fallback}</>;

    return undefined;
};
