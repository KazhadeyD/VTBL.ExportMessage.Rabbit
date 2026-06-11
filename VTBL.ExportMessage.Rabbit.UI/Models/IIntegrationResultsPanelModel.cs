using System.Collections;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public interface IIntegrationResultsPanelModel : IIntegrationPagingModel
    {
        string ErrorMessage { get; }

        string ResultsErrorMessage { get; }

        bool HasActiveFilter { get; }

        int TotalCount { get; }

        int RangeFrom { get; }

        int RangeTo { get; }

        IEnumerable MessageGroups { get; }

        string ResolveStatusName(int? statusId);
    }
}
