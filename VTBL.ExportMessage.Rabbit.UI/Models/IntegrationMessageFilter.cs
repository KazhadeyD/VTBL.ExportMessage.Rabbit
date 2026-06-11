using System;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public class IntegrationMessageFilter
    {
        public Guid? Id { get; set; }

        public string OperationKey { get; set; }

        public bool HasId => Id.HasValue;

        public bool HasOperationKey => !string.IsNullOrWhiteSpace(OperationKey);

        public bool WithError { get; set; }

        public bool WithSendMessage { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public bool HasCreatedFrom => CreatedFrom.HasValue;

        public bool HasCreatedTo => CreatedTo.HasValue;
    }
}
