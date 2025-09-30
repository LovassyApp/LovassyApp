# Blueboard – Docker Setup

[Magyar readme](./README-blueboard.hu.md)

services:

- **blueboard** – runs the backend, see '--help' for details
- **db** – PostgreSQL database with persisted data

---

## Prerequisites

- [Docker](https://docs.docker.com/get-docker/)  
- [Docker Compose](https://docs.docker.com/compose/install/)  

---

## Environment Variables

All sensitive values should be provided via a `.env` file in the root of the project. ([see this example](../.env.example))
> ⚠️ Without a `.env` file one **can not** run the app.

> ⚠️ **Do not commit** `.env` to version control.

---

## Usage

### Start Blueboard

```bash
docker compose run --rm blueboard migrate
docker compose up (-d)
```

### Run commands to manage Blueboard

```bash
docker compose run --rm blueboard --help
```
