namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    /// <summary>
    /// Агрегированные показатели по сообщениям системы.
    /// </summary>
    public class IntegrationDashboardStats
    {
        /// <summary>
        /// Общее количество сообщений.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Количество сообщений без ошибок в истории статусов.
        /// </summary>
        public int Successful { get; set; }

        /// <summary>
        /// Количество сообщений с ошибками в истории статусов.
        /// </summary>
        public int Failed { get; set; }
    }
}
