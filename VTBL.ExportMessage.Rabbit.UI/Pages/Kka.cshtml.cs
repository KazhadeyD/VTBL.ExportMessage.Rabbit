using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    public class KkaModel : PageModel
    {
        private readonly IKkaMessageService _kkaMessageService;
        private readonly ILogger<KkaModel> _logger;

        public KkaModel(IKkaMessageService kkaMessageService, ILogger<KkaModel> logger)
        {
            _kkaMessageService = kkaMessageService;
            _logger = logger;
        }

        public IReadOnlyList<ExportMessageRabbitKka> MessageGroups { get; private set; }
            = Array.Empty<ExportMessageRabbitKka>();

        public IReadOnlyDictionary<int, string> StatusNames { get; private set; }
            = new Dictionary<int, string>();

        public string ErrorMessage { get; private set; }

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            try
            {
                MessageGroups = await _kkaMessageService.GetMessagesGroupedByIdAsync().ConfigureAwait(false);
                StatusNames = await _kkaMessageService.GetStatusNameMapAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load KKA messages.");
                ErrorMessage = ex.Message;
            }
        }

        public string ResolveStatusName(int? statusId)
        {
            if (!statusId.HasValue)
            {
                return "—";
            }

            return StatusNames.TryGetValue(statusId.Value, out var name) ? name : statusId.Value.ToString();
        }
    }
}
