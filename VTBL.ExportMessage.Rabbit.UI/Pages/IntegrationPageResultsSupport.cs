using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.UI.Models;
using VTBL.ExportMessage.Rabbit.UI.Services;

namespace VTBL.ExportMessage.Rabbit.UI.Pages
{
    internal static class IntegrationPageResultsSupport
    {
        public static async Task LoadResultsAsync<TMessage>(
            IIntegrationMessageService<TMessage> messageService,
            int pageNumber,
            int pageSize,
            IntegrationMessageFilter filter,
            IntegrationResultsState state)
            where TMessage : class
        {
            pageNumber = Math.Max(1, pageNumber);

            var result = await messageService
                .GetMessagesPageAsync(pageNumber, pageSize, filter)
                .ConfigureAwait(false);

            if (result.TotalPages > 0 && pageNumber > result.TotalPages)
            {
                pageNumber = result.TotalPages;
                result = await messageService
                    .GetMessagesPageAsync(pageNumber, pageSize, filter)
                    .ConfigureAwait(false);
            }

            state.MessageGroups = result.Items;
            state.TotalCount = result.TotalCount;
            state.PageNumber = result.Page;
            state.TotalPages = result.TotalPages;
            state.HasPrevious = result.HasPrevious;
            state.HasNext = result.HasNext;
            state.RangeFrom = result.RangeFrom;
            state.RangeTo = result.RangeTo;

            state.StatusNames = await messageService.GetStatusNameMapAsync().ConfigureAwait(false);
        }

        public static string FormatLoadError(
            Exception exception,
            IntegrationSystemDescriptor system,
            ILogger logger,
            string logMessage)
        {
            logger.LogError(exception, logMessage);
            return IntegrationDatabaseErrorFormatter.ToUserMessage(exception, system);
        }
    }

    internal class IntegrationResultsState
    {
        public object MessageGroups { get; set; }

        public int TotalCount { get; set; }

        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }

        public bool HasPrevious { get; set; }

        public bool HasNext { get; set; }

        public int RangeFrom { get; set; }

        public int RangeTo { get; set; }

        public System.Collections.Generic.IReadOnlyDictionary<int, string> StatusNames { get; set; }
            = new System.Collections.Generic.Dictionary<int, string>();
    }
}
