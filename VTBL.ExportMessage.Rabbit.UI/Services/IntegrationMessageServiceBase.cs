using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    /// <summary>
    /// Базовая реализация сервиса интеграционных сообщений: фильтрация, пагинация и агрегаты дашборда.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения интеграции.</typeparam>
    /// <typeparam name="TStatus">Тип статуса сообщения.</typeparam>
    public class IntegrationMessageServiceBase<TMessage, TStatus> : IIntegrationMessageService<TMessage>
        where TMessage : class, IExportMessageRabbitMessage<TStatus>
        where TStatus : class, IExportMessageRabbitStatus
    {
        private readonly Func<MscrmExtDbContext, IQueryable<TMessage>> _baseQueryFactory;

        /// <summary>
        /// EF Core контекст базы данных.
        /// </summary>
        protected readonly MscrmExtDbContext DbContext;

        /// <summary>
        /// Инициализирует базовый сервис интеграционных сообщений.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных.</param>
        /// <param name="baseQueryFactory">Фабрика базового запроса с нужными <c>Include</c>.</param>
        protected IntegrationMessageServiceBase(
            MscrmExtDbContext dbContext,
            Func<MscrmExtDbContext, IQueryable<TMessage>> baseQueryFactory)
        {
            DbContext = dbContext;
            _baseQueryFactory = baseQueryFactory;
        }

        /// <summary>
        /// Загружает страницу сообщений с учетом фильтров.
        /// </summary>
        /// <param name="page">Номер страницы (начиная с 1).</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <param name="filter">Фильтр сообщений или <see langword="null"/>.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Страница сообщений с метаданными пагинации.</returns>
        public async Task<IntegrationMessagesPageResult<TMessage>> GetMessagesPageAsync(
            int page,
            int pageSize,
            IntegrationMessageFilter filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery();

            if (filter != null)
            {
                if (filter.HasId)
                {
                    query = query.Where(m => m.Id == filter.Id.Value);
                }

                if (filter.HasOperationKey)
                {
                    query = query.Where(m => m.OperationKey == filter.OperationKey);
                }

                if (filter.WithError)
                {
                    query = query.Where(m => m.StatusHistory.Any(s =>
                        s.ErrorMessage != null && s.ErrorMessage != string.Empty));
                }

                if (filter.WithSendMessage)
                {
                    query = query.Where(m => m.StatusHistory.Any(s =>
                        s.SendMessage != null && s.SendMessage != string.Empty));
                }

                if (filter.HasCreatedFrom)
                {
                    query = query.Where(m => m.Created >= filter.CreatedFrom.Value);
                }

                if (filter.HasCreatedTo)
                {
                    query = query.Where(m => m.Created <= filter.CreatedTo.Value);
                }
            }

            var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

            var messages = await query
                .OrderByDescending(m => m.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return new IntegrationMessagesPageResult<TMessage>
            {
                Items = messages,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
            };
        }

        /// <summary>
        /// Возвращает словарь наименований статусов по их числовому идентификатору.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Словарь идентификатор статуса — наименование.</returns>
        public async Task<IReadOnlyDictionary<int, string>> GetStatusNameMapAsync(
            CancellationToken cancellationToken = default)
        {
            var statusNames = await DbContext.ExportMessageRabbitStatusNames
                .AsNoTracking()
                .Where(s => s.Id != null)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return statusNames.ToDictionary(s => s.Id.Value, s => s.StatusName);
        }

        /// <summary>
        /// Возвращает доступные <c>OperationKey</c> в алфавитном порядке.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список ключей операций.</returns>
        public async Task<IReadOnlyList<string>> GetOperationKeysAsync(
            CancellationToken cancellationToken = default)
        {
            return await DbContext.RabbitIntegrationOperationKeysConfigurations
                .AsNoTracking()
                .OrderBy(c => c.Key)
                .Select(c => c.Key)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Вычисляет агрегаты для главной страницы: всего, с ошибками и без ошибок.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Агрегированная статистика по системе.</returns>
        public async Task<IntegrationDashboardStats> GetDashboardStatsAsync(
            CancellationToken cancellationToken = default)
        {
            // Без Include: для COUNT достаточно навигации в Where; Include ломает перевод запроса.
            var baseQuery = DbContext.Set<TMessage>().AsNoTracking();

            var total = await baseQuery
                .CountAsync(cancellationToken)
                .ConfigureAwait(false);

            var failed = await baseQuery
                .Where(m => m.StatusHistory.Any(s =>
                    s.ErrorMessage != null && s.ErrorMessage != string.Empty))
                .CountAsync(cancellationToken)
                .ConfigureAwait(false);

            return new IntegrationDashboardStats
            {
                Total = total,
                Failed = failed,
                Successful = total - failed,
            };
        }

        /// <summary>
        /// Строит базовый запрос для сущности интеграции.
        /// </summary>
        /// <returns>Базовый <see cref="IQueryable{T}"/> с подключёнными навигациями.</returns>
        protected IQueryable<TMessage> BuildBaseQuery() => _baseQueryFactory(DbContext);
    }
}
