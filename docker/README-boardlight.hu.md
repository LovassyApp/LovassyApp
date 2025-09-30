# Boardlight – Docker beállítás

[English readme](./README-boardlight.en.md)

Szolgáltatás:

- **boardlight** – futtatja a frontend-et automatikus újratöltéssel (hot reload)

---

## Előfeltételek

- [Docker](https://docs.docker.com/get-docker/)  
- [Docker Compose](https://docs.docker.com/compose/install/)  

---

## Környezeti változók

Néhány fontos (nem publikus), a Boardlight által használt értéket egy `.env` fájlban kell megadni, a [Boardlight mappában](../Boardlight). ([ez egy jo kiindulási pont](../Boardlight/.env.example))
> ⚠️ A `.env` fájl nélkül **nem tudod** futattni az appot.

> ⚠️ A `.env` fájlt **soha ne commitold**.

---

## Használat

### 1. Az alkalmazás futtatása

```bash
docker compose up boardlight
```

Ez felépíti az image-et, felcsatolja a forráskódot, és elindítja a Boardlight-ot a <http://localhost> címen.
A kódban végzett módosítások automatikusan meg fognak jelenni.
