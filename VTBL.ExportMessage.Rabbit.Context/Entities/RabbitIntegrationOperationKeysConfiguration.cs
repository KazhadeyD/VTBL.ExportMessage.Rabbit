using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VTBL.ExportMessage.Rabbit.Context.Entities
{
    public class RabbitIntegrationOperationKeysConfiguration
    {
        public Guid Id { get; set; }

        [Column("Key")]
        public string Key { get; set; } = string.Empty;

        public string PackageBuilderURL { get; set; } = string.Empty;

        public string SendPackageRabbitExchange { get; set; } = string.Empty;

        public string SendPackageRabbitRoutingkey { get; set; } = string.Empty;

        /// <summary>
        /// Сообщения с этим ключом операции (FK в БД нет).
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
