using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public IReadOnlyList<ExportMessageRabbitKka> MessageGroups { get; private set; }
            = Array.Empty<ExportMessageRabbitKka>();

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

        public string ErrorMessage { get; private set; }

        public async System.Threading.Tasks.Task OnGetAsync(int pageNumber = 1)
        {
            try
            {
                // Параметр не "page": в Razor Pages он зарезервирован для выбора .cshtml.
                pageNumber = Math.Max(1, pageNumber);

                var result = await _kkaMessageService
                    .GetMessagesPageAsync(pageNumber, PageSize)
                    .ConfigureAwait(false);

                if (result.TotalPages > 0 && pageNumber > result.TotalPages)
                {
                    pageNumber = result.TotalPages;
                    result = await _kkaMessageService
                        .GetMessagesPageAsync(pageNumber, PageSize)
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
    }
}
