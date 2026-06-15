using System.Collections;
using System.Collections.Generic;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    /// <summary>
    /// Контракт модели страницы интеграции для общих Razor partial.
    /// </summary>
    public interface IIntegrationPageModel : IIntegrationPagingModel
    {
        /// <summary>
        /// Заголовок страницы интеграционной системы.
        /// </summary>
        string PageTitle { get; }

        /// <summary>
        /// Признак включённого фильтра «есть ошибка в статусах».
        /// </summary>
        bool FilterHasError { get; }

        /// <summary>
        /// Признак включённого фильтра «есть SendMessage в статусах».
        /// </summary>
        bool FilterHasSendMessage { get; }

        /// <summary>
        /// Текущее строковое значение фильтра по идентификатору.
        /// </summary>
        string FilterId { get; }

        /// <summary>
        /// Текущее значение фильтра по ключу операции.
        /// </summary>
        string FilterOperationKey { get; }

        /// <summary>
        /// Текущее значение нижней границы даты создания.
        /// </summary>
        string FilterCreatedFrom { get; }

        /// <summary>
        /// Текущее значение верхней границы даты создания.
        /// </summary>
        string FilterCreatedTo { get; }

        /// <summary>
        /// Ошибка валидации поля идентификатора.
        /// </summary>
        string FilterIdError { get; }

        /// <summary>
        /// Ошибка валидации поля ключа операции.
        /// </summary>
        string FilterOperationKeyError { get; }

        /// <summary>
        /// Ошибка валидации нижней границы даты создания.
        /// </summary>
        string FilterCreatedFromError { get; }

        /// <summary>
        /// Ошибка валидации верхней границы даты создания.
        /// </summary>
        string FilterCreatedToError { get; }

        /// <summary>
        /// Ошибка валидации диапазона дат создания.
        /// </summary>
        string FilterCreatedRangeError { get; }

        /// <summary>
        /// Сообщение об ошибке загрузки всей страницы.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Сообщение об ошибке обновления панели результатов.
        /// </summary>
        string ResultsErrorMessage { get; }

        /// <summary>
        /// Признак того, что пользователь задал хотя бы один фильтр.
        /// </summary>
        bool HasActiveFilter { get; }

        /// <summary>
        /// Общее количество найденных сообщений.
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Номер первой записи на текущей странице.
        /// </summary>
        int RangeFrom { get; }

        /// <summary>
        /// Номер последней записи на текущей странице.
        /// </summary>
        int RangeTo { get; }

        /// <summary>
        /// Доступные ключи операций для выпадающего списка фильтра.
        /// </summary>
        IReadOnlyList<string> OperationKeys { get; }

        /// <summary>
        /// Группы сообщений, отображаемые в панели результатов.
        /// </summary>
        IEnumerable MessageGroups { get; }

        /// <summary>
        /// Возвращает человекочитаемое имя статуса по его идентификатору.
        /// </summary>
        /// <param name="statusId">Идентификатор статуса.</param>
        /// <returns>Имя статуса или строковое представление идентификатора.</returns>
        string ResolveStatusName(int? statusId);
    }
}
