using VTBL.RabbitIntegration.Monitor.Context.Entities;

namespace VTBL.RabbitIntegration.Monitor.UI.Models
{
    /// <summary>
    /// Результат постраничной выборки сообщений NOVA.
    /// </summary>
    public class NovaMessagesPageResult : IntegrationMessagesPageResult<ExportMessageRabbitNova>
    {
    }
}
