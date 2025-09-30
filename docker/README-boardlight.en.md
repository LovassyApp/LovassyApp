# Boardlight – Docker Setup

[Magyar readme](./README-blueboard.hu.md)

Service:

- **boardlight** – runs the frontend with hot reload

---

## Prerequisites

- [Docker](https://docs.docker.com/get-docker/)  
- [Docker Compose](https://docs.docker.com/compose/install/)  

---

## Environment Variables

Some values used by Boardlight should be provided via a `.env` file in the [Boardlight folder](../Boardlight). ([see this example](../Boardlight/.env.example))
> ⚠️ Without a `.env` file one **can not** run the app.

> ⚠️ **Do not commit** `.env` to version control.

---

## Usage

### 1. Run the app

```bash
docker compose up boardlight
```
 
This will build the image, mount the source code, and start Boardlight on <http://localhost>.
Changes to your code should reflect automatically.
