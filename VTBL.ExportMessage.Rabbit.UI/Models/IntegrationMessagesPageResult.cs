using System;
using System.Collections.Generic;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    /// <summary>
    /// Результат постраничной выборки сообщений с вычисляемыми метаданными пагинации.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения интеграции.</typeparam>
    public class IntegrationMessagesPageResult<TMessage>
    {
        /// <summary>
        /// Элементы текущей страницы.
        /// </summary>
        public IReadOnlyList<TMessage> Items { get; set; } = Array.Empty<TMessage>();

        /// <summary>
        /// Общее количество записей по фильтру.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Номер текущей страницы (начиная с 1).
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Размер страницы.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Общее количество страниц.
        /// </summary>
        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize)
            : 0;

        /// <summary>
        /// Признак наличия предыдущей страницы.
        /// </summary>
        public bool HasPrevious => Page > 1;

        /// <summary>
        /// Признак наличия следующей страницы.
        /// </summary>
        public bool HasNext => Page < TotalPages;

        /// <summary>
        /// Номер первой записи на текущей странице в общем списке.
        /// </summary>
        public int RangeFrom => TotalCount == 0 ? 0 : ((Page - 1) * PageSize) + 1;

        /// <summary>
        /// Номер последней записи на текущей странице в общем списке.
        /// </summary>
        public int RangeTo => TotalCount == 0 ? 0 : Math.Min(Page * PageSize, TotalCount);
    }
}
