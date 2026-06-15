using System;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    /// <summary>
    /// Фильтр поиска интеграционных сообщений в UI.
    /// </summary>
    public class IntegrationMessageFilter
    {
        /// <summary>
        /// Идентификатор сообщения для точного поиска.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Ключ операции для фильтрации.
        /// </summary>
        public string OperationKey { get; set; }

        /// <summary>
        /// Признак того, что задан фильтр по <see cref="Id"/>.
        /// </summary>
        public bool HasId => Id.HasValue;

        /// <summary>
        /// Признак того, что задан фильтр по <see cref="OperationKey"/>.
        /// </summary>
        public bool HasOperationKey => !string.IsNullOrWhiteSpace(OperationKey);

        /// <summary>
        /// Оставлять только сообщения, у которых в истории статусов есть ошибка.
        /// </summary>
        public bool WithError { get; set; }

        /// <summary>
        /// Оставлять только сообщения, у которых в истории статусов есть <c>SendMessage</c>.
        /// </summary>
        public bool WithSendMessage { get; set; }

        /// <summary>
        /// Нижняя граница диапазона даты создания (включительно).
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Верхняя граница диапазона даты создания (включительно).
        /// </summary>
        public DateTime? CreatedTo { get; set; }

        /// <summary>
        /// Признак того, что задана нижняя граница <see cref="CreatedFrom"/>.
        /// </summary>
        public bool HasCreatedFrom => CreatedFrom.HasValue;

        /// <summary>
        /// Признак того, что задана верхняя граница <see cref="CreatedTo"/>.
        /// </summary>
        public bool HasCreatedTo => CreatedTo.HasValue;
    }
}
