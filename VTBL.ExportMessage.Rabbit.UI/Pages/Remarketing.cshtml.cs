using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    public class RemarketingModel : PageModel, IIntegrationPagingModel, IIntegrationCreatedFilterFields
    {
        public const int DefaultPageSize = 20;

        private readonly IRemarketingMessageService _remarketingMessageService;
        private readonly ILogger<RemarketingModel> _logger;

        public RemarketingModel(
            IRemarketingMessageService remarketingMessageService,
            ILogger<RemarketingModel> logger)
        {
            _remarketingMessageService = remarketingMessageService;
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

        public IReadOnlyList<ExportMessageRabbitRemarketing> MessageGroups { get; private set; }
            = Array.Empty<ExportMessageRabbitRemarketing>();

        public IReadOnlyList<string> OperationKeys { get; private set; }
            = Array.Empty<string>();

        public IReadOnlyDictionary<int, string> StatusNames { get; private set; }
            = new Dictionary<int, string>();

        public string PageName => "Remarketing";

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
                pageNumber = Math.Max(1, pageNumber);
                OperationKeys = await _remarketingMessageService.GetOperationKeysAsync().ConfigureAwait(false);

                if (!TryBuildFilter(OperationKeys, out var filter))
                {
                    StatusNames = await _remarketingMessageService.GetStatusNameMapAsync().ConfigureAwait(false);
                    return;
                }

                var result = await _remarketingMessageService
                    .GetMessagesPageAsync(pageNumber, PageSize, filter)
                    .ConfigureAwait(false);

                if (result.TotalPages > 0 && pageNumber > result.TotalPages)
                {
                    pageNumber = result.TotalPages;
                    result = await _remarketingMessageService
                        .GetMessagesPageAsync(pageNumber, PageSize, filter)
                        .ConfigureAwait(false);
                }

                MessageGroups = result.Items;
                TotalCount = result.TotalCount;
                PageNumber = result.Page;
                TotalPages = result.TotalPages;
                HasPrevious = result.HasPrevious;
                HasNext = result.HasNext;
                RangeFrom = result.RangeFrom;
                RangeTo = result.RangeTo;

                StatusNames = await _remarketingMessageService.GetStatusNameMapAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load Remarketing messages.");
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

        private bool TryBuildFilter(IReadOnlyList<string> operationKeys, out RemarketingMessageFilter filter)
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
