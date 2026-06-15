using VTBL.RabbitIntegration.Monitor.Context.Entities;

namespace VTBL.RabbitIntegration.Monitor.UI.Services
{
    /// <summary>
    /// Сервис чтения сообщений интеграции ККА.
    /// </summary>
    public interface IKkaMessageService : IIntegrationMessageService<ExportMessageRabbitKka>
    {
    }
}
