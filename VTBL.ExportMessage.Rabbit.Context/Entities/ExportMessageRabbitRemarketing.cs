using System;
using System.Collections.Generic;

namespace VTBL.ExportMessage.Rabbit.Context.Entities
{
    public class ExportMessageRabbitRemarketing : IExportMessageRabbitMessage<ExportMessageRabbitRemarketingStatus>
    {
        public Guid Id { get; set; }

        public Guid? MessageId { get; set; }

        public string OperationKey { get; set; } = string.Empty;

        /// <summary>
        /// Конфигурация операции (<see cref="OperationKey"/> = <see cref="RabbitIntegrationOperationKeysConfiguration.Key"/>).
        /// </summary>
        public RabbitIntegrationOperationKeysConfiguration OperationConfiguration { get; set; }

        public string Endpoint { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public string Body { get; set; }

        /// <summary>
        /// История статусов. Связь с <see cref="ExportMessageRabbitRemarketingStatus.IntegrationId"/> (FK в БД нет).
        /// </summary>
        public ICollection<ExportMessageRabbitRemarketingStatus> StatusHistory { get; set; }
            = new List<ExportMessageRabbitRemarketingStatus>();
    }
}
