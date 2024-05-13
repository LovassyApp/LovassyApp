import { FileInputProps } from "@mantine/core";

declare module "@mantine/core" {
    export interface FileInputProps {
        placeholder?: string;
    }
}
