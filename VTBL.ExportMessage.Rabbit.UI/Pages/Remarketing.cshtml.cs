using Microsoft.Extensions.Logging;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    public class RemarketingModel : IntegrationPageModelBase<ExportMessageRabbitRemarketing, RemarketingMessageFilter>
    {
        private readonly IRemarketingMessageService _messageService;

        public RemarketingModel(IRemarketingMessageService messageService, ILogger<RemarketingModel> logger)
            : base(logger)
        {
            _messageService = messageService;
        }

        protected override IIntegrationMessageService<ExportMessageRabbitRemarketing> MessageService => _messageService;

        protected override IntegrationSystemDescriptor SystemInfo => IntegrationSystemInfo.Remarketing;

        public override string PageName => "Remarketing";
    }
}
