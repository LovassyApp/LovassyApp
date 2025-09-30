# Blueboard – Docker beállítás

[English readme](./README-blueboard.en.md)

Szolgáltatások:

- **blueboard** – futtatja a backend-et, lesd meg a '--help' további részletekért
- **db** – PostgreSQL adatbázis perzisztens adattal

---

## Előfeltételek

- [Docker](https://docs.docker.com/get-docker/)  
- [Docker Compose](https://docs.docker.com/compose/install/)  

---

## Környezeti változók

Minden fontos (nem publikus) értéket egy `.env` fájlba írj a project gyökérkönyvtárába. ([ez egy jo kiindulási pont](../.env.example))
> ⚠️ A `.env` fájl nélkül **nem tudod** futattni az appot.

> ⚠️ A `.env` fájlt **soha ne commitold**.

---

## Használat

### Indítsd el a Blueboard-ot

```bash
docker compose run --rm blueboard migrate
docker compose up (-d)
````

### Futtass parancsokat a Blueboard kezeléséhez

```bash
docker compose run --rm blueboard --help
```
