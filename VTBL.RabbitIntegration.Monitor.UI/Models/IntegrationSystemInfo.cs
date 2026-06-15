namespace VTBL.RabbitIntegration.Monitor.UI.Models
{
    /// <summary>
    /// Каталог поддерживаемых интеграционных систем и связанных таблиц базы данных.
    /// </summary>
    public static class IntegrationSystemInfo
    {
        /// <summary>
        /// Описание интеграционной системы ККА.
        /// </summary>
        public static readonly IntegrationSystemDescriptor Kka = new IntegrationSystemDescriptor(
            "ККА",
            "ExportMessageRabbitKKA",
            "ExportMessageRabbitKKAStatus");

        /// <summary>
        /// Описание интеграционной системы NOVA.
        /// </summary>
        public static readonly IntegrationSystemDescriptor Nova = new IntegrationSystemDescriptor(
            "NOVA",
            "ExportMessageRabbitNOVA",
            "ExportMessageRabbitNOVAStatus");

        /// <summary>
        /// Описание интеграционной системы Remarketing.
        /// </summary>
        public static readonly IntegrationSystemDescriptor Remarketing = new IntegrationSystemDescriptor(
            "Remarketing",
            "ExportMessageRabbitREMARKETING",
            "ExportMessageRabbitREMARKETINGStatus");
    }

    /// <summary>
    /// Описание интеграционной системы для UI и форматирования ошибок БД.
    /// </summary>
    public class IntegrationSystemDescriptor
    {
        /// <summary>
        /// Создаёт описание интеграционной системы.
        /// </summary>
        /// <param name="displayName">Отображаемое имя системы.</param>
        /// <param name="messageTables">Имена таблиц сообщений и статусов в БД.</param>
        public IntegrationSystemDescriptor(string displayName, params string[] messageTables)
        {
            DisplayName = displayName;
            MessageTables = messageTables;
        }

        /// <summary>
        /// Отображаемое имя системы в интерфейсе.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Имена таблиц системы в базе <c>MSCRM_EXT</c>.
        /// </summary>
        public string[] MessageTables { get; }
    }
}
