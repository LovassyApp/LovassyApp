# LovassyApp

A Lovassy László Gimnázium diákjainak életét megkönnyíteni hivatott alkalmazás.

*Megjegyzés: Ez a repo a jelenlegi legújabb, fejlesztés alatti verziót tartalmazza. Blueboard v4 előtti verziók megtekinthetők itt: https://github.com/LovassyApp/LovassyAppLegacy. Ezt a verziót a 2023/2024-es tanév elején tervezzük bevezetni.*

## Funkciók

- [x] Lovassy Lóvé rendszer
  - [x] Automatikus LoLó generáció
  - [x] LoLó kérvények benyújtása (pl.: versenyeredményért járó LoLó)
  - [x] LoLó elköltése kimentésekre és egyéb termékekre (*jelenleg csak 3.x.x-ben*)
  - [x] Vásárolt termékek felhasználása (*jelenleg csak 3.x.x-ben*)
- [X] Jegyek megtekintése
- [ ] Szavazó rendszer (farsangi jelmezversenyhez, stb.)
- [ ] Párt rendszer (párthéthez)
  - [ ] Pártok létrehozása, regisztálása
  - [ ] Pártprogramok megtekintése
- [ ] Éjszakai röpi rendszer
  - [ ] Röpi csapatok létrehozása, regisztálása
  - [ ] Értesítések meccsek előtt
  - [ ] Pontozás, kivetítendő pontozó tábla
- [ ] Menza rendszer
  - [ ] A vagy B menü igénylése digitálisan
  - [ ] Kajajegy helyett újra felhasználható NFC kártyák/telefonos NFC

*A tervezett funkciók sorrendje megegyezik a prioritási sorrendjükkel.*

## Rövid háttértörténet

A LovassyApp fejlesztését eredetileg [Gyimesi Máté](https://github.com/minigyima) és [Ocskó Nándor](https://github.com/Xeretis) kezdte el a covid okozta karantén alatt (bár az ötlet már hamarabb megvolt). Eredetileg a KRÉTA API-t használva működött a jegyek importálása, de sajnos a fejlesztés során kiderült, hogy a KRÉTA ezt nem engedélyezi. Egy hosszabb szünet után a 2022/2023-as tanév elején sikerült előállnunk egy alternatív megoldással a jegyek importálására, így a projekt folytatódott. A 2022/2023-as tanév vége felé úgy döntöttünk, hogy bővítjük a projekt célkitűzéseit és ezzel egyetemben újra is írjuk a már meglévő rendszert, és ez a Blueboard v4 valamint a Backboard v2 és a Boardlight v2 (azaz ami ebben a repoban van). 

## A fejlesztéshez csatlakozni kívánóknak

*Minden kontribúciónak örülünk, amennyiben nem tudod hogy miben tudsz segíteni, de szeretnél, csak keress meg minket Messengeren (Ocskó Nándor vagy Gyimesi Máté).*

### A LovassyApp részei

- **Blueboard**: A LovassyApp backend-je, [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-7.0) keretrendszerben íródott.
- **Boardlight**: A LovassyApp diákoknak szánt frontend-je, [React](https://react.dev/) keretrendszerben íródott.
- **Backboard**: A LovassyApp iskolavezetőségnek szánt asztali frontend-je, [Tauri](https://tauri.studio/) és [React](https://react.dev/) keretrendszerben íródott.

![LovassyAppDiagram.png](.github/LovassyAppDiagram.png)

### Futtatás lokálisan

#### Blueboard

A Blueboard futtatásához szükséges a [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0). Amennyiben ez megvan ajánlott a `LovassyApp.sln` fájlt egy választott fejlesztői környezetben megnyitni és a solutionben található futási konfigurációt használni.

Alternatív módon a `dotnet restore` parancs futtatása után a `Blueboard` mappában futtatható `dotnet run` parancs, amennyiben a .NET 7 SDK telepítve van.

**Fontos**: Futás előtt globálisan állítsd be a `ASPNETCORE_ENVIRONMENT=development` env változót máskülönben a build lépés egy hibát fog dobni.

#### Boardlight

*A boardlight újraírása még nem kezdődött el, ez a szekció frissítve lesz amikor elkezdődik.*

#### Backboard

A Backboard futtatásához szükséges a [Node.js](https://nodejs.org/en/), a [Rust](https://www.rust-lang.org/) és a [pnpm](https://pnpm.io/) telepítése. Amennyiben ez megvan ajánlott a `backboard` mappában a `pnpm install` parancs után a `pnpm tauri dev` parancs futtatása.
