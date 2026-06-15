using VTBL.RabbitIntegration.Monitor.Context.Entities;

namespace VTBL.RabbitIntegration.Monitor.UI.Models
{
    /// <summary>
    /// Результат постраничной выборки сообщений ККА.
    /// </summary>
    public class KkaMessagesPageResult : IntegrationMessagesPageResult<ExportMessageRabbitKka>
    {
    }
}
