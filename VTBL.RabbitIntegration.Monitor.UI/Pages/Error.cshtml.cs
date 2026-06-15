using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace VTBL.RabbitIntegration.Monitor.UI.Pages
{
    /// <summary>
    /// Страница отображения неперехваченных ошибок приложения.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        /// <summary>
        /// Идентификатор запроса для корреляции с логами.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Признак того, что идентификатор запроса доступен для отображения.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        /// <summary>
        /// Инициализирует страницу ошибки.
        /// </summary>
        /// <param name="logger">Логгер страницы.</param>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Обрабатывает GET-запрос к странице ошибки.
        /// </summary>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
