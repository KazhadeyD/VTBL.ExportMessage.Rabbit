namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    /// <summary>
    /// Сырые значения фильтра, полученные из query string до валидации.
    /// </summary>
    public class IntegrationFilterBinding
    {
        /// <summary>
        /// Строковое значение фильтра по идентификатору сообщения.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Строковое значение фильтра по ключу операции.
        /// </summary>
        public string OperationKey { get; set; }

        /// <summary>
        /// Признак фильтра «есть ошибка в статусах».
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Признак фильтра «есть SendMessage в статусах».
        /// </summary>
        public bool HasSendMessage { get; set; }

        /// <summary>
        /// Строковое значение нижней границы даты создания.
        /// </summary>
        public string CreatedFrom { get; set; }

        /// <summary>
        /// Строковое значение верхней границы даты создания.
        /// </summary>
        public string CreatedTo { get; set; }
    }
}
