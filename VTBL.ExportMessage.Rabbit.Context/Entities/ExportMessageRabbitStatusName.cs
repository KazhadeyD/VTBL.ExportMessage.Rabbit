namespace VTBL.ExportMessage.Rabbit.Context.Entities
{
    /// <summary>
    /// Запись справочника статусов интеграции.
    /// </summary>
    public class ExportMessageRabbitStatusName
    {
        /// <summary>
        /// Числовой идентификатор статуса.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Наименование статуса.
        /// </summary>
        public string StatusName { get; set; } = string.Empty;
    }
}
