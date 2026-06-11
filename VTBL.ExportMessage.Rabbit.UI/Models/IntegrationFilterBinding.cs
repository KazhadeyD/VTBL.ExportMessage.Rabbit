namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public class IntegrationFilterBinding
    {
        public string Id { get; set; }

        public string OperationKey { get; set; }

        public bool HasError { get; set; }

        public bool HasSendMessage { get; set; }

        public string CreatedFrom { get; set; }

        public string CreatedTo { get; set; }
    }
}
