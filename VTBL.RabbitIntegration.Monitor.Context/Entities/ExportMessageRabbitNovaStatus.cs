using System;
using System.ComponentModel.DataAnnotations;

namespace VTBL.RabbitIntegration.Monitor.Context.Entities
{
    /// <summary>
    /// Статус обработки сообщения интеграции NOVA.
    /// </summary>
    public class ExportMessageRabbitNovaStatus : IExportMessageRabbitStatus
    {
        /// <summary>
        /// Идентификатор записи статуса.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор связанного сообщения интеграции.
        /// </summary>
        public Guid IntegrationId { get; set; }

        /// <summary>
        /// Сообщение интеграции (<see cref="ExportMessageRabbitNova.Id"/> = <see cref="IntegrationId"/>).
        /// </summary>
        public ExportMessageRabbitNova Integration { get; set; }

        /// <summary>
        /// Идентификатор статуса из справочника <see cref="ExportMessageRabbitStatusName"/>.
        /// </summary>
        public int? StatusId { get; set; }

        /// <summary>
        /// Дата и время фиксации статуса.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Идентификатор процесса обработки, если задан.
        /// </summary>
        public Guid? ProcessingId { get; set; }

        /// <inheritdoc />
        public string ErrorMessage { get; set; }

        /// <inheritdoc />
        public string SendMessage { get; set; }

        /// <summary>
        /// Версия строки SQL Server для упорядочивания истории статусов.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
