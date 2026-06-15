using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    /// <summary>
    /// Сервис чтения сообщений интеграции ККА.
    /// </summary>
    public interface IKkaMessageService : IIntegrationMessageService<ExportMessageRabbitKka>
    {
    }
}
