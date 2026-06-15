using Microsoft.Extensions.Logging;
using VTBL.RabbitIntegration.Monitor.Context.Entities;
using VTBL.RabbitIntegration.Monitor.UI.Models;
using VTBL.RabbitIntegration.Monitor.UI.Services;

namespace VTBL.RabbitIntegration.Monitor.UI.Pages
{
    /// <summary>
    /// Страница отображения сообщений интеграции ККА.
    /// </summary>
    public class KkaModel : IntegrationPageModelBase<ExportMessageRabbitKka, KkaMessageFilter>
    {
        private readonly IKkaMessageService _messageService;

        /// <summary>
        /// Инициализирует страницу ККА.
        /// </summary>
        /// <param name="messageService">Сервис сообщений ККА.</param>
        /// <param name="logger">Логгер страницы.</param>
        public KkaModel(IKkaMessageService messageService, ILogger<KkaModel> logger)
            : base(logger)
        {
            _messageService = messageService;
        }

        /// <inheritdoc />
        protected override IIntegrationMessageService<ExportMessageRabbitKka> MessageService => _messageService;

        /// <inheritdoc />
        protected override IntegrationSystemDescriptor SystemInfo => IntegrationSystemInfo.Kka;

        /// <inheritdoc />
        public override string PageName => "Kka";
    }
}
