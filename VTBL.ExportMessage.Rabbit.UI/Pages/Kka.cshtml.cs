using Microsoft.Extensions.Logging;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
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
