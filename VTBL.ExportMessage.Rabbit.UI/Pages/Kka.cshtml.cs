using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    public class KkaModel : PageModel
    {
        public const int DefaultPageSize = 20;

        private readonly IKkaMessageService _kkaMessageService;
        private readonly ILogger<KkaModel> _logger;

        public KkaModel(IKkaMessageService kkaMessageService, ILogger<KkaModel> logger)
        {
            _kkaMessageService = kkaMessageService;
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

        public IReadOnlyList<ExportMessageRabbitKka> MessageGroups { get; private set; }
            = Array.Empty<ExportMessageRabbitKka>();

        public IReadOnlyList<string> OperationKeys { get; private set; }
            = Array.Empty<string>();

        public IReadOnlyDictionary<int, string> StatusNames { get; private set; }
            = new Dictionary<int, string>();

        public int Page { get; private set; } = 1;

        public int PageSize { get; } = DefaultPageSize;

        public int TotalCount { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPrevious { get; private set; }

        public bool HasNext { get; private set; }

        public int RangeFrom { get; private set; }

        public int RangeTo { get; private set; }

        public string FilterIdError { get; private set; }

        public string FilterOperationKeyError { get; private set; }

        public bool HasActiveFilter { get; private set; }

        public string ErrorMessage { get; private set; }

        public string FilterIdForRoute =>
            string.IsNullOrWhiteSpace(FilterId) ? null : FilterId.Trim();

        public string FilterOperationKeyForRoute =>
            string.IsNullOrWhiteSpace(FilterOperationKey) ? null : FilterOperationKey.Trim();

        public bool? FilterHasErrorForRoute => FilterHasError ? true : (bool?)null;

        public bool? FilterHasSendMessageForRoute => FilterHasSendMessage ? true : (bool?)null;

        public async System.Threading.Tasks.Task OnGetAsync(int pageNumber = 1)
        {
            try
            {
                pageNumber = Math.Max(1, pageNumber);
                OperationKeys = await _kkaMessageService.GetOperationKeysAsync().ConfigureAwait(false);

                if (!TryBuildFilter(OperationKeys, out var filter))
                {
                    StatusNames = await _kkaMessageService.GetStatusNameMapAsync().ConfigureAwait(false);
                    return;
                }

                var result = await _kkaMessageService
                    .GetMessagesPageAsync(pageNumber, PageSize, filter)
                    .ConfigureAwait(false);

                if (result.TotalPages > 0 && pageNumber > result.TotalPages)
                {
                    pageNumber = result.TotalPages;
                    result = await _kkaMessageService
                        .GetMessagesPageAsync(pageNumber, PageSize, filter)
                        .ConfigureAwait(false);
                }

                MessageGroups = result.Items;
                TotalCount = result.TotalCount;
                Page = result.Page;
                TotalPages = result.TotalPages;
                HasPrevious = result.HasPrevious;
                HasNext = result.HasNext;
                RangeFrom = result.RangeFrom;
                RangeTo = result.RangeTo;

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

        public IEnumerable<int> GetVisiblePageNumbers()
        {
            if (TotalPages <= 0)
            {
                yield break;
            }

            const int window = 2;
            var from = Math.Max(1, Page - window);
            var to = Math.Min(TotalPages, Page + window);

            for (var p = from; p <= to; p++)
            {
                yield return p;
            }
        }

        private bool TryBuildFilter(IReadOnlyList<string> operationKeys, out KkaMessageFilter filter)
        {
            filter = null;
            var hasId = !string.IsNullOrWhiteSpace(FilterId);
            var hasOperationKey = !string.IsNullOrWhiteSpace(FilterOperationKey);

            if (!hasId && !hasOperationKey && !FilterHasError && !FilterHasSendMessage)
            {
                return true;
            }

            HasActiveFilter = true;
            filter = new KkaMessageFilter
            {
                WithError = FilterHasError,
                WithSendMessage = FilterHasSendMessage,
            };

            if (hasId)
            {
                if (Guid.TryParse(FilterId.Trim(), out var parsedId))
                {
                    filter.Id = parsedId;
                }
                else
                {
                    FilterIdError = "Некорректный формат Id. Укажите GUID, например: CA42A29F-4D8B-4428-9D43-20F9597C615F";
                    return false;
                }
            }

            if (hasOperationKey)
            {
                var key = FilterOperationKey.Trim();
                if (operationKeys.Contains(key, StringComparer.Ordinal))
                {
                    filter.OperationKey = key;
                }
                else
                {
                    FilterOperationKeyError = "Выберите OperationKey из списка.";
                    return false;
                }
            }

            return true;
        }
    }
}
