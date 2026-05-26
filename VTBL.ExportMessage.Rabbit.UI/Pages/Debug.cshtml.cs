using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    public class DebugModel : PageModel
    {
        private readonly IExportMessageRabbitStatusNameService _statusNameService;
        private readonly ILogger<DebugModel> _logger;

        public DebugModel(
            IExportMessageRabbitStatusNameService statusNameService,
            ILogger<DebugModel> logger)
        {
            _statusNameService = statusNameService;
            _logger = logger;
        }

        public IReadOnlyList<ExportMessageRabbitStatusName> StatusNames { get; private set; }
            = Array.Empty<ExportMessageRabbitStatusName>();

        public bool IsLoaded { get; private set; }

        public string ErrorMessage { get; private set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadAsync()
        {
            try
            {
                StatusNames = await _statusNameService.GetAllAsync().ConfigureAwait(false);
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load ExportMessageRabbitStatusName.");
                ErrorMessage = ex.Message;
                IsLoaded = true;
            }

            return Page();
        }
    }
}
