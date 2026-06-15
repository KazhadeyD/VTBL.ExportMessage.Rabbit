using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    /// <summary>
    /// Результат постраничной выборки сообщений NOVA.
    /// </summary>
    public class NovaMessagesPageResult : IntegrationMessagesPageResult<ExportMessageRabbitNova>
    {
    }
}
