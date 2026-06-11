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
| `docker/mssql/init/02-create-tables.sql` | Таблицы `ExportMessageRabbitKKA`, `ExportMessageRabbitKKAStatus`, `ExportMessageRabbitNOVA`, `ExportMessageRabbitNOVAStatus`, `ExportMessageRabbitREMARKETING`, `ExportMessageRabbitREMARKETINGStatus`, `ExportMessageRabbitStatusName` |
| `docker/mssql/init/03-seed-status-names.sql` | Справочник статусов: Ready, InProcessed, Send, Close, Error |
| `docker/mssql/init/04-seed-rabbit-integration-operation-keys.sql` | Конфигурация интеграций Rabbit (7 записей) |

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
docker/mssql/init/                    # SQL-скрипты инициализации
VTBL.ExportMessage.Rabbit.Context/    # EF Core, сущности, MscrmExtDbContext
VTBL.ExportMessage.Rabbit.UI/         # ASP.NET Core Razor Pages
```

## Добавление новой системы (по шаблону KKA/NOVA)

Архитектура подготовлена для расширения на следующие интеграции с тем же устройством (KKA, NOVA, Remarketing — по одному шаблону).

1. Добавьте сущности в `VTBL.ExportMessage.Rabbit.Context/Entities`:
   - `ExportMessageRabbit<System>.cs`
   - `ExportMessageRabbit<System>Status.cs`
2. Зарегистрируйте `DbSet` и Fluent API в `MscrmExtDbContext`.
3. Добавьте SQL-таблицы в `docker/mssql/init/02-create-tables.sql` по образцу KKA/NOVA.
4. Создайте UI-модели фильтра/результата:
   - `VTBL.ExportMessage.Rabbit.UI/Models/<System>MessageFilter.cs`
   - `VTBL.ExportMessage.Rabbit.UI/Models/<System>MessagesPageResult.cs`
5. Создайте сервис:
   - `I<System>MessageService` + `<System>MessageService`
   - Реализация наследуется от `IntegrationMessageServiceBase<TMessage, TStatus>`.
6. Добавьте Razor Pages:
   - `Pages/<System>.cshtml`
   - `Pages/<System>.cshtml.cs` (реализует `IIntegrationPagingModel`)
7. Подключите DI в `Startup.cs` и пункт меню в `_Layout.cshtml`.
8. Для пагинации используйте общий partial `Pages/Shared/_IntegrationPagination.cshtml`.

## История изменений

| Дата | Изменение |
|------|-----------|
| 2026-05-26 | Кнопка «Обновить» в блоке результатов (ККА, NOVA, Remarketing): частичная перезагрузка через handler `Results` |
| 2026-05-26 | Обработка отсутствующих таблиц БД: понятные сообщения на страницах интеграций и на главной (`IntegrationDatabaseErrorFormatter`) |
| 2026-05-26 | Фильтр по диапазону `Created` (от/до) на страницах ККА, NOVA, Remarketing; общий `IntegrationFilterBuilder` и partial `_IntegrationCreatedFilter` |
| 2026-05-26 | Добавлен раздел Remarketing (таблицы SQL, EF-сущности, сервисы, Razor Pages `/Remarketing`, навигация, сводка на главной) |
| 2026-05-28 | Добавлен раздел NOVA (таблицы SQL, EF-сущности, сервисы, Razor Pages, навигация) |
| 2026-05-28 | Вынесен общий каркас integration-систем: `IntegrationMessageServiceBase`, общие модели фильтра/пагинации/статистики, общий partial пагинации |
| 2026-05-26 | Seed `RabbitIntegrationOperationKeysConfiguration` (7 операций KKA/1C) |
| 2026-05-26 | Таблица `RabbitIntegrationOperationKeysConfiguration` (SQL + EF) |
| 2026-05-26 | Проект `VTBL.ExportMessage.Rabbit.Context` (EF Core 5, `MscrmExtDbContext`) |
| 2026-05-26 | Web UI: страница Debug — загрузка `ExportMessageRabbitStatusName` из БД |
| 2026-05-26 | MSSQL: именованный том + `E:\volumes\...\backup` (обход LSA на Windows) |
| 2026-05-26 | Seed `ExportMessageRabbitStatusName` (Ready, InProcessed, Send, Close, Error) |
| 2026-05-26 | Docker Compose: MS SQL Server 2022, БД `MSCRM_EXT`, таблицы `ExportMessageRabbitKKA`, `ExportMessageRabbitKKAStatus`, `ExportMessageRabbitStatusName` |
