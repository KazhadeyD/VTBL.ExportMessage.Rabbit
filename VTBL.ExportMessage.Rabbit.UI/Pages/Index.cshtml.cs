using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    /// <summary>
    /// Главная страница с агрегированной сводкой по интеграционным системам.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly IKkaMessageService _kkaMessageService;
        private readonly INovaMessageService _novaMessageService;
        private readonly IRemarketingMessageService _remarketingMessageService;
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Инициализирует главную страницу.
        /// </summary>
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

        /// <summary>
        /// Строки дашборда по интеграционным системам.
        /// </summary>
        public IReadOnlyList<IntegrationDashboardEntry> Systems { get; private set; }
            = Array.Empty<IntegrationDashboardEntry>();

        /// <summary>
        /// Загружает статистику систем для таблицы дашборда.
        /// </summary>
        public async Task OnGetAsync()
        {
            // Один scoped DbContext на запрос — параллельные вызовы через Task.WhenAll недопустимы.
            var entries = new IntegrationDashboardEntry[3];

            entries[0] = await LoadEntryAsync(
                IntegrationSystemInfo.Kka,
                "Kka",
                () => _kkaMessageService.GetDashboardStatsAsync()).ConfigureAwait(false);

            entries[1] = await LoadEntryAsync(
                IntegrationSystemInfo.Nova,
                "Nova",
                () => _novaMessageService.GetDashboardStatsAsync()).ConfigureAwait(false);

            entries[2] = await LoadEntryAsync(
                IntegrationSystemInfo.Remarketing,
                "Remarketing",
                () => _remarketingMessageService.GetDashboardStatsAsync()).ConfigureAwait(false);

            Systems = entries;
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
