using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    /// <summary>
    /// Результат постраничной выборки сообщений Remarketing.
    /// </summary>
    public class RemarketingMessagesPageResult : IntegrationMessagesPageResult<ExportMessageRabbitRemarketing>
    {
    }
}
