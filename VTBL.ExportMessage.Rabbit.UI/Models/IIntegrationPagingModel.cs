using System.Collections.Generic;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public interface IIntegrationPagingModel
    {
        string PageName { get; }

        int PageNumber { get; }

        int PageSize { get; }

        int TotalPages { get; }

        bool HasPrevious { get; }

        bool HasNext { get; }

        string FilterIdForRoute { get; }

        string FilterOperationKeyForRoute { get; }

        bool? FilterHasErrorForRoute { get; }

        bool? FilterHasSendMessageForRoute { get; }

        string FilterCreatedFromForRoute { get; }

        string FilterCreatedToForRoute { get; }

        IEnumerable<int> GetVisiblePageNumbers();
    }
}
