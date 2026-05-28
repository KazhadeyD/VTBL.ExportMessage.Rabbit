using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.UI.Models;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public interface IIntegrationMessageService<TMessage>
    {
        Task<IntegrationMessagesPageResult<TMessage>> GetMessagesPageAsync(
            int page,
            int pageSize,
            IntegrationMessageFilter filter = null,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyDictionary<int, string>> GetStatusNameMapAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<string>> GetOperationKeysAsync(
            CancellationToken cancellationToken = default);

        Task<IntegrationDashboardStats> GetDashboardStatsAsync(
            CancellationToken cancellationToken = default);
    }
}
