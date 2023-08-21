import { Box, DefaultProps, Selectors, createStyles, rem } from "@mantine/core";
import { Html5QrcodeResult, Html5QrcodeScanType, Html5QrcodeSupportedFormats } from "html5-qrcode";
import { Html5QrcodeScanner, Html5QrcodeScannerConfig } from "html5-qrcode/esm/html5-qrcode-scanner";

import { useEffect } from "react";

const useStyles = createStyles((theme) => ({
    root: {
        ["#html5qr-code-full-region"]: {
            borderRadius: theme.radius.sm,
            borderColor: `${theme.colorScheme === "dark" ? theme.colors.dark[4] : theme.colors.gray[2]} !important`,
            ['img[alt="Info icon"]']: {
                display: "none",
            },
            ['img[alt="Camera based scan"]']: {
                display: "none",
            },
        },
        ["#html5-qrcode-button-camera-permission"]: {
            textIndent: "-9999px",
            lineHeight: 0,

            borderRadius: theme.radius.sm,
            backgroundColor: theme.fn.primaryColor(),
            border: "none",
            fontWeight: 600,
            cursor: "pointer",
            paddingLeft: rem(18),
            paddingRight: rem(18),
            height: rem(36),
            ["&:active"]: theme.activeStyles,
            ["&:hover"]: {
                backgroundColor: theme.colorScheme === "dark" ? theme.colors.blue[9] : theme.colors.blue[7],
            },
        },
        ["#html5-qrcode-button-camera-permission:after"]: {
            content: '"Kamera elérésének engedélyezése"',
            display: "block",
            textIndent: 0,
            lineHeight: 1,
            fontSize: theme.fontSizes.sm,
            marginBottom: -2,
            color: theme.white,
        },
        ["#html5-qrcode-button-camera-stop"]: {
            textIndent: "-9999px",
            lineHeight: 0,

            borderRadius: theme.radius.sm,
            backgroundColor: theme.fn.primaryColor(),
            border: "none",
            fontWeight: 600,
            cursor: "pointer",
            paddingLeft: rem(18),
            paddingRight: rem(18),
            height: rem(36),
            ["&:active"]: theme.activeStyles,
            ["&:hover"]: {
                backgroundColor: theme.colorScheme === "dark" ? theme.colors.blue[9] : theme.colors.blue[7],
            },
        },
        ["#html5-qrcode-button-camera-stop:after"]: {
            content: '"Kamera leállítása"',
            display: "block",
            textIndent: 0,
            lineHeight: 1,
            fontSize: theme.fontSizes.sm,
            marginBottom: -2,
            color: theme.white,
        },
        ["#html5-qrcode-button-camera-start"]: {
            textIndent: "-9999px",
            lineHeight: 0,

            borderRadius: theme.radius.sm,
            backgroundColor: theme.fn.primaryColor(),
            border: "none",
            fontWeight: 600,
            cursor: "pointer",
            paddingLeft: rem(18),
            paddingRight: rem(18),
            height: rem(36),
            ["&:active"]: theme.activeStyles,
            ["&:hover"]: {
                backgroundColor: theme.colorScheme === "dark" ? theme.colors.blue[9] : theme.colors.blue[7],
            },
        },
        ["#html5-qrcode-button-camera-start:after"]: {
            content: '"Kamera elindítása"',
            display: "block",
            textIndent: 0,
            lineHeight: 1,
            fontSize: theme.fontSizes.sm,
            marginBottom: -2,
            color: theme.white,
        },
        ["#html5qr-code-full-region__scan_region"]: {
            borderTopRightRadius: theme.radius.md,
            borderTopLeftRadius: theme.radius.md,
            overflow: "hidden",
        },
        ["#html5qr-code-full-region__header_message"]: {
            display: "none !important",
        },
    },
    reader: {},
}));

type QrCodeReaderStylesNames = Selectors<typeof useStyles>;

interface QrCodeReaderProps extends DefaultProps<QrCodeReaderStylesNames> {
    successCallback?(decodedText: string, result: Html5QrcodeResult): void;
}

const createConfig = (props: any) => {
    const config: Html5QrcodeScannerConfig = {
        supportedScanTypes: [Html5QrcodeScanType.SCAN_TYPE_CAMERA],
        formatsToSupport: [Html5QrcodeSupportedFormats.QR_CODE],
        fps: 10,
    };
    if (props.fps) {
        config.fps = props.fps;
    }
    if (props.qrbox) {
        config.qrbox = props.qrbox;
    }
    if (props.aspectRatio) {
        config.aspectRatio = props.aspectRatio;
    }
    if (props.disableFlip !== undefined) {
        config.disableFlip = props.disableFlip;
    }
    return config;
};

export const QrCodeReader = ({
    classNames,
    styles,
    unstyled,
    className,
    successCallback,
    ...others
}: QrCodeReaderProps & Partial<Html5QrcodeScannerConfig>) => {
    const { classes, cx } = useStyles(undefined, { name: "QrCodeReader", classNames, styles, unstyled });

    const qrcodeRegionId = "html5qr-code-full-region";

    useEffect(() => {
        const config = createConfig(others);
        const html5QrcodeScanner = new Html5QrcodeScanner(qrcodeRegionId, config, false);
        html5QrcodeScanner.render(successCallback, undefined);

        return () => {
            try {
                html5QrcodeScanner.clear();
            } catch (e) {
                console.error(e);
            }
        };
    }, [others, successCallback]);

    return (
        <Box className={cx(classes.root, className)} {...others}>
            <div className={classes.reader} id={qrcodeRegionId} />
        </Box>
    );
};
