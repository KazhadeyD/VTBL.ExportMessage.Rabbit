using System;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public class KkaMessageFilter
    {
        public Guid? Id { get; set; }

        public string OperationKey { get; set; }

        public bool HasId => Id.HasValue;

        public bool HasOperationKey => !string.IsNullOrWhiteSpace(OperationKey);
    }
}
