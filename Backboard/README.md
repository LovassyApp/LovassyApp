# Backboard

A *GUI* app, amit az iskola-vezetőség (*E-Kréta admin* *export*-joggal) használ olykor, hogy feltöltse a diákok jegyeit a [szerver]re.  
Itt állítják be az egész adatbázist titkosító jelszót is, melyre fokozottan kell ügyelniük.

## Első használat

1. telepítő letöltése [GitHub Release]ből (*windows*: `.msi` vagy `.exe`)
2. alkalmazás telepítése, *sajnos a Windows Defender akadékoskodása ellenére is*
3. *[szerver] URL* és *import kulcs* beállítása a beállításokban
4. *visszaállítási jelszó* beállítása (a saját fülén)
5. jegyek és tanulói adatok importálása E-Kréta exportból *csv* formátumban

## A projekt felépítése

### [src-api](./src-api)

Az [`openapi-generator`] által [Rust]hoz generált [API]-*kötés*eket tartalmazó *crate*, ezt használja a [Tauri] app.  
Ezeket [egy GitHub Action] frissíti, ha van rajta mit. Kézzel szerkeszteni **nem** ajánlott.

### [src-tauri](./src-tauri)

Az app lényegi része. Itt történik a feltölteni kívánt adatok beolvasása, feldolgozása, majd innen küldtenek el a [szerver]re.
[Tauri] az alapvető eleme, egyébként [Rust]ban van írva, sok kommenttel.

Regisztrál 3 `invoke_handler`t, amit a [frontend]ről lehet elindítani, ezek végzik majd a munkát.
Tehát importnál először elküldi a [szerver]nek az *import kulcsot* ha szükséges, majd lekéri az ott már beregisztrált tanulók adatait,
hogy aztán az újonnan feltöltött jegyeiket hozzá adja, vissza küldje azokat.
Onnan egy új bejelentkezés után az adatbázisból a felhasználónak be is importálódnak, meg is jellenek az új jegyek.

**Fontos:** Az app 2 helyre ír *log*okat: a futtatás mappájában egy `.lovassyapp-backboard.log` fájlba, továbbá a *konzol*ra, ha ez létezik. Az alapértelmezett `INFO` log szint megváltoztatható a `RUST_LOG` környezeti változó definiálásával.
Ennek értékei lehetnek: `trace, debug, info, warn, error`.
Pl.: `RUST_LOG=debug pnpm tauri dev`, *windows*on: `$env:RUST_LOG="warn"; & '.\LovassyApp - Backboard.exe'`.

**Fontos**: van egy-egy tesztelni való *csv*: [évközi jegyek E-Krétából], továbbá [tanulók adatai E-Krétából].
Ilyen formátumban exportál a Kréta pillanatnyilag, később ha netalántán változna, frissíteni kell az *elemző*ket.

### [src](./src)

[React]ben([TypeScript]) írt *UI*, innen indítható import, itt lehet kezelni kényelmesen a [Tauri] backendet, azt használni.

## Futtatás lokálisan

Szükséges a [Node.js], a [Rust] és a [pnpm] telepítése.  
Amennyiben ez megvan, ajánlott a `Backboard` mappában a `pnpm install` parancs után a `pnpm tauri dev` parancs futtatása.

## Ajánlott fejlesztői környezet

- [VS Code](https://code.visualstudio.com/) + [Tauri](https://marketplace.visualstudio.com/items?itemName=tauri-apps.tauri-vscode) + [rust-analyzer](https://marketplace.visualstudio.com/items?itemName=rust-lang.rust-analyzer)

## Kód dokumentáció

Ha meg szeretnéd tekinteni (ezt) plusz a `Backboard` kód összes dokumentációját, használd a `cargo doc --open` parancsot.

[`openapi-generator`]: https://openapi-generator.tech/
[Rust]: https://rust-lang.org/
[API]: https://bump.sh/xeretis/doc/lovassyapp/
[Tauri]: https://tauri.app/
[React]: https://react.dev/
[szerver]: ../Blueboard/
[frontend]: ./src
[GitHub Release]: https://github.com/LovassyApp/LovassyApp/releases/latest
[egy GitHub Action]: ../.github/workflows/backboard-build.yml
[TypeScript]: https://www.typescriptlang.org/
[évközi jegyek E-Krétából]: ./src-tauri/test_grades.csv
[tanulók adatai E-Krétából]: ./src-tauri/test_students.csv
[pnpm]: https://pnpm.io/
[Node.js]: https://nodejs.org/en/
