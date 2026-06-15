using Microsoft.Extensions.Logging;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
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
