using VTBL.RabbitIntegration.Monitor.Context.Entities;

namespace VTBL.RabbitIntegration.Monitor.UI.Services
{
    /// <summary>
    /// Сервис чтения сообщений интеграции NOVA.
    /// </summary>
    public interface INovaMessageService : IIntegrationMessageService<ExportMessageRabbitNova>
    {
    }
}
