using System;
using System.Collections.Generic;

namespace VTBL.ExportMessage.Rabbit.Context.Entities
{
    /// <summary>
    /// Сообщение интеграции ККА.
    /// </summary>
    public class ExportMessageRabbitKka : IExportMessageRabbitMessage<ExportMessageRabbitKkaStatus>
    {
        /// <summary>
        /// Идентификатор записи сообщения.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Внешний идентификатор сообщения RabbitMQ, если задан.
        /// </summary>
        public Guid? MessageId { get; set; }

        /// <summary>
        /// Ключ операции интеграции.
        /// </summary>
        public string OperationKey { get; set; } = string.Empty;

        /// <summary>
        /// Конфигурация операции (<see cref="OperationKey"/> = <see cref="RabbitIntegrationOperationKeysConfiguration.Key"/>).
        /// </summary>
        public RabbitIntegrationOperationKeysConfiguration OperationConfiguration { get; set; }

        /// <summary>
        /// Конечная точка обработки сообщения.
        /// </summary>
        public string Endpoint { get; set; } = string.Empty;

        /// <summary>
        /// Дата и время создания записи.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Тело сообщения.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// История статусов. Связь с <see cref="ExportMessageRabbitKkaStatus.IntegrationId"/> (FK в БД нет).
        /// </summary>
        public ICollection<ExportMessageRabbitKkaStatus> StatusHistory { get; set; }
            = new List<ExportMessageRabbitKkaStatus>();
    }
}
