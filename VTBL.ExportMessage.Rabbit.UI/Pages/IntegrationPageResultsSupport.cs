using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    /// <summary>
    /// Вспомогательная логика загрузки результатов и форматирования ошибок для интеграционных страниц.
    /// </summary>
    internal static class IntegrationPageResultsSupport
    {
        /// <summary>
        /// Загружает страницу данных и нормализует номер страницы, если он вышел за диапазон.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения интеграции.</typeparam>
        /// <param name="messageService">Сервис чтения сообщений.</param>
        /// <param name="pageNumber">Запрошенный номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <param name="filter">Применяемый фильтр.</param>
        /// <param name="state">Объект состояния для заполнения результатами.</param>
        public static async Task LoadResultsAsync<TMessage>(
            IIntegrationMessageService<TMessage> messageService,
            int pageNumber,
            int pageSize,
            IntegrationMessageFilter filter,
            IntegrationResultsState state)
            where TMessage : class
        {
            pageNumber = Math.Max(1, pageNumber);

            var result = await messageService
                .GetMessagesPageAsync(pageNumber, pageSize, filter)
                .ConfigureAwait(false);

            if (result.TotalPages > 0 && pageNumber > result.TotalPages)
            {
                pageNumber = result.TotalPages;
                result = await messageService
                    .GetMessagesPageAsync(pageNumber, pageSize, filter)
                    .ConfigureAwait(false);
            }

            state.MessageGroups = result.Items;
            state.TotalCount = result.TotalCount;
            state.PageNumber = result.Page;
            state.TotalPages = result.TotalPages;
            state.HasPrevious = result.HasPrevious;
            state.HasNext = result.HasNext;
            state.RangeFrom = result.RangeFrom;
            state.RangeTo = result.RangeTo;

            state.StatusNames = await messageService.GetStatusNameMapAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Логирует исключение и возвращает пользовательское сообщение об ошибке.
        /// </summary>
        /// <param name="exception">Исходное исключение.</param>
        /// <param name="system">Описание интеграционной системы.</param>
        /// <param name="logger">Логгер страницы.</param>
        /// <param name="logMessage">Техническое сообщение для лога.</param>
        /// <returns>Пользовательское сообщение об ошибке.</returns>
        public static string FormatLoadError(
            Exception exception,
            IntegrationSystemDescriptor system,
            ILogger logger,
            string logMessage)
        {
            logger.LogError(exception, logMessage);
            return IntegrationDatabaseErrorFormatter.ToUserMessage(exception, system);
        }
    }

    /// <summary>
    /// Промежуточное состояние результатов страницы интеграции.
    /// </summary>
    internal class IntegrationResultsState
    {
        public object MessageGroups { get; set; }

        public int TotalCount { get; set; }

        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }

        public bool HasPrevious { get; set; }

        public bool HasNext { get; set; }

        public int RangeFrom { get; set; }

        public int RangeTo { get; set; }

        public System.Collections.Generic.IReadOnlyDictionary<int, string> StatusNames { get; set; }
            = new System.Collections.Generic.Dictionary<int, string>();
    }
}
