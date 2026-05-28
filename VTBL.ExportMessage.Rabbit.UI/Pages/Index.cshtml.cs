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
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IKkaMessageService kkaMessageService, ILogger<IndexModel> logger)
        {
            _kkaMessageService = kkaMessageService;
            _logger = logger;
        }

        public KkaDashboardStats DashboardStats { get; private set; } = new KkaDashboardStats();

        public async Task OnGetAsync()
        {
            DashboardStats = await _kkaMessageService.GetDashboardStatsAsync().ConfigureAwait(false);
        }
    }
}
