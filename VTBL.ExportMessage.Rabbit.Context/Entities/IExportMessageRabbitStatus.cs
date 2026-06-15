namespace VTBL.ExportMessage.Rabbit.Context.Entities
{
    /// <summary>
    /// Минимальный контракт статуса интеграционного сообщения.
    /// </summary>
    public interface IExportMessageRabbitStatus
    {
        /// <summary>
        /// Текст ошибки обработки, если есть.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Отправляемое сообщение, связанное со статусом.
        /// </summary>
        string SendMessage { get; }
    }
}
