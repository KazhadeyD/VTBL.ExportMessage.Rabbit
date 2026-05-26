using System;
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
    }
}
