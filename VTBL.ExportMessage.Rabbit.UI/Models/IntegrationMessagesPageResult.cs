using System;
using System.Collections.Generic;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public class IntegrationMessagesPageResult<TMessage>
    {
        public IReadOnlyList<TMessage> Items { get; set; } = Array.Empty<TMessage>();

        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize)
            : 0;

        public bool HasPrevious => Page > 1;

        public bool HasNext => Page < TotalPages;

        public int RangeFrom => TotalCount == 0 ? 0 : ((Page - 1) * PageSize) + 1;

        public int RangeTo => TotalCount == 0 ? 0 : Math.Min(Page * PageSize, TotalCount);
    }
}
