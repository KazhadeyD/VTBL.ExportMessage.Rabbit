using System;
using System.ComponentModel.DataAnnotations;

namespace VTBL.ExportMessage.Rabbit.Context.Entities
{
    public class ExportMessageRabbitKkaStatus
    {
        public Guid Id { get; set; }

        public Guid IntegrationId { get; set; }

        public int? StatusId { get; set; }

        public DateTime Created { get; set; }

        public Guid? ProcessingId { get; set; }

        public string ErrorMessage { get; set; }

        public string SendMessage { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
