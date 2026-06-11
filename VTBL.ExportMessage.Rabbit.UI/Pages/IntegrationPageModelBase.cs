using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    public abstract class IntegrationPageModelBase<TMessage, TFilter> : PageModel, IIntegrationPageModel
        where TMessage : class
        where TFilter : IntegrationMessageFilter, new()
    {
        private readonly ILogger _logger;
        private IReadOnlyList<TMessage> _messageGroups = Array.Empty<TMessage>();

        protected IntegrationPageModelBase(ILogger logger)
        {
            _logger = logger;
        }

        protected abstract IIntegrationMessageService<TMessage> MessageService { get; }

        protected abstract IntegrationSystemDescriptor SystemInfo { get; }

        public abstract string PageName { get; }

        [BindProperty(SupportsGet = true, Name = "id")]
        public string FilterId { get; set; }

        [BindProperty(SupportsGet = true, Name = "operationKey")]
        public string FilterOperationKey { get; set; }

        [BindProperty(SupportsGet = true, Name = "hasError")]
        public bool FilterHasError { get; set; }

        [BindProperty(SupportsGet = true, Name = "hasSendMessage")]
        public bool FilterHasSendMessage { get; set; }

        [BindProperty(SupportsGet = true, Name = "createdFrom")]
        public string FilterCreatedFrom { get; set; }

        [BindProperty(SupportsGet = true, Name = "createdTo")]
        public string FilterCreatedTo { get; set; }

        [BindProperty(SupportsGet = true, Name = "pageSize")]
        public int? FilterPageSize { get; set; }

        public IReadOnlyList<string> OperationKeys { get; private set; }
            = Array.Empty<string>();

        IEnumerable IIntegrationPageModel.MessageGroups => _messageGroups;

        public IReadOnlyDictionary<int, string> StatusNames { get; private set; }
            = new Dictionary<int, string>();

        public int PageNumber { get; private set; } = 1;

        public int PageSize => IntegrationPageSize.Normalize(FilterPageSize);

        public int TotalCount { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPrevious { get; private set; }

        public bool HasNext { get; private set; }

        public int RangeFrom { get; private set; }

        public int RangeTo { get; private set; }

        public string FilterIdError { get; private set; }

        public string FilterOperationKeyError { get; private set; }

        public string FilterCreatedFromError { get; private set; }

        public string FilterCreatedToError { get; private set; }

        public string FilterCreatedRangeError { get; private set; }

        public bool HasActiveFilter { get; private set; }

        public string ErrorMessage { get; private set; }

        public string ResultsErrorMessage { get; private set; }

        public string FilterIdForRoute =>
            string.IsNullOrWhiteSpace(FilterId) ? null : FilterId.Trim();

        public string FilterOperationKeyForRoute =>
            string.IsNullOrWhiteSpace(FilterOperationKey) ? null : FilterOperationKey.Trim();

        public bool? FilterHasErrorForRoute => FilterHasError ? true : (bool?)null;

        public bool? FilterHasSendMessageForRoute => FilterHasSendMessage ? true : (bool?)null;

        public string FilterCreatedFromForRoute =>
            string.IsNullOrWhiteSpace(FilterCreatedFrom) ? null : FilterCreatedFrom.Trim();

        public string FilterCreatedToForRoute =>
            string.IsNullOrWhiteSpace(FilterCreatedTo) ? null : FilterCreatedTo.Trim();

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

        public string ResolveStatusName(int? statusId)
        {
            if (!statusId.HasValue)
            {
                return "—";
            }

            return StatusNames.TryGetValue(statusId.Value, out var name) ? name : statusId.Value.ToString();
        }

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
