using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VTBL.ExportMessage.Rabbit.Context.Entities
{
    /// <summary>
    /// Конфигурация маршрутизации Rabbit для конкретного <c>OperationKey</c>.
    /// </summary>
    public class RabbitIntegrationOperationKeysConfiguration
    {
        /// <summary>
        /// Идентификатор записи конфигурации.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ключ операции интеграции.
        /// </summary>
        [Column("Key")]
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// URL сервиса сборки пакета.
        /// </summary>
        public string PackageBuilderURL { get; set; } = string.Empty;

        /// <summary>
        /// Exchange RabbitMQ для отправки пакета.
        /// </summary>
        public string SendPackageRabbitExchange { get; set; } = string.Empty;

        /// <summary>
        /// Routing key RabbitMQ для отправки пакета.
        /// </summary>
        public string SendPackageRabbitRoutingkey { get; set; } = string.Empty;

        /// <summary>
        /// Сообщения ККА с этим ключом операции (FK в БД нет).
        /// </summary>
        public ICollection<ExportMessageRabbitKka> ExportMessages { get; set; }
            = new List<ExportMessageRabbitKka>();

        /// <summary>
        /// Сообщения NOVA с этим ключом операции (FK в БД нет).
        /// </summary>
        public ICollection<ExportMessageRabbitNova> NovaExportMessages { get; set; }
            = new List<ExportMessageRabbitNova>();

        /// <summary>
        /// Сообщения Remarketing с этим ключом операции (FK в БД нет).
        /// </summary>
        public ICollection<ExportMessageRabbitRemarketing> RemarketingExportMessages { get; set; }
            = new List<ExportMessageRabbitRemarketing>();
    }
}
