import { Kbd, Text } from "@mantine/core";

const tipps: JSX.Element[] = [
    <>
        <Text mr={3}>
            Tipp: Használd a{" "}
        </Text>
        <Kbd mr={3}>Ctrl</Kbd>
        <Text mr={3}>
            +
        </Text>
        <Kbd mr={3}>J</Kbd>
        <Text> kombinációt a téma megváltoztatásához.</Text>
    </>,
    <>
        <Text>
            {" "}
            Tipp: A versenyeredményeidért kérvények létrehozásával kaphatsz lolót.
        </Text>
    </>,
];

export const getRandomTipp = (): JSX.Element => {
    const randomIndex = Math.floor(Math.random() * tipps.length);
    return tipps[randomIndex];
};
