using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VTBL.RabbitIntegration.Monitor.Context.Entities;
using VTBL.RabbitIntegration.Monitor.UI.Services;

namespace VTBL.RabbitIntegration.Monitor.UI.Pages
{
    /// <summary>
    /// Отладочная страница загрузки справочника статусов из БД.
    /// </summary>
    public class DebugModel : PageModel
    {
        private readonly IExportMessageRabbitStatusNameService _statusNameService;
        private readonly ILogger<DebugModel> _logger;

        /// <summary>
        /// Инициализирует отладочную страницу.
        /// </summary>
        public DebugModel(
            IExportMessageRabbitStatusNameService statusNameService,
            ILogger<DebugModel> logger)
        {
            _statusNameService = statusNameService;
            _logger = logger;
        }

        /// <summary>
        /// Загруженные записи справочника статусов.
        /// </summary>
        public IReadOnlyList<ExportMessageRabbitStatusName> StatusNames { get; private set; }
            = Array.Empty<ExportMessageRabbitStatusName>();

        /// <summary>
        /// Признак того, что была выполнена попытка загрузки данных.
        /// </summary>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Текст ошибки загрузки справочника, если она произошла.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Обрабатывает GET-запрос к странице Debug.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// Загружает справочник статусов по кнопке на странице Debug.
        /// </summary>
        /// <returns>Та же страница с результатом загрузки.</returns>
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
