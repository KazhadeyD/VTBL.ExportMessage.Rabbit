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

## Добавление новой интеграционной системы

Эталон последней добавленной системы — **Remarketing**. Копируйте её файлы и заменяйте имя системы (`Remarketing` → `MySystem`). Архитектура общая: фильтры, пагинация, partial'ы, базовый сервис и PageModel уже реализованы — для новой системы создаётся только тонкий слой-обёртка.

### Что менять не нужно

Следующие компоненты **общие для всех систем** — правки не требуются:

| Компонент | Назначение |
|-----------|------------|
| `Pages/Shared/_IntegrationPageLayout.cshtml` | Двухколоночный layout (фильтры + результаты) |
| `Pages/Shared/_IntegrationFiltersForm.cshtml` | GET-форма фильтров |
| `Pages/Shared/_IntegrationCreatedFilter.cshtml` | Поля Created от/до |
| `Pages/Shared/_IntegrationResultsPanel.cshtml` | Список сообщений, статусы, пагинация |
| `Pages/Shared/_IntegrationPagination.cshtml` | Навигация по страницам |
| `Pages/Shared/_IntegrationPageSizeSelector.cshtml` | Выбор 20 / 50 / 100 |
| `Pages/Shared/_IntegrationDataErrorAlert.cshtml` | Ошибка загрузки страницы |
| `Pages/IntegrationPageModelBase.cs` | Общая логика OnGet / handler `Results` |
| `Services/IntegrationMessageServiceBase.cs` | Запросы, фильтрация, статистика дашборда |
| `Models/IntegrationMessageFilter.cs` | Базовый фильтр (Id, OperationKey, Created, флаги) |
| `wwwroot/js/integration-filters.js` | Маска GUID в поле Id |
| `wwwroot/js/integration-results-refresh.js` | AJAX-обновление панели результатов |

### Чеклист по слоям

Подставьте вместо `<System>` имя в PascalCase (например `Remarketing`), вместо `<SYSTEM>` — имя таблицы в SQL (например `REMARKETING`).

#### 1. EF Core (Context)

| Шаг | Файл | Эталон |
|-----|------|--------|
| Сущность сообщения | `Context/Entities/ExportMessageRabbit<System>.cs` | `ExportMessageRabbitRemarketing.cs` |
| Сущность статуса | `Context/Entities/ExportMessageRabbit<System>Status.cs` | `ExportMessageRabbitRemarketingStatus.cs` |
| DbSet + Fluent API | `Context/MscrmExtDbContext.cs` | блок `ExportMessageRabbitRemarketing` (~строки 41–48, 154–198) |
| Навигация OperationKey | `Context/Entities/RabbitIntegrationOperationKeysConfiguration.cs` | свойство `RemarketingExportMessages` + `WithMany` в Fluent API |

На сущностях реализуйте:

- `IExportMessageRabbitMessage<TStatus>` — на классе сообщения
- `IExportMessageRabbitStatus` — на классе статуса

Связи **логические** (FK в БД нет): `OperationKey` → `RabbitIntegrationOperationKeysConfiguration.Key`, `IntegrationId` → `Id` сообщения.

#### 2. UI — модели

Тонкие наследники базовых типов (тело класса пустое):

| Файл | Базовый класс | Эталон |
|------|---------------|--------|
| `UI/Models/<System>MessageFilter.cs` | `IntegrationMessageFilter` | `RemarketingMessageFilter.cs` |
| `UI/Models/<System>MessagesPageResult.cs` | `IntegrationMessagesPageResult<TMessage>` | `RemarketingMessagesPageResult.cs` |

Добавьте дескриптор в `UI/Models/IntegrationSystemInfo.cs`:

```csharp
public static readonly IntegrationSystemDescriptor MySystem = new IntegrationSystemDescriptor(
    "Отображаемое имя",           // подпись в navbar и на дашборде
    "ExportMessageRabbitMYSYSTEM",  // таблица сообщений
    "ExportMessageRabbitMYSYSTEMStatus");
```

#### 3. UI — сервис

