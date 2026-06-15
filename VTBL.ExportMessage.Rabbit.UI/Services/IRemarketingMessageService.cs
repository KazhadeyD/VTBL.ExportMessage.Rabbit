using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    /// <summary>
    /// Сервис чтения сообщений интеграции Remarketing.
    /// </summary>
    public interface IRemarketingMessageService : IIntegrationMessageService<ExportMessageRabbitRemarketing>
    {
    }
}
