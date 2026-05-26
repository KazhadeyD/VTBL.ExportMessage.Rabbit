using System;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public class KkaMessageFilter
    {
        public Guid? Id { get; set; }

        public string OperationKey { get; set; }

        public bool HasId => Id.HasValue;

        public bool HasOperationKey => !string.IsNullOrWhiteSpace(OperationKey);

        /// <summary>Хотя бы один статус с заполненным ErrorMessage.</summary>
        public bool WithError { get; set; }

        /// <summary>Хотя бы один статус с заполненным SendMessage.</summary>
        public bool WithSendMessage { get; set; }
    }
}
