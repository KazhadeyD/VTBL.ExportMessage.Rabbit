using Microsoft.Extensions.Logging;
using VTBL.RabbitIntegration.Monitor.Context.Entities;
using VTBL.RabbitIntegration.Monitor.UI.Models;
using VTBL.RabbitIntegration.Monitor.UI.Services;

namespace VTBL.RabbitIntegration.Monitor.UI.Pages
{
    /// <summary>
    /// Страница отображения сообщений интеграции NOVA.
    /// </summary>
    public class NovaModel : IntegrationPageModelBase<ExportMessageRabbitNova, NovaMessageFilter>
    {
        private readonly INovaMessageService _messageService;

        /// <summary>
        /// Инициализирует страницу NOVA.
        /// </summary>
        /// <param name="messageService">Сервис сообщений NOVA.</param>
        /// <param name="logger">Логгер страницы.</param>
        public NovaModel(INovaMessageService messageService, ILogger<NovaModel> logger)
            : base(logger)
        {
            _messageService = messageService;
        }

        /// <inheritdoc />
        protected override IIntegrationMessageService<ExportMessageRabbitNova> MessageService => _messageService;

        /// <inheritdoc />
        protected override IntegrationSystemDescriptor SystemInfo => IntegrationSystemInfo.Nova;

        /// <inheritdoc />
        public override string PageName => "Nova";
    }
}
