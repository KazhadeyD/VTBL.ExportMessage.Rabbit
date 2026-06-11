using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IKkaMessageService _kkaMessageService;
        private readonly INovaMessageService _novaMessageService;
        private readonly IRemarketingMessageService _remarketingMessageService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IKkaMessageService kkaMessageService,
            INovaMessageService novaMessageService,
            IRemarketingMessageService remarketingMessageService,
            ILogger<IndexModel> logger)
        {
            _kkaMessageService = kkaMessageService;
            _novaMessageService = novaMessageService;
            _remarketingMessageService = remarketingMessageService;
            _logger = logger;
        }

        public IntegrationDashboardStats KkaStats { get; private set; } = new IntegrationDashboardStats();

        public IntegrationDashboardStats NovaStats { get; private set; } = new IntegrationDashboardStats();

        public IntegrationDashboardStats RemarketingStats { get; private set; } = new IntegrationDashboardStats();

        public async Task OnGetAsync()
        {
            KkaStats = await _kkaMessageService.GetDashboardStatsAsync().ConfigureAwait(false);
            NovaStats = await _novaMessageService.GetDashboardStatsAsync().ConfigureAwait(false);
            RemarketingStats = await _remarketingMessageService.GetDashboardStatsAsync().ConfigureAwait(false);
        }
    }
}
