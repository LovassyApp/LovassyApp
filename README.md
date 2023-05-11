# Blueboard

## A kódban használt fogalmak

*Megjegyzés: Minden név, komment, mappa, fájl és miegyéb a kódban angolul van és maradjon is úgy.*

- **Common**: Olyan osztályok összesége, amelyeket az alkalmazás minden része használ (de ha már mondjuk a REST API vagy a SignalR hubok minden részére hatással lehet akkor is ide tartozik) akár direkt vagy indirekt módon.
- **Core**: Olyan funkciós csoportokra osztott osztályok összessége, amelyeket több **feature** is használhat.
- **Feature**: Olyan funkciós csoportokra osztott osztályok összessége, amelyek logikusan összetartoznak és a saját funkciós csoportjukon kívül nincsenek használva. (Speciális esetben "kifelé irányuló kommunikáció a külvilággal" megengedett **event**ek segítségével, ilyen eset mondjuk egy realtime értesítés küldése)
- **Infrastucture**: Olyan osztályok összessége, amelyek külső szolgáltatásokkal való kommunikációért felelősek. (pl.: adatbázis, fájlrendszer, stb.)

<hr>

- **Command, Query**: Lásd: [CQRS](https://www.eventstore.com/cqrs-pattern). (*megjegyzés: itt beszélnek külön write és read modelről is, de mi itt annyira nem bonyolítjuk*)
- **Job**: Bármilyen háttérben futó folyamat akár egyszeri akár viszzatérő. Lásd: [Quartz.NET Jobs And Triggers](https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/jobs-and-triggers.html)
- **Event**: Bármilyen eseményt jelentő osztály. MediatR-en keresztül használjuk őket akkor amikor erre az eventre válaszul még a response visszaküldése előtt történnie kell valaminek (ha elég a response visszaküldése után, akkor egy **job**ot használunk). Lásd: [MediatR Events](https://dev.to/pbouillon/publishing-domain-events-with-mediatr-32mm).
- **Model**: Bármilyen adatot hordozó kizárólag propokkal rendelkező osztály. Ilyen osztály soha semmi logikát nem végez.
- **Service**: Lásd: [Dependency Injection](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-7.0).
- **Controller**: Lásd: [Web Api Controllers](https://www.tutorialsteacher.com/webapi/web-api-controller).
- **Hub**: Lásd: [SignalR Hubs](https://learn.microsoft.com/en-us/aspnet/core/signalr/hubs?view=aspnetcore-7.0).
- **Extension**: Lásd: [Extension Methods](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods).
- **Behaviour**: Lásd: [MediatR Behaviours](https://garywoodfine.com/how-to-use-mediatr-pipeline-behaviours/)
- **Filter**: Lásd: [Filters](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-7.0) (*megjegyzés: sokszor sokkal több értelme van ezeket haszálni mint mondjuk egy middlewaret, middleware igazából csak akkor kell ha az az összes requestre kihat és akkor sem mindig*)
- **ConsoleCommand**: Olyan feladatot tartalmazó osztály, amelyet manuális futtatunk a parancssorból (mint a Laravel Artisan). Fejlesztői környezetben ez így néz ki: `dotnet run <command_name>`.
- **LifetimeAction**: Olyan feladatot tartalmazó osztály, amely az alkalmazás indulásakor vagy leállásakor kell hogy lefusson.
- **Utility**: Olyan osztály, amely mindenképp statikus, nem tartalmaz extension methodot és többször használt vagy "segítő" kódot tartalmaz.
- **Option**: Az alkalmazás `appsettings.json`-ból betöltött konfigurációjának egy részéhez tartozó **model**.
- **Entity**: Adatbázisban tárolt **model**.
- **Seeder**: Olyan osztály, amely az adatbázisba hoz létre előre meghatározott paraméterek alapján **entity**-ket. Általában **ConsoleCommand**-on vagy **LifetimeAction**-on keresztül.
