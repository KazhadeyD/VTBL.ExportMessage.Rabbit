using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    public class NovaModel : PageModel, IIntegrationResultsPanelModel, IIntegrationCreatedFilterFields, IIntegrationDataErrorPage
    {
        public const int DefaultPageSize = 20;

        private readonly INovaMessageService _novaMessageService;
        private readonly ILogger<NovaModel> _logger;

        public NovaModel(INovaMessageService novaMessageService, ILogger<NovaModel> logger)
        {
            _novaMessageService = novaMessageService;
            _logger = logger;
        }

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

        public IReadOnlyList<ExportMessageRabbitNova> MessageGroups { get; private set; }
            = Array.Empty<ExportMessageRabbitNova>();

        IEnumerable IIntegrationResultsPanelModel.MessageGroups => MessageGroups;

        public IReadOnlyList<string> OperationKeys { get; private set; }
            = Array.Empty<string>();

        public IReadOnlyDictionary<int, string> StatusNames { get; private set; }
            = new Dictionary<int, string>();

        public string PageName => "Nova";

        public int PageNumber { get; private set; } = 1;

        public int PageSize { get; } = DefaultPageSize;

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
                OperationKeys = await _novaMessageService.GetOperationKeysAsync().ConfigureAwait(false);

                if (!TryBuildFilter(OperationKeys, out var filter))
                {
                    StatusNames = await _novaMessageService.GetStatusNameMapAsync().ConfigureAwait(false);
                    return;
                }

                await LoadResultsAsync(pageNumber, filter).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ErrorMessage = IntegrationPageResultsSupport.FormatLoadError(
                    ex,
                    IntegrationSystemInfo.Nova,
                    _logger,
                    "Failed to load NOVA messages.");
            }
        }

        public async Task<IActionResult> OnGetResultsAsync(int pageNumber = 1)
        {
            ResultsErrorMessage = null;

            try
            {
                OperationKeys = await _novaMessageService.GetOperationKeysAsync().ConfigureAwait(false);

                if (!TryBuildFilter(OperationKeys, out var filter))
                {
                    StatusNames = await _novaMessageService.GetStatusNameMapAsync().ConfigureAwait(false);
                    return Partial("_IntegrationResultsPanel", this);
                }

                await LoadResultsAsync(pageNumber, filter).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ResultsErrorMessage = IntegrationPageResultsSupport.FormatLoadError(
                    ex,
                    IntegrationSystemInfo.Nova,
                    _logger,
                    "Failed to refresh NOVA results.");
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

        private async Task LoadResultsAsync(int pageNumber, NovaMessageFilter filter)
        {
            var state = new IntegrationResultsState();
            await IntegrationPageResultsSupport.LoadResultsAsync(
                _novaMessageService,
                pageNumber,
                PageSize,
                filter,
                state).ConfigureAwait(false);

            MessageGroups = (IReadOnlyList<ExportMessageRabbitNova>)state.MessageGroups;
            TotalCount = state.TotalCount;
            PageNumber = state.PageNumber;
            TotalPages = state.TotalPages;
            HasPrevious = state.HasPrevious;
            HasNext = state.HasNext;
            RangeFrom = state.RangeFrom;
            RangeTo = state.RangeTo;
            StatusNames = state.StatusNames;
        }

        private bool TryBuildFilter(IReadOnlyList<string> operationKeys, out NovaMessageFilter filter)
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
