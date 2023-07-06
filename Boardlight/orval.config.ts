export default {
    qrtagger: {
        output: {
            mode: "tags-split",
            target: "./src/api/generated/features",
            schemas: "./src/api/generated/models",
            client: "react-query",
            override: {
                mutator: {
                    path: "./src/api/customClient.ts",
                    name: "useCustomClient",
                },
            },
        },
        input: {
            target: "../openapi_dev/schema.json",
        },
    },
};
