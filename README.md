# VTBL.ExportMessage.Rabbit

Веб-приложение для чтения и отображения состояния сообщений RabbitMQ. Тело и метаданные сообщений хранятся в БД **MSCRM_EXT** (MS SQL Server).

## Требования

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (или Docker Engine + Compose v2)
- .NET 5 SDK (для UI-проекта)

## База данных (Docker)

Используется **Microsoft SQL Server 2022** в контейнере. База: `MSCRM_EXT`.

### Быстрый старт

```powershell
# из корня репозитория
Copy-Item .env.example .env
docker compose up -d
```

Первый запуск поднимает SQL Server и однократно выполняет init-скрипты (`mssql-init`):

| Скрипт | Назначение |
|--------|------------|
| `docker/mssql/init/01-create-database.sql` | Создание БД `MSCRM_EXT` |
| `docker/mssql/init/02-create-tables.sql` | Таблицы `ExportMessageRabbitKKA`, `ExportMessageRabbitKKAStatus`, `ExportMessageRabbitStatusName` |
| `docker/mssql/init/03-seed-status-names.sql` | Справочник статусов: Ready, InProcessed, Send, Close, Error |

Проверка:

```powershell
docker compose ps
docker logs vtbl-mssql-init
```

Подключение с хоста:

- **Server:** `localhost,1433`
- **Database:** `MSCRM_EXT`
- **User:** `sa`
- **Password:** значение `MSSQL_SA_PASSWORD` из `.env`

Строка подключения для UI (Development) — в `VTBL.ExportMessage.Rabbit.UI/appsettings.Development.json` (`ConnectionStrings:MSCRM_EXT`). Пароль должен совпадать с `.env`.

### Хранение данных SQL Server

| Путь на хосте | В контейнере | Назначение |
|---------------|--------------|------------|
| Docker-том `vtbl-exportmessage-mssql-data` | `/var/opt/mssql` | Файлы БД (mdf/ldf) |
| `E:\volumes\VTBL.ExportMessage.Rabbit.UI\backup` | `/var/opt/mssql/backup` | Бэкапы `.bak` (видны в проводнике) |

**Почему не весь `/var/opt/mssql` на `E:\`:** SQL Server Linux в Docker Desktop на Windows при bind mount на NTFS падает с `Failed to load LSA: 0xc0070102`. Именованный том — рабочий вариант.

Чтобы **все** данные Docker (включая тома) физически лежали на диске `E:`, в Docker Desktop: **Settings → Resources → Advanced → Disk image location** → `E:\Docker` (или аналог), затем перезапуск Docker.

Просмотр тома с данными БД:

```powershell
docker volume inspect vtbl-exportmessage-mssql-data
```

В Docker Desktop → Settings → Resources → File sharing должен быть доступен диск `E:` (для каталога `backup`).

Если MSSQL падал с LSA — удалите битый каталог и поднимите заново:

```powershell
docker compose down
docker volume rm vtbl-exportmessage-mssql-data
Remove-Item -Path "E:\volumes\VTBL.ExportMessage.Rabbit.UI\*" -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path "E:\volumes\VTBL.ExportMessage.Rabbit.UI\backup" -Force
docker compose up -d
```

### Остановка и данные

```powershell
docker compose down          # контейнеры остановлены, данные на диске E:\volumes\... сохранены
```

Полный сброс БД — `docker compose down`, `docker volume rm vtbl-exportmessage-mssql-data`, затем `docker compose up -d`.

### Повторная инициализация схемы

Скрипты идемпотентны (не пересоздают существующие объекты). Для чистой БД — очистить каталог данных и снова `docker compose up -d` (при необходимости `docker compose run --rm mssql-init`).

## Структура репозитория

```
docker-compose.yml
docker/mssql/init/          # SQL-скрипты инициализации
VTBL.ExportMessage.Rabbit.UI/   # ASP.NET Core Razor Pages
```

## История изменений

| Дата | Изменение |
|------|-----------|
| 2026-05-26 | Web UI: страница Debug — загрузка `ExportMessageRabbitStatusName` из БД |
| 2026-05-26 | MSSQL: именованный том + `E:\volumes\...\backup` (обход LSA на Windows) |
| 2026-05-26 | Seed `ExportMessageRabbitStatusName` (Ready, InProcessed, Send, Close, Error) |
| 2026-05-26 | Docker Compose: MS SQL Server 2022, БД `MSCRM_EXT`, таблицы `ExportMessageRabbitKKA`, `ExportMessageRabbitKKAStatus`, `ExportMessageRabbitStatusName` |
