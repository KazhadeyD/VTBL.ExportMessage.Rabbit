using Microsoft.Extensions.Logging;
using VTBL.RabbitIntegration.Monitor.Context.Entities;
using VTBL.RabbitIntegration.Monitor.UI.Models;
using VTBL.RabbitIntegration.Monitor.UI.Services;

namespace VTBL.RabbitIntegration.Monitor.UI.Pages
{
    /// <summary>
    /// Страница отображения сообщений интеграции Remarketing.
    /// </summary>
    public class RemarketingModel : IntegrationPageModelBase<ExportMessageRabbitRemarketing, RemarketingMessageFilter>
    {
        private readonly IRemarketingMessageService _messageService;

        /// <summary>
        /// Инициализирует страницу Remarketing.
        /// </summary>
        /// <param name="messageService">Сервис сообщений Remarketing.</param>
        /// <param name="logger">Логгер страницы.</param>
        public RemarketingModel(IRemarketingMessageService messageService, ILogger<RemarketingModel> logger)
            : base(logger)
        {
            _messageService = messageService;
        }

        /// <inheritdoc />
        protected override IIntegrationMessageService<ExportMessageRabbitRemarketing> MessageService => _messageService;

        /// <inheritdoc />
        protected override IntegrationSystemDescriptor SystemInfo => IntegrationSystemInfo.Remarketing;

        /// <inheritdoc />
        public override string PageName => "Remarketing";
    }
}
