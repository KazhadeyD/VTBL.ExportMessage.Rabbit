using System;
using System.Collections.Generic;

namespace VTBL.RabbitIntegration.Monitor.Context.Entities
{
    /// <summary>
    /// Сообщение интеграции NOVA.
    /// </summary>
    public class ExportMessageRabbitNova : IExportMessageRabbitMessage<ExportMessageRabbitNovaStatus>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <summary>
        /// Внешний идентификатор сообщения RabbitMQ, если задан.
        /// </summary>
        public Guid? MessageId { get; set; }

        /// <inheritdoc />
        public string OperationKey { get; set; } = string.Empty;

        /// <summary>
        /// Конфигурация операции (<see cref="OperationKey"/> = <see cref="RabbitIntegrationOperationKeysConfiguration.Key"/>).
        /// </summary>
        public RabbitIntegrationOperationKeysConfiguration OperationConfiguration { get; set; }

        /// <summary>
        /// Конечная точка обработки сообщения.
        /// </summary>
        public string Endpoint { get; set; } = string.Empty;

        /// <inheritdoc />
        public DateTime Created { get; set; }

        /// <summary>
        /// Тело сообщения.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// История статусов. Связь с <see cref="ExportMessageRabbitNovaStatus.IntegrationId"/> (FK в БД нет).
        /// </summary>
        public ICollection<ExportMessageRabbitNovaStatus> StatusHistory { get; set; }
            = new List<ExportMessageRabbitNovaStatus>();
    }
}
