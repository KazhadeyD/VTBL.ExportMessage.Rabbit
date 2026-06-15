namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    /// <summary>
    /// Ошибки валидации пользовательского фильтра.
    /// </summary>
    public class IntegrationFilterValidationErrors
    {
        /// <summary>
        /// Ошибка валидации поля идентификатора.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Ошибка валидации поля ключа операции.
        /// </summary>
        public string OperationKey { get; set; }

        /// <summary>
        /// Ошибка валидации нижней границы даты создания.
        /// </summary>
        public string CreatedFrom { get; set; }

        /// <summary>
        /// Ошибка валидации верхней границы даты создания.
        /// </summary>
        public string CreatedTo { get; set; }

        /// <summary>
        /// Ошибка валидации диапазона дат создания.
        /// </summary>
        public string CreatedRange { get; set; }
    }
}
