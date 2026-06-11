using System.Collections;
using System.Collections.Generic;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public interface IIntegrationPageModel : IIntegrationPagingModel
    {
        string PageTitle { get; }

        bool FilterHasError { get; }

        bool FilterHasSendMessage { get; }

        string FilterId { get; }

        string FilterOperationKey { get; }

        string FilterCreatedFrom { get; }

        string FilterCreatedTo { get; }

        string FilterIdError { get; }

        string FilterOperationKeyError { get; }

        string FilterCreatedFromError { get; }

        string FilterCreatedToError { get; }

        string FilterCreatedRangeError { get; }

        string ErrorMessage { get; }

        string ResultsErrorMessage { get; }

        bool HasActiveFilter { get; }

        int TotalCount { get; }

        int RangeFrom { get; }

        int RangeTo { get; }

        IReadOnlyList<string> OperationKeys { get; }

        IEnumerable MessageGroups { get; }

        string ResolveStatusName(int? statusId);
    }
}
