using VTBL.RabbitIntegration.Monitor.Context.Entities;

namespace VTBL.RabbitIntegration.Monitor.UI.Services
{
    /// <summary>
    /// Сервис чтения сообщений интеграции Remarketing.
    /// </summary>
    public interface IRemarketingMessageService : IIntegrationMessageService<ExportMessageRabbitRemarketing>
    {
    }
}
