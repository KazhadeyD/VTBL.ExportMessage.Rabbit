using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VTBL.RabbitIntegration.Monitor.UI.Models;

namespace VTBL.RabbitIntegration.Monitor.UI.Services
{
    /// <summary>
    /// Контракт сервиса чтения интеграционных сообщений, справочников и агрегатов для UI.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения интеграции.</typeparam>
    public interface IIntegrationMessageService<TMessage>
    {
        /// <summary>
        /// Возвращает страницу сообщений с учетом фильтра.
        /// </summary>
        /// <param name="page">Номер страницы (начиная с 1).</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <param name="filter">Фильтр сообщений или <see langword="null"/>.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Страница сообщений с метаданными пагинации.</returns>
        Task<IntegrationMessagesPageResult<TMessage>> GetMessagesPageAsync(
            int page,
            int pageSize,
            IntegrationMessageFilter filter = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает словарь идентификатор статуса - наименование.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Словарь идентификатор статуса — наименование.</returns>
        Task<IReadOnlyDictionary<int, string>> GetStatusNameMapAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает список доступных ключей операций.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список ключей операций.</returns>
        Task<IReadOnlyList<string>> GetOperationKeysAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает статистику для дашборда главной страницы.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Агрегированная статистика по системе.</returns>
        Task<IntegrationDashboardStats> GetDashboardStatsAsync(
            CancellationToken cancellationToken = default);
    }
}
