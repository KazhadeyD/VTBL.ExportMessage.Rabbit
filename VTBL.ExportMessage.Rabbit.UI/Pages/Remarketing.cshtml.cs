using Microsoft.Extensions.Logging;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
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
