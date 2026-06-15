using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using VTBL.RabbitIntegration.Monitor.UI.Models;
using VTBL.RabbitIntegration.Monitor.UI.Services;

namespace VTBL.RabbitIntegration.Monitor.UI.Pages
{
    /// <summary>
    /// Базовая PageModel для страниц интеграций: связывание фильтров, загрузка результатов и обработка ошибок.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения интеграции.</typeparam>
    /// <typeparam name="TFilter">Тип фильтра сообщений.</typeparam>
    public abstract class IntegrationPageModelBase<TMessage, TFilter> : PageModel, IIntegrationPageModel
        where TMessage : class
        where TFilter : IntegrationMessageFilter, new()
    {
        private readonly ILogger _logger;
        private IReadOnlyList<TMessage> _messageGroups = Array.Empty<TMessage>();

        /// <summary>
        /// Инициализирует базовую PageModel интеграции.
        /// </summary>
        /// <param name="logger">Логгер страницы.</param>
        protected IntegrationPageModelBase(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Сервис чтения сообщений текущей интеграции.
        /// </summary>
        protected abstract IIntegrationMessageService<TMessage> MessageService { get; }

        /// <summary>
        /// Метаданные интеграционной системы для UI и ошибок БД.
        /// </summary>
        protected abstract IntegrationSystemDescriptor SystemInfo { get; }

        /// <inheritdoc />
        public abstract string PageName { get; }

        /// <inheritdoc />
        public string PageTitle => SystemInfo.DisplayName;

        /// <inheritdoc />
        [BindProperty(SupportsGet = true, Name = "id")]
        public string FilterId { get; set; }

        /// <inheritdoc />
        [BindProperty(SupportsGet = true, Name = "operationKey")]
        public string FilterOperationKey { get; set; }

        /// <inheritdoc />
        [BindProperty(SupportsGet = true, Name = "hasError")]
        public bool FilterHasError { get; set; }

        /// <inheritdoc />
        [BindProperty(SupportsGet = true, Name = "hasSendMessage")]
        public bool FilterHasSendMessage { get; set; }

        /// <inheritdoc />
        [BindProperty(SupportsGet = true, Name = "createdFrom")]
        public string FilterCreatedFrom { get; set; }

        /// <inheritdoc />
        [BindProperty(SupportsGet = true, Name = "createdTo")]
        public string FilterCreatedTo { get; set; }

        /// <summary>
        /// Запрошенный пользователем размер страницы из query string.
        /// </summary>
        [BindProperty(SupportsGet = true, Name = "pageSize")]
        public int? FilterPageSize { get; set; }

        /// <inheritdoc />
        public IReadOnlyList<string> OperationKeys { get; private set; }
            = Array.Empty<string>();

        /// <inheritdoc />
        IEnumerable IIntegrationPageModel.MessageGroups => _messageGroups;

        /// <summary>
        /// Словарь идентификатор статуса — наименование для отображения в UI.
        /// </summary>
        public IReadOnlyDictionary<int, string> StatusNames { get; private set; }
            = new Dictionary<int, string>();

        /// <inheritdoc />
        public int PageNumber { get; private set; } = 1;

        /// <inheritdoc />
        public int PageSize => IntegrationPageSize.Normalize(FilterPageSize);

        /// <inheritdoc />
        public int TotalCount { get; private set; }

        /// <inheritdoc />
        public int TotalPages { get; private set; }

        /// <inheritdoc />
        public bool HasPrevious { get; private set; }

        /// <inheritdoc />
        public bool HasNext { get; private set; }

        /// <inheritdoc />
        public int RangeFrom { get; private set; }

        /// <inheritdoc />
        public int RangeTo { get; private set; }

        /// <inheritdoc />
        public string FilterIdError { get; private set; }

        /// <inheritdoc />
        public string FilterOperationKeyError { get; private set; }

        /// <inheritdoc />
        public string FilterCreatedFromError { get; private set; }

        /// <inheritdoc />
        public string FilterCreatedToError { get; private set; }

        /// <inheritdoc />
        public string FilterCreatedRangeError { get; private set; }

        /// <inheritdoc />
        public bool HasActiveFilter { get; private set; }

        /// <inheritdoc />
        public string ErrorMessage { get; private set; }

        /// <inheritdoc />
        public string ResultsErrorMessage { get; private set; }

        /// <inheritdoc />
        public string FilterIdForRoute =>
            string.IsNullOrWhiteSpace(FilterId) ? null : FilterId.Trim();

        /// <inheritdoc />
        public string FilterOperationKeyForRoute =>
            string.IsNullOrWhiteSpace(FilterOperationKey) ? null : FilterOperationKey.Trim();

        /// <inheritdoc />
        public bool? FilterHasErrorForRoute => FilterHasError ? true : (bool?)null;

        /// <inheritdoc />
        public bool? FilterHasSendMessageForRoute => FilterHasSendMessage ? true : (bool?)null;

        /// <inheritdoc />
        public string FilterCreatedFromForRoute =>
            string.IsNullOrWhiteSpace(FilterCreatedFrom) ? null : FilterCreatedFrom.Trim();

        /// <inheritdoc />
        public string FilterCreatedToForRoute =>
            string.IsNullOrWhiteSpace(FilterCreatedTo) ? null : FilterCreatedTo.Trim();

        /// <summary>
        /// Загружает страницу интеграции при полном рендеринге.
        /// </summary>
        /// <param name="pageNumber">Номер запрашиваемой страницы результатов.</param>
        public async Task OnGetAsync(int pageNumber = 1)
        {
            try
            {
                OperationKeys = await MessageService.GetOperationKeysAsync().ConfigureAwait(false);

                if (!TryBuildFilter(OperationKeys, out var filter))
                {
                    StatusNames = await MessageService.GetStatusNameMapAsync().ConfigureAwait(false);
                    return;
                }

                await LoadResultsAsync(pageNumber, filter).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ErrorMessage = IntegrationPageResultsSupport.FormatLoadError(
                    ex,
                    SystemInfo,
                    _logger,
                    $"Failed to load {SystemInfo.DisplayName} messages.");
            }
        }

        /// <summary>
        /// Обновляет только панель результатов через partial view.
        /// </summary>
        /// <param name="pageNumber">Номер запрашиваемой страницы результатов.</param>
        /// <returns>Partial view панели результатов.</returns>
        public async Task<IActionResult> OnGetResultsAsync(int pageNumber = 1)
        {
            ResultsErrorMessage = null;

            try
            {
                OperationKeys = await MessageService.GetOperationKeysAsync().ConfigureAwait(false);

                if (!TryBuildFilter(OperationKeys, out var filter))
                {
                    StatusNames = await MessageService.GetStatusNameMapAsync().ConfigureAwait(false);
                    return Partial("_IntegrationResultsPanel", this);
                }

                await LoadResultsAsync(pageNumber, filter).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ResultsErrorMessage = IntegrationPageResultsSupport.FormatLoadError(
                    ex,
                    SystemInfo,
                    _logger,
                    $"Failed to refresh {SystemInfo.DisplayName} results.");
            }

            return Partial("_IntegrationResultsPanel", this);
        }

        /// <inheritdoc />
        public string ResolveStatusName(int? statusId)
        {
            if (!statusId.HasValue)
            {
                return "—";
            }

            return StatusNames.TryGetValue(statusId.Value, out var name) ? name : statusId.Value.ToString();
        }

        /// <inheritdoc />
        public IEnumerable<int> GetVisiblePageNumbers()
        {
            if (TotalPages <= 0)
            {
                yield break;
            }

            const int window = 2;
            var from = Math.Max(1, PageNumber - window);
            var to = Math.Min(TotalPages, PageNumber + window);

            for (var p = from; p <= to; p++)
            {
                yield return p;
            }
        }

        private async Task LoadResultsAsync(int pageNumber, TFilter filter)
        {
            var state = new IntegrationResultsState();
            await IntegrationPageResultsSupport.LoadResultsAsync(
                MessageService,
                pageNumber,
                PageSize,
                filter,
                state).ConfigureAwait(false);

            _messageGroups = (IReadOnlyList<TMessage>)state.MessageGroups;
            TotalCount = state.TotalCount;
            PageNumber = state.PageNumber;
            TotalPages = state.TotalPages;
            HasPrevious = state.HasPrevious;
            HasNext = state.HasNext;
            RangeFrom = state.RangeFrom;
            RangeTo = state.RangeTo;
            StatusNames = state.StatusNames;
        }

        private bool TryBuildFilter(IReadOnlyList<string> operationKeys, out TFilter filter)
        {
            var binding = new IntegrationFilterBinding
            {
                Id = FilterId,
                OperationKey = FilterOperationKey,
                HasError = FilterHasError,
                HasSendMessage = FilterHasSendMessage,
                CreatedFrom = FilterCreatedFrom,
                CreatedTo = FilterCreatedTo,
            };

            if (!IntegrationFilterBuilder.TryBuild(binding, operationKeys, out filter, out var errors, out var hasActiveFilter))
            {
                FilterIdError = errors.Id;
                FilterOperationKeyError = errors.OperationKey;
                FilterCreatedFromError = errors.CreatedFrom;
                FilterCreatedToError = errors.CreatedTo;
                FilterCreatedRangeError = errors.CreatedRange;
                return false;
            }

            HasActiveFilter = hasActiveFilter;
            return true;
        }
    }
}