| Файл | Эталон |
|------|--------|
| `UI/Services/I<System>MessageService.cs` | `IRemarketingMessageService.cs` — наследует `IIntegrationMessageService<TMessage>` |
| `UI/Services/<System>MessageService.cs` | `RemarketingMessageService.cs` |

Сервис наследует `IntegrationMessageServiceBase<TMessage, TStatus>` и передаёт фабрику запроса с обязательными `Include`:

```csharp
: base(dbContext, ctx => ctx.ExportMessageRabbitMySystems
    .AsNoTracking()
    .Include(m => m.StatusHistory.OrderBy(s => s.RowVersion))
    .Include(m => m.OperationConfiguration))
```

Сортировка статусов по `RowVersion`, не по `Created`.

#### 4. UI — Razor Pages

| Файл | Эталон | Содержимое |
|------|--------|------------|
| `Pages/<System>.cshtml.cs` | `Remarketing.cshtml.cs` | Наследник `IntegrationPageModelBase<TMessage, TFilter>`; три override: `MessageService`, `SystemInfo`, `PageName` |
| `Pages/<System>.cshtml` | `Remarketing.cshtml` | `ViewData["MainContainerClass"] = "container-fluid"`, partial `_IntegrationPageLayout`, секция `Scripts` с `integration-filters.js` и `integration-results-refresh.js` |

`PageName` должен совпадать с именем Razor Page **без** слэша (например `"Remarketing"` → маршрут `/Remarketing`).

#### 5. Регистрация в приложении

| Место | Действие | Эталон |
|-------|----------|--------|
| `UI/Startup.cs` | `services.AddScoped<I<System>MessageService, <System>MessageService>()` | строка с `IRemarketingMessageService` |
| `Pages/Shared/_IntegrationNavItems.cshtml` | `<li>` с `asp-page="/<System>"` и `@IntegrationSystemInfo.<System>.DisplayName` | пункт Remarketing |
| `Pages/Index.cshtml.cs` | Внедрить `I<System>MessageService`, добавить `LoadEntryAsync(...)` в `OnGetAsync` | блок `entries[2]` для Remarketing |

На главной статистика загружается **последовательно** (один scoped `DbContext` на запрос — `Task.WhenAll` недопустим). При добавлении системы увеличьте размер массива `entries` (сейчас `[3]`).

#### 6. Проверка

```powershell
dotnet build VTBL.ExportMessage.Rabbit.UI/VTBL.ExportMessage.Rabbit.UI.csproj -o _build_out
```

Далее вручную:

1. `/MySystem` — страница открывается, фильтры и пагинация работают.
2. Главная `/` — строка новой системы в таблице дашборда (`_IntegrationDashboardTable`).
3. Navbar — пункт с `DisplayName` из `IntegrationSystemInfo`.
4. Клик по числу ошибок на дашборде ведёт на `/MySystem?hasError=true`.
5. Кнопка «Обновить» в результатах перезагружает панель без полной перезагрузки страницы.

### Сводка новых файлов (минимум)

```
VTBL.ExportMessage.Rabbit.Context/Entities/
  ExportMessageRabbit<System>.cs
  ExportMessageRabbit<System>Status.cs
VTBL.ExportMessage.Rabbit.Context/MscrmExtDbContext.cs            # DbSet + Fluent API
VTBL.ExportMessage.Rabbit.Context/Entities/
  RabbitIntegrationOperationKeysConfiguration.cs                  # ICollection навиг.
VTBL.ExportMessage.Rabbit.UI/Models/
  <System>MessageFilter.cs
  <System>MessagesPageResult.cs
  IntegrationSystemInfo.cs                                        # новый дескриптор
VTBL.ExportMessage.Rabbit.UI/Services/
  I<System>MessageService.cs
  <System>MessageService.cs
VTBL.ExportMessage.Rabbit.UI/Pages/
  <System>.cshtml
  <System>.cshtml.cs
VTBL.ExportMessage.Rabbit.UI/Startup.cs                           # DI
VTBL.ExportMessage.Rabbit.UI/Pages/Shared/_IntegrationNavItems.cshtml
VTBL.ExportMessage.Rabbit.UI/Pages/Index.cshtml.cs              # дашборд
```

## История изменений

