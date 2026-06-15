using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    /// <summary>
    /// Сервис чтения сообщений интеграции NOVA.
    /// </summary>
    public interface INovaMessageService : IIntegrationMessageService<ExportMessageRabbitNova>
    {
    }
}
