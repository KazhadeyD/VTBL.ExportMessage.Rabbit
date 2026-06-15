using VTBL.RabbitIntegration.Monitor.Context.Entities;

namespace VTBL.RabbitIntegration.Monitor.UI.Models
{
    /// <summary>
    /// Результат постраничной выборки сообщений Remarketing.
    /// </summary>
    public class RemarketingMessagesPageResult : IntegrationMessagesPageResult<ExportMessageRabbitRemarketing>
    {
    }
}
