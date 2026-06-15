using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    /// <summary>
    /// Результат постраничной выборки сообщений ККА.
    /// </summary>
    public class KkaMessagesPageResult : IntegrationMessagesPageResult<ExportMessageRabbitKka>
    {
    }
}
