using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
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

        public string KkaStatsError { get; private set; }

        public string NovaStatsError { get; private set; }

        public string RemarketingStatsError { get; private set; }

        public async Task OnGetAsync()
        {
            await LoadStatsAsync(
                IntegrationSystemInfo.Kka,
                () => _kkaMessageService.GetDashboardStatsAsync(),
                stats => KkaStats = stats,
                error => KkaStatsError = error).ConfigureAwait(false);

            await LoadStatsAsync(
                IntegrationSystemInfo.Nova,
                () => _novaMessageService.GetDashboardStatsAsync(),
                stats => NovaStats = stats,
                error => NovaStatsError = error).ConfigureAwait(false);

            await LoadStatsAsync(
                IntegrationSystemInfo.Remarketing,
                () => _remarketingMessageService.GetDashboardStatsAsync(),
                stats => RemarketingStats = stats,
                error => RemarketingStatsError = error).ConfigureAwait(false);
        }

        private async Task LoadStatsAsync(
            IntegrationSystemDescriptor system,
            Func<Task<IntegrationDashboardStats>> loadStats,
            Action<IntegrationDashboardStats> setStats,
            Action<string> setError)
        {
            try
            {
                setStats(await loadStats().ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load dashboard stats for {System}.", system.DisplayName);
                setError(IntegrationDatabaseErrorFormatter.ToUserMessage(ex, system));
            }
        }
    }
}
