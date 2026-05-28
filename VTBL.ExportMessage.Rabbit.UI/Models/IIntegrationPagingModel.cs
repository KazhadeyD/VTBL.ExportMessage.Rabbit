using System.Collections.Generic;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public interface IIntegrationPagingModel
    {
        string PageName { get; }

        int PageNumber { get; }

        int TotalPages { get; }

        bool HasPrevious { get; }

        bool HasNext { get; }

        string FilterIdForRoute { get; }

        string FilterOperationKeyForRoute { get; }

        bool? FilterHasErrorForRoute { get; }

        bool? FilterHasSendMessageForRoute { get; }

        IEnumerable<int> GetVisiblePageNumbers();
    }
}
