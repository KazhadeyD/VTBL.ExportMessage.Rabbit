using Microsoft.Extensions.Logging;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    public class KkaModel : IntegrationPageModelBase<ExportMessageRabbitKka, KkaMessageFilter>
    {
        private readonly IKkaMessageService _messageService;

        public KkaModel(IKkaMessageService messageService, ILogger<KkaModel> logger)
            : base(logger)
        {
            _messageService = messageService;
        }

        protected override IIntegrationMessageService<ExportMessageRabbitKka> MessageService => _messageService;

        protected override IntegrationSystemDescriptor SystemInfo => IntegrationSystemInfo.Kka;

        public override string PageName => "Kka";
    }
}
