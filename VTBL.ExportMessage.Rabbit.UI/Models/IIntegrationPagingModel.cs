using System.Collections.Generic;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    /// <summary>
    /// Контракт данных пагинации и route-параметров для partial пагинации.
    /// </summary>
    public interface IIntegrationPagingModel
    {
        /// <summary>
        /// Имя Razor Page текущего раздела.
        /// </summary>
        string PageName { get; }

        /// <summary>
        /// Номер текущей страницы результатов.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Размер страницы результатов.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Общее количество страниц результатов.
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// Признак наличия предыдущей страницы.
        /// </summary>
        bool HasPrevious { get; }

        /// <summary>
        /// Признак наличия следующей страницы.
        /// </summary>
        bool HasNext { get; }

        /// <summary>
        /// Значение фильтра по идентификатору для построения ссылок пагинации.
        /// </summary>
        string FilterIdForRoute { get; }

        /// <summary>
        /// Значение фильтра по ключу операции для построения ссылок пагинации.
        /// </summary>
        string FilterOperationKeyForRoute { get; }

        /// <summary>
        /// Значение фильтра по ошибкам для построения ссылок пагинации.
        /// </summary>
        bool? FilterHasErrorForRoute { get; }

        /// <summary>
        /// Значение фильтра по SendMessage для построения ссылок пагинации.
        /// </summary>
        bool? FilterHasSendMessageForRoute { get; }

        /// <summary>
        /// Значение нижней границы даты создания для построения ссылок пагинации.
        /// </summary>
        string FilterCreatedFromForRoute { get; }

        /// <summary>
        /// Значение верхней границы даты создания для построения ссылок пагинации.
        /// </summary>
        string FilterCreatedToForRoute { get; }

        /// <summary>
        /// Возвращает номера страниц для окна пагинации вокруг текущей.
        /// </summary>
        /// <returns>Последовательность номеров страниц.</returns>
        IEnumerable<int> GetVisiblePageNumbers();
    }
}
