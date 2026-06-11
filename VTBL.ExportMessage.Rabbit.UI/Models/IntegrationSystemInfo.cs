namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public static class IntegrationSystemInfo
    {
        public static readonly IntegrationSystemDescriptor Kka = new IntegrationSystemDescriptor(
            "ККА",
            "ExportMessageRabbitKKA",
            "ExportMessageRabbitKKAStatus");

        public static readonly IntegrationSystemDescriptor Nova = new IntegrationSystemDescriptor(
            "NOVA",
            "ExportMessageRabbitNOVA",
            "ExportMessageRabbitNOVAStatus");

        public static readonly IntegrationSystemDescriptor Remarketing = new IntegrationSystemDescriptor(
            "Remarketing",
            "ExportMessageRabbitREMARKETING",
            "ExportMessageRabbitREMARKETINGStatus");
    }

    public class IntegrationSystemDescriptor
    {
        public IntegrationSystemDescriptor(string displayName, params string[] messageTables)
        {
            DisplayName = displayName;
            MessageTables = messageTables;
        }

        public string DisplayName { get; }

        public string[] MessageTables { get; }
    }
}