| Дата | Изменение |
|------|-----------|
| 2026-06-15 | site.css: секционные комментарии по блокам (layout, navbar, footer, integration-*) |
| 2026-06-15 | README: чеклист новой системы — убран подраздел про SQL-скрипты БД |
| 2026-06-15 | README: подробный чеклист добавления новой интеграционной системы (эталон Remarketing, актуальные partial'ы и регистрация) |
| 2026-06-15 | Razor Pages (*.cshtml): заголовочные комментарии @* ... *@ с назначением, моделью и зависимостями partial |
| 2026-06-15 | Полное XML-покрытие публичных типов и членов в Context/UI (классы, интерфейсы, свойства, методы, параметры) |
| 2026-06-15 | Добавлена XML-документация в ключевых слоях Context/UI: сервисы, модели, PageModel, интерфейсы и инфраструктурные классы |
| 2026-06-11 | Shared: CSS-классы `integration-*` вместо `kka-*`; навигация в `_IntegrationNavItems` (подписи из `IntegrationSystemInfo`) |
| 2026-06-11 | Исправлена сводка на главной: последовательная загрузка статистики (общий `DbContext`), COUNT без лишних `Include` |
| 2026-06-11 | Главная: компактная таблица сводки (`_IntegrationDashboardTable`), ссылки на разделы и фильтр ошибок; onboarding в collapse |
| 2026-06-11 | Общие partial'ы `_IntegrationPageLayout` и `_IntegrationFiltersForm`; единый `IntegrationMessageServiceBase` с `Include(OrderBy RowVersion)`; интерфейсы `IExportMessageRabbitMessage` / `IExportMessageRabbitStatus` |
| 2026-06-11 | Рефакторинг страниц интеграций: `IntegrationPageModelBase<TMessage, TFilter>`, единый `IIntegrationPageModel` |
| 2026-06-11 | `.gitignore`: каталог `_build_out/` (альтернативный вывод `dotnet build`) |
| 2026-06-11 | Выбор размера страницы результатов: 20 / 50 / 100 (`pageSize` в query string) |
| 2026-06-11 | Сортировка статусов в результатах по `RowVersion` (SQL rowversion), а не по `Created` |
| 2026-06-11 | Кнопка «Обновить» в блоке результатов (ККА, NOVA, Remarketing): частичная перезагрузка через handler `Results` |
| 2026-06-11 | Обработка отсутствующих таблиц БД: понятные сообщения на страницах интеграций и на главной (`IntegrationDatabaseErrorFormatter`) |
| 2026-06-11 | Фильтр по диапазону `Created` (от/до) на страницах ККА, NOVA, Remarketing; общий `IntegrationFilterBuilder` и partial `_IntegrationCreatedFilter` |
| 2026-06-11 | Добавлен раздел Remarketing (таблицы SQL, EF-сущности, сервисы, Razor Pages `/Remarketing`, навигация, сводка на главной) |
| 2026-05-28 | Добавлен раздел NOVA (таблицы SQL, EF-сущности, сервисы, Razor Pages, навигация) |
| 2026-05-28 | Вынесен общий каркас integration-систем: `IntegrationMessageServiceBase`, общие модели фильтра/пагинации/статистики, общий partial пагинации |
| 2026-05-26 | Seed `RabbitIntegrationOperationKeysConfiguration` (7 операций KKA/1C) |
| 2026-05-26 | Таблица `RabbitIntegrationOperationKeysConfiguration` (SQL + EF) |
| 2026-05-26 | Проект `VTBL.ExportMessage.Rabbit.Context` (EF Core 5, `MscrmExtDbContext`) |
| 2026-05-26 | Web UI: страница Debug — загрузка `ExportMessageRabbitStatusName` из БД |
| 2026-05-26 | MSSQL: именованный том + `E:\volumes\...\backup` (обход LSA на Windows) |
| 2026-05-26 | Seed `ExportMessageRabbitStatusName` (Ready, InProcessed, Send, Close, Error) |
| 2026-05-26 | Docker Compose: MS SQL Server 2022, БД `MSCRM_EXT`, таблицы `ExportMessageRabbitKKA`, `ExportMessageRabbitKKAStatus`, `ExportMessageRabbitStatusName` |
