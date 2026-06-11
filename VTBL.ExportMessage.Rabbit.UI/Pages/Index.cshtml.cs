using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public IReadOnlyList<IntegrationDashboardEntry> Systems { get; private set; }
            = Array.Empty<IntegrationDashboardEntry>();

        public async Task OnGetAsync()
        {
            var kkaTask = LoadEntryAsync(
                IntegrationSystemInfo.Kka,
                "Kka",
                () => _kkaMessageService.GetDashboardStatsAsync());

            var novaTask = LoadEntryAsync(
                IntegrationSystemInfo.Nova,
                "Nova",
                () => _novaMessageService.GetDashboardStatsAsync());

            var remarketingTask = LoadEntryAsync(
                IntegrationSystemInfo.Remarketing,
                "Remarketing",
                () => _remarketingMessageService.GetDashboardStatsAsync());

            Systems = await Task.WhenAll(kkaTask, novaTask, remarketingTask).ConfigureAwait(false);
        }

        private async Task<IntegrationDashboardEntry> LoadEntryAsync(
            IntegrationSystemDescriptor system,
            string pageRoute,
            Func<Task<IntegrationDashboardStats>> loadStats)
        {
            try
            {
                return new IntegrationDashboardEntry
                {
                    System = system,
                    PageRoute = pageRoute,
                    Stats = await loadStats().ConfigureAwait(false),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load dashboard stats for {System}.", system.DisplayName);
                return new IntegrationDashboardEntry
                {
                    System = system,
                    PageRoute = pageRoute,
                    LoadError = IntegrationDatabaseErrorFormatter.ToUserMessage(ex, system),
                };
            }
        }
    }
}
