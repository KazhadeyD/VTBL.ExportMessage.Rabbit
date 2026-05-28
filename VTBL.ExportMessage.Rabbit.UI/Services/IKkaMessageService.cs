using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.UI.Models;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public interface IKkaMessageService
    {
        Task<KkaMessagesPageResult> GetMessagesPageAsync(
            int page,
            int pageSize,
            KkaMessageFilter filter = null,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyDictionary<int, string>> GetStatusNameMapAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<string>> GetOperationKeysAsync(
            CancellationToken cancellationToken = default);

        Task<KkaDashboardStats> GetDashboardStatsAsync(
            CancellationToken cancellationToken = default);
    }
}
