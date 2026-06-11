using Microsoft.Extensions.Logging;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    public class NovaModel : IntegrationPageModelBase<ExportMessageRabbitNova, NovaMessageFilter>
    {
        private readonly INovaMessageService _messageService;

        public NovaModel(INovaMessageService messageService, ILogger<NovaModel> logger)
            : base(logger)
        {
            _messageService = messageService;
        }

        protected override IIntegrationMessageService<ExportMessageRabbitNova> MessageService => _messageService;

        protected override IntegrationSystemDescriptor SystemInfo => IntegrationSystemInfo.Nova;

        public override string PageName => "Nova";
    }
}
