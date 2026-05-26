using System;

namespace VTBL.ExportMessage.Rabbit.Context.Entities
{
    public class ExportMessageRabbitKka
    {
        public Guid Id { get; set; }

        public Guid? MessageId { get; set; }

        public string OperationKey { get; set; } = string.Empty;

        public string Endpoint { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public string Body { get; set; }
    }
}
